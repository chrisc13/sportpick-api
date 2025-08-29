using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sportpick_bll
{
    public interface ITokenService
    {
        string GenerateToken(string username, string userId);
    }
}
