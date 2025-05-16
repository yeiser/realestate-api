using MediatR;

namespace Application.Commands.Properties
{
    public class UpdatePropertyCommand : IRequest<bool>
    {
        public string IdProperty { get; set; } = default!;
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal? Price { get; set; }
        public string? CodeInternal { get; set; }
        public int Year { get; set; }
        public string? IdOwner { get; set; }
    }
}
