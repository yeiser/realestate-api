using Application.Commands.Owners;
using Application.DTOs;
using MediatR;

namespace Application.Queries.Owners
{
    public record GetOwnerByNameQuery(string Name) : IRequest<List<OwnerDto>>;
}
