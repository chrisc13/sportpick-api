using System;
using sportpick_domain;

namespace sportpick_dal
{
    public static class UserMapper
    {
        public static User ToDomain(UserEntity entity)
        {
            if (entity == null) return null;

            return new User
            {
                Id = entity.Id,
                Username = entity.Username,
                Password = entity.Password
            };
        }

        public static UserEntity ToEntity(User user)
        {
            if (user == null) return null;

            return new UserEntity
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password
            };
        }
    }
}
