using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IPropertyImageRepository
    {
        Task InsertAsync(PropertyImage image);
        Task<List<PropertyImage>> GetImagesByPropertyId(string value);
    }
}
