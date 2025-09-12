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

        public async Task<AppUser?> GetByUsernameAsync(string username){
            var userInDb = await _userProvider.GetByUsernameAsync(username);
            
            if (userInDb != null){
                return AppUserMapper.ToDomain(userInDb);
            }

            return null;
        }

        public async Task<AppUser?> CreateAppUserAsync(AppUser newAppUser){
            if (newAppUser != null){
                var entity = AppUserMapper.ToEntity(newAppUser);
                var result = await _userProvider.CreateAppUserAsync(entity);
                return AppUserMapper.ToDomain(result);
            }     
            return null;
        }
    }
}