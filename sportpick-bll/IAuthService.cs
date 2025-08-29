using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_bll
{
    public interface IAuthService
    {
        AppUser? Login(AppUser user);
        bool Register(AppUser request);

    }
}