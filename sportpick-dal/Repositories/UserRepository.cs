using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_dal
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserProvider _userProvider;
        public UserRepository(IUserProvider userProvider){
            _userProvider = userProvider;
        }

        public User? GetByUsername(string username){
            var userInDb = _userProvider.GetByUsername(username);
            
            if (userInDb != null){
                return UserMapper.ToDomain(userInDb);
            }

            return null;
        }

        public bool CreateUser(User newUser){
            if (newUser != null){
                var entity = UserMapper.ToEntity(newUser);
                return _userProvider.CreateUser(entity);
            }     
            return false;
        }
    }
}