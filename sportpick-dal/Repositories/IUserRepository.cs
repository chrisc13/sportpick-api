using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_dal;

public interface IUserRepository
{
    User? GetByUsername(string username);
    bool CreateUser(User newUser);
}
