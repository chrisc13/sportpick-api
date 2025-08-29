using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sportpick_domain;

namespace sportpick_api.DTO
{
    public class UserResponse
    {
        public string Username { get; set; }
        public UserResponse(string username){
            this.Username = username;
        }
    }


    public class AuthResponse
    {
        public UserResponse AppUser { get; set; }

        public string Token {get; set;}

        public string Message {get; set;}

        public AuthResponse(UserResponse user, string token, string message){
            this.AppUser = user;
            this.Token = token;
            this.Message = message;
        }

    }
}