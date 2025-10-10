using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;
using sportpick_dal;
using sportpick_domain;
using System.Linq;

namespace sportpick_bll
{
    public class AuthService : IAuthService
    {
        private readonly IAppUserRepository _userRepository;
        private readonly IProfileProvider _profileProvider;
        private const string DUPLICATE_USER = "A user with this username already exists.";
        private const string REGISTER_ERROR = "Error on register.";
        private const string REGISTER_SUCCESS = "Successfully registered";

        public AuthService(IAppUserRepository userRepository, IProfileProvider profileProvider){
            _userRepository = userRepository;
            _profileProvider = profileProvider;
        }

        public async Task<AppUser?> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = await _userRepository.GetByUsernameAsync(username);

            // Ensure user exists and stored password is not null/empty
            if (user == null || string.IsNullOrEmpty(user.Password))
                return null;

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            return new AppUser
            {
                Id = user.Id,
                Username = user.Username,
                ProfileImageUrl = user.ProfileImageUrl,
                Password = null // don't return hashed password
            };
        }


        public async Task<AuthDTO> RegisterAsync(string username, string password)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");

            // Check if user already exists
            var existingUser = await _userRepository.GetByUsernameAsync(username.ToLowerInvariant());
            if (existingUser != null)
                return new AuthDTO(null, DUPLICATE_USER);

            // Hash password
            password = BCrypt.Net.BCrypt.HashPassword(password);

            // Create new user
            var newUser = await _userRepository.CreateAppUserAsync(username, password);
            if (newUser == null)
                return new AuthDTO(null, REGISTER_ERROR);

            // Create default profile
            var profile = new ProfileEntity
            {
                Id = newUser.Id,
                Username = newUser.Username,
                FirstName = "",
                LastName = "",
                Bio = "",
                ProfileImageUrl = "",
                SportLevel = new Dictionary<string, string>()
            };

            await _profileProvider.UpsertProfileAsync(profile);

            return new AuthDTO(newUser, REGISTER_SUCCESS);
        }

        public async Task<Dictionary<string, string>> GetProfileImagesForUsernamesAsync(IEnumerable<string> usernames){
            var users = await _userRepository.GetByUsernamesAsync(usernames);

            if (users != null && users.Any()){
                return users.ToDictionary(u => u.Username, u => u.ProfileImageUrl);
            }
            
            return new Dictionary<string, string>();
        }

        public async Task<string?> GetProfileImageForUsernameAsync(string username)
        {
            var users = await _userRepository.GetByUsernamesAsync(new[] { username });

            var user = users
                .FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

            return user?.ProfileImageUrl;
        }

    }
}
