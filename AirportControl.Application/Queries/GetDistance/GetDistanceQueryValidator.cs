using AirportControl.Application.Queries.GetDistance.Contracts;
using FluentValidation;

namespace AirportControl.Application.Queries.GetDistance;

public sealed class GetDistanceQueryValidator : AbstractValidator<IGetDistanceQuery>
{
    public GetDistanceQueryValidator()
    {
        RuleFor(q => q.Source)
            .NotEmpty()
            .Length(Constants.Codes.Length)
            .WithName("Source airport");

        RuleFor(q => q)
            .Must(value => value.Source.All(char.IsUpper) && value.Target.All(char.IsUpper))
            .WithMessage("Airport code should be in uppercase");

        RuleFor(q => q.Target)
            .NotEmpty()
            .Length(Constants.Codes.Length)
            .WithName("Target airport");

        RuleFor(q => q)
            .Must(q => !string.Equals(q.Source, q.Target, StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Need to set two unique airports");
    }
}