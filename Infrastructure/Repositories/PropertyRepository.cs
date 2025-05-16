using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(MongoDbContext context)
            :base(context, "Properties") { }

        public async Task<List<Property>> SearchAsync(string? name, string? address, decimal? priceFrom, decimal? priceTo)
        {
            var filterBuilder = Builders<Property>.Filter;
            var filters = new List<FilterDefinition<Property>>();

            if (!string.IsNullOrEmpty(name))
                filters.Add(filterBuilder.Regex(x => x.Name, new BsonRegularExpression(name, "i")));

            if (!string.IsNullOrEmpty(address))
                filters.Add(filterBuilder.Regex(x => x.Address, new BsonRegularExpression(address, "i")));

            if (priceFrom.HasValue)
                filters.Add(filterBuilder.Gte(x => x.Price, priceFrom.Value));

            if (priceTo.HasValue)
                filters.Add(filterBuilder.Lte(x => x.Price, priceTo.Value));

            var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<Property>.Empty;

            return await _collection.Find(combinedFilter).ToListAsync();
        }
    }
}
