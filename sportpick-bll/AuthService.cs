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
        private readonly IProfileProvider _profileProvider;

        public AuthService(IAppUserRepository userRepository, IProfileProvider profileProvider){
            _userRepository = userRepository;
            _profileProvider = profileProvider;
        }

        public async Task<AppUser?> LoginAsync(AppUser request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var user = await _userRepository.GetByUsernameAsync(request.Username);

            // Ensure user exists and stored password is not null/empty
            if (user == null || string.IsNullOrEmpty(user.Password))
                return null;

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return null;

            return new AppUser
            {
                Id = user.Id,
                Username = user.Username,
                Password = null // don't return hashed password
            };
        }


        public async Task<AppUser?> RegisterAsync(AppUser request)
        {
            // Validate input
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username cannot be empty", nameof(request.Username));
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password cannot be empty", nameof(request.Password));

            // Check if user already exists
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                return null;

            // Hash password
            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create new user
            var newUser = await _userRepository.CreateAppUserAsync(request);
            if (newUser == null)
                return null;

            // Create default profile
            var profile = new ProfileEntity
            {
                Id = newUser.Id,
                Username = newUser.Username,
                FirstName = "",
                LastName = "",
                Bio = "",
                ProfileImageUrl = "/default-avatar.png",
                SportLevel = new Dictionary<string, string>()
            };

            await _profileProvider.UpsertProfileAsync(profile);

            return newUser;
        }
    }
}