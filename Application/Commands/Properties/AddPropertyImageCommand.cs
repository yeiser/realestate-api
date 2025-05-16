using MediatR;

namespace Application.Commands.Properties
{
    public class AddPropertyImageCommand : IRequest<string>
    {
        public string PropertyId { get; set; } = default!;
        public string Base64Image { get; set; } = default!;
    }
}
