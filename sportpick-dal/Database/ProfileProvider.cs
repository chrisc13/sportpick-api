using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sportpick_dal
{
    public class ProfileProvider : IProfileProvider
    {
        private readonly IMongoCollection<ProfileEntity> _profiles;

        public ProfileProvider(IDatabaseProvider databaseProvider)
        {
            _profiles = databaseProvider.GetCollection<ProfileEntity>("profiles");
        }

        public async Task<ProfileEntity?> GetByUsernameAsync(string username)
        {
            return await _profiles
                .Find(p => p.Username == username)
                .FirstOrDefaultAsync();
        }

        public async Task<ProfileEntity?> GetByUserIdAsync(string userId)
        {
            return await _profiles
                .Find(p => p.Id == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpsertProfileAsync(ProfileEntity profile)
        {
            var filter = Builders<ProfileEntity>.Filter.Eq(p => p.Id, profile.Id);

            var result = await _profiles.ReplaceOneAsync(
                filter,
                profile,
                new ReplaceOptions { IsUpsert = true }
            );

            // Returns true if something was modified or inserted
            return result.IsAcknowledged && (result.ModifiedCount > 0 || result.UpsertedId != null);
        }


        public async Task EnsureIndexesAsync()
        {
            // Unique index on Username
            var usernameIndex = new CreateIndexModel<ProfileEntity>(
                Builders<ProfileEntity>.IndexKeys.Ascending(p => p.Username),
                new CreateIndexOptions { Unique = true }
            );

            await _profiles.Indexes.CreateOneAsync(usernameIndex);
        }
    }
}
