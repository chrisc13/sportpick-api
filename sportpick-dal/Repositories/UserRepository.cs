using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_dal
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly IAppUserProvider _userProvider;
        public AppUserRepository(IAppUserProvider userProvider){
            _userProvider = userProvider;
        }

        public AppUser? GetByUsername(string username){
            var userInDb = _userProvider.GetByUsername(username);
            
            if (userInDb != null){
                return AppUserMapper.ToDomain(userInDb);
            }

            return null;
        }

        public bool CreateAppUser(AppUser newAppUser){
            if (newAppUser != null){
                var entity = AppUserMapper.ToEntity(newAppUser);
                return _userProvider.CreateAppUser(entity);
            }     
            return false;
        }
    }
}