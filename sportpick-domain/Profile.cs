using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sportpick_domain
{
    public class Profile
    {
        public string? Id { get; set; }          // FK to AppUserEntity
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Username { get; set; }        // public handle
        public string Bio { get; set; }
        public string? Location {get; set;}
        public double? Latitude { get; set; }          
        public double? Longitude { get; set; }     
        public string ProfileImageUrl {get; set;}
        public Dictionary<string, string> SportLevel { get; set; } = new();
    }
}