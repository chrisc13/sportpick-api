using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_bll
{
    public interface IAuthService
    {
        User? Login(User user);
        bool Register(User request);

    }
}