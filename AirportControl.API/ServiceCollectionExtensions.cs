using System.Net;
using System.Reflection;
using System.Text.Json;
using AirportControl.API.Contracts.Queries;
using AirportControl.Application.Pipelines;
using AirportControl.Application.Queries.GetDistance;
using AirportControl.Application.Queries.GetDistance.Contracts;
using AirportControl.CacheClient;
using AirportControl.CteleportClient;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;

namespace AirportControl.API;

internal static class ServiceCollectionExtensions
{
    public static void Register(this IServiceCollection services, IConfiguration configuration)
    {
        var cteleportOptions = configuration
            .GetSection(nameof(CTeleportOptions))
            .Get<CTeleportOptions>();

        var cacheOptions = configuration
            .GetSection(nameof(CacheOptions))
            .Get<CacheOptions>();
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en");

        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddScoped<IRequestHandler<GetDistanceQuery, Distance>, GetDistanceQueryHandler>()
            .AddScoped<IValidator<GetDistanceQuery>, GetDistanceQueryValidator>()
            .AddCTeleportClient(cteleportOptions)
            .AddCache(cacheOptions);
    }

    public static void AddExceptionHandling<TException>(
        this IApplicationBuilder app,
        HttpStatusCode code)
    where TException : Exception
    {
        var errors = new Dictionary<Type, HttpStatusCode>();
        if (app is null)
            throw new ArgumentNullException(nameof(app));

        errors.Add(typeof(TException), code);
        
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    ?.Error;
                
                if (exception is not null)
                {
                    if (errors.TryGetValue(exception.GetType(), out HttpStatusCode errorResult))
                    {
                        await ConfigureExceptionResponse(
                            context,
                            errorResult,
                            exception.Message);
                        return;
                    }

                    await ConfigureExceptionResponse(
                        context,
                        HttpStatusCode.InternalServerError,
                        exception.Message);
                }
            });
        });
    }
    
    private static async Task ConfigureExceptionResponse(
        HttpContext context,
        HttpStatusCode code,
        string message)
    {
        var errorMessage = code is HttpStatusCode.InternalServerError ? "Internal server error" : message;
        
        context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)code;

        var errorResult = new
        {
            Code = code,
            Message = errorMessage
        };
                        
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResult));
    }
}