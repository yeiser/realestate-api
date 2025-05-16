using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IOwnerRepository
    {
        Task<Owner?> GetByIdAsync(string id);
        Task<List<Owner>> GetAllAsync();
        Task InsertAsync(Owner owner);
        Task UpdateAsync(string id, Owner owner);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<List<Owner>> FindByPropertyAsync(Expression<Func<Owner, string>> propertySelector, string value);
    }
}
