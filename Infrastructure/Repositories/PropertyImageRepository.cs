using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PropertyImageRepository : BaseRepository<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(MongoDbContext context)
            :base(context, "PropertyImage") { }

        public async Task<List<PropertyImage>> GetImagesByPropertyId(string propertyId){
            var filter = Builders<PropertyImage>.Filter.Eq(pi => pi.IdProperty, propertyId);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }
    }
}
