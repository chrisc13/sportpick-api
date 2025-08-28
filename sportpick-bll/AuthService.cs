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
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository){
            _userRepository = userRepository;
        }

        public User? Login(User request){
            var user = _userRepository.GetByUsername(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password)){
                return null;
            }
            
             return new User
            {
                Username = user.Username,
                Password = null
            };
        }

        public bool Register(User request){
            var user = _userRepository.GetByUsername(request.Username);
            if (user != null){
                return false;
            }
            string hashed = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = hashed;

            return _userRepository.CreateUser(request);
        }


    }
}