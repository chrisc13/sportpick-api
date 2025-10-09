using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_bll
{
    public interface IAuthService
    {
        Task<AppUser?> LoginAsync(string username, string password);
        Task<AppUser> RegisterAsync(string username, string password);
    }
}