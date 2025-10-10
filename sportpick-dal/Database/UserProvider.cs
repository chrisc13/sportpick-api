using MongoDB.Driver;
using System;
using System.Collections.Generic;


namespace sportpick_dal
{
    public class AppUserProvider : IAppUserProvider
    {
        private readonly IMongoCollection<AppUserEntity> _users;
        public AppUserProvider(IDatabaseProvider databaseProvider){
            _users = databaseProvider.GetCollection<AppUserEntity>("users");
        }

        public async Task<AppUserEntity?> GetByUsernameAsync(string username)
        {
                 return await _users
                    .Find(u => u.Username == username)
                    .FirstOrDefaultAsync();
        }

        public async Task<AppUserEntity?> CreateAppUserAsync(AppUserEntity newAppUser)
        {
            try
            {
                await _users.InsertOneAsync(newAppUser);
                return newAppUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo create user error: {ex.Message}");
                return null;
            }
        }
        public async Task<IEnumerable<AppUserEntity>> GetByUsernamesAsync(IEnumerable<string> usernames)
        {
            var filter = Builders<AppUserEntity>.Filter.In(u => u.Username, usernames);
            var users = await _users.Find(filter).ToListAsync();
            return users;
        }

    }
}