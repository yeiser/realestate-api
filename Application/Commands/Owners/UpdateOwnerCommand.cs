using Application.DTOs;
using MediatR;

namespace Application.Commands.Owners
{
    public class UpdateOwnerCommand : IRequest<OwnerDto>
    {
        public string IdOwner { get; set; } = null!;
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
