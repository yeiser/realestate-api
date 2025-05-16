using Application.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Queries.PropertyImages
{
    public class GetImagesByPropertyIdQuery : IRequest<List<PropertyImage>>
    {
        public string Id { get; set; } = null!;
    }
}
