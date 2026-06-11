using MediatR;

namespace ReservEasy.Api.Features.Properties.Get;

public record GetPropertyQuery(Guid Id) : IRequest<GetPropertyResponse>;
