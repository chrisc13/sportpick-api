using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_dal
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IProfileProvider _profileProvider;
        public ProfileRepository(IProfileProvider profileProvider){
            _profileProvider = profileProvider;
        }

        public async Task<Profile?> GetProfileByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var entity = await _profileProvider.GetByUsernameAsync(username);
            if (entity == null)
                return null;


            return ProfileMapper.ToDomain(entity);
        }

        public async Task<Profile?> GetByUserIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var entity = await _profileProvider.GetByUserIdAsync(id);
            if (entity == null)
                return null;


            return ProfileMapper.ToDomain(entity);
        }
        public async Task<bool> UpsertProfileAsync(Profile profile){
            var updateEntity = ProfileMapper.ToEntity(profile); 
            return await _profileProvider.UpsertProfileAsync(updateEntity);
        }

        
    }
}
