using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_dal;

public interface IAppUserRepository
{
    AppUser? GetByUsername(string username);
    bool CreateAppUser(AppUser newAppUser);
}
