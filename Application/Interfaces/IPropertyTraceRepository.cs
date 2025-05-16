using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPropertyTraceRepository
    {
        Task InsertAsync(PropertyTrace traceProperty);
    }
}
