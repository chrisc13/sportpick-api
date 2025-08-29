using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_dal;
public interface IAppUserProvider
{
    AppUserEntity? GetByUsername(string username);
    bool CreateAppUser(AppUserEntity newAppUser);
}
