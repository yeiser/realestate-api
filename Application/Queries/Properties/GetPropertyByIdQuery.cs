using Application.DTOs;
using MediatR;

namespace Application.Queries.Properties
{
    public class GetPropertyByIdQuery : IRequest<PropertyDetailDto>
    {
        public string Id { get; set; } = null!;
    }
}
