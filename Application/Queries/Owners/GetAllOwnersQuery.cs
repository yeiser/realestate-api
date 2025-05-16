using Application.DTOs;
using MediatR;

namespace Application.Queries.Owners
{
    public record GetAllOwnersQuery() : IRequest<List<OwnerDto>>;
}
