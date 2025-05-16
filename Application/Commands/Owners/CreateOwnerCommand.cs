
using Application.DTOs;
using MediatR;

namespace Application.Commands.Owners
{
    public class CreateOwnerCommand : IRequest<OwnerDto>
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Photo { get; set; } = null!;
        public DateTime Birthday { get; set; }
    }
}
