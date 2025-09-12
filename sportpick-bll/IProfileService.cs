using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_bll
{
    public interface IProfileService
    {
        Task<Profile?> GetProfileByUsernameAsync(string username);
        Task<Profile?> GetProfileByUserIdAsync(string id);
        Task<bool> UpsertProfileAsync(Profile profile);
    }
}