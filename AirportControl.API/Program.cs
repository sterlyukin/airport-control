using System.Net;
using AirportControl.API;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel();
builder.Services.Register(builder.Configuration);

var app = builder.Build();
app.AddExceptionHandling<ValidationException>(HttpStatusCode.BadRequest);

app
    .UseHttpsRedirection()
    .UseRouting()
    .UseEndpoints(e =>
    {
        e.MapControllers();
    })
    .UseSwagger()
    .UseSwaggerUI();

app.Run();