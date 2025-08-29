using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using sportpick_dal;
using sportpick_domain;

namespace sportpick_bll
{
    public class AuthService : IAuthService
    {
        private readonly IAppUserRepository _userRepository;
        public AuthService(IAppUserRepository userRepository){
            _userRepository = userRepository;
        }

        public AppUser? Login(AppUser request){
            var user = _userRepository.GetByUsername(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password)){
                return null;
            }
            
             return new AppUser
            {
                Id = user.Id,
                Username = user.Username,
                Password = null
            };
        }

        public bool Register(AppUser request){
            var user = _userRepository.GetByUsername(request.Username);
            if (user != null){
                return false;
            }
            string hashed = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = hashed;

            return _userRepository.CreateAppUser(request);
        }


    }
}