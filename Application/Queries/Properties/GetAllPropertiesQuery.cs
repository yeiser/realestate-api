using Application.DTOs;
using MediatR;

namespace Application.Queries.Properties
{
    public record GetAllPropertiesQuery() : IRequest<List<PropertyDto>>;
}
