using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetAllAsync();
        Task InsertAsync(Property property);
        Task<Property?> GetByIdAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<List<Property>> SearchAsync(string? name, string? address, decimal? priceFrom, decimal? priceTo);
        Task UpdateAsync(string id, Property property);
    }
}
