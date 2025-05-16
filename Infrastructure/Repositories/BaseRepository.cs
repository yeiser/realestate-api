using Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public BaseRepository(MongoDbContext dbContext, string collectionName)
        {
            _collection = dbContext.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            var className = typeof(T).Name;
            return await _collection.Find(Builders<T>.Filter.Eq($"Id{className}", id)).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, T entity)
        {
            var className = typeof(T).Name;
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq($"Id{className}", id), entity);
        }

        public async Task DeleteAsync(string id)
        {
            var className = typeof(T).Name;
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq($"Id{className}", id));
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var className = typeof(T).Name;
            return await _collection.Find(Builders<T>.Filter.Eq($"Id{className}", id)).AnyAsync();
        }

        public async Task<List<T>> FindByPropertyAsync(Expression<Func<T, string>> propertySelector, string value)
        {
            var builder = Builders<T>.Filter;
            var propertyName = ((MemberExpression)propertySelector.Body).Member.Name;

            var regex = new BsonRegularExpression(value, "i");
            var filter = builder.Regex(propertyName, regex);

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
