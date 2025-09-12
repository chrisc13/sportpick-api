using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_dal
{
    public interface IProfileRepository
    {
        Task<Profile?> GetProfileByUsernameAsync(string username);
        Task<Profile?> GetByUserIdAsync(string id);
        Task<bool> UpsertProfileAsync(Profile profile);
    }
}
