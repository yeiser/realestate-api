using Domain.Entities;
using MediatR;

namespace Application.Queries.Owners
{
    public record GetOwnerByIdQuery(string Id) : IRequest<Owner>;
}
