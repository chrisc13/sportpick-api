using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;
using sportpick_dal;


namespace sportpick_bll
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<Profile?> GetProfileByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var profile = await _profileRepository.GetProfileByUsernameAsync(username);
            return profile;
        }

        public async Task<Profile?> GetProfileByUserIdAsync(string id){
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var profile = await _profileRepository.GetByUserIdAsync(id);
            return profile;
        }
        public async Task<bool> UpsertProfileAsync(Profile profile){
            if (profile == null)
                return false;

            return await _profileRepository.UpsertProfileAsync(profile);
        }
    }
}