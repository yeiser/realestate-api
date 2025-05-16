using MediatR;

namespace Application.Commands.PropertyTraces
{
    public record CreatePropertyTraceCommand(
        string IdProperty,
        DateTime DateSale,
        string Name,
        decimal Value,
        decimal Tax
    ) : IRequest<string>;
}
