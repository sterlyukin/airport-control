using MediatR;

namespace AirportControl.Application.Queries.GetDistance.Contracts;

public interface IGetDistanceQuery : IRequest<Distance>
{
    string Source { get; }
    string Target { get; }
}