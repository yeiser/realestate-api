using Application.DTOs;
using MediatR;

namespace Application.Commands.Properties
{
    public class CreatePropertyWithImageCommand : IRequest<PropertyDto>
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public string? CodeInternal { get; set; }
        public string IdOwner { get; set; } = null!;
        public string? ImageBase64 { get; set; }
    }
}
