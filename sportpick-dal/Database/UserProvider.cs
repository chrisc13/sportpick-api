using MongoDB.Driver;
using System;
using System.Collections.Generic;


namespace sportpick_dal
{
    public class UserProvider : IUserProvider
    {
        private readonly IMongoCollection<UserEntity> _users;
        public UserProvider(IDatabaseProvider databaseProvider){
            _users = databaseProvider.GetCollection<UserEntity>("users");
        }

        public UserEntity? GetByUsername(string username){
             var user = _users
                .Find(u => u.Username == username)
                .Project(u => new UserEntity { 
                    Username = u.Username, 
                    Password = u.Password 
                })
                .FirstOrDefault();

            return user; // returns null if not found
        }

        public bool CreateUser(UserEntity newUser){
            try
            {
                _users.InsertOne(newUser);
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