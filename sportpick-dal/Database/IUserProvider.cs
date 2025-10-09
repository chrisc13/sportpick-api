using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_dal;
public interface IAppUserProvider
{
    Task<AppUserEntity?> GetByUsernameAsync(string username);
    Task<AppUserEntity?> CreateAppUserAsync(AppUserEntity newAppUser);
    Task<IEnumerable<AppUserEntity>> GetByIdsAsync(IEnumerable<string> userIds);
}
