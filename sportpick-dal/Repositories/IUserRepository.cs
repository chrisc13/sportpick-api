using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_dal;

public interface IAppUserRepository
{
    Task<AppUser?> GetByUsernameAsync(string username);
    Task<AppUser?> CreateAppUserAsync(string username, string password);
    Task<IEnumerable<AppUser>> GetUsersByIdsAsync(IEnumerable<string> userIds);
}
