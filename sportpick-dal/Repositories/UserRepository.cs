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
        private readonly string DEFAULT_IMAGE = "/default-avatar.png";

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

        public async Task<AppUser?> CreateAppUserAsync(string username, string password){
            var entity = new AppUserEntity();
            entity.Username = username;
            entity.Password = password;
            entity.ProfileImageUrl = DEFAULT_IMAGE;
            var result = await _userProvider.CreateAppUserAsync(entity);
            return AppUserMapper.ToDomain(result);
        }
        
        public async Task<IEnumerable<AppUser>> GetUsersByIdsAsync(IEnumerable<string> userIds)
        {
            var usersInDb = await _userProvider.GetByIdsAsync(userIds);
            return usersInDb.Select(AppUserMapper.ToDomain);
        }
    }
}