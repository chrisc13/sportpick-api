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

        public AppUserEntity? GetByUsername(string username){
             var user = _users
                .Find(u => u.Username == username)
                .Project(u => new AppUserEntity { 
                    Id = u.Id,
                    Username = u.Username, 
                    Password = u.Password 
                })
                .FirstOrDefault();

            return user; // returns null if not found
        }

        public bool CreateAppUser(AppUserEntity newAppUser){
            try
            {
                _users.InsertOne(newAppUser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo create user error: {ex.Message}");
                return false;
            }
        }
        
    }
}