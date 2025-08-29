using System;
using sportpick_domain;

namespace sportpick_dal
{
    public static class AppUserMapper
    {
        public static AppUser ToDomain(AppUserEntity entity)
        {
            if (entity == null) return null;

            return new AppUser
            {
                Id = entity.Id,
                Username = entity.Username,
                Password = entity.Password
            };
        }

        public static AppUserEntity ToEntity(AppUser user)
        {
            if (user == null) return null;

            return new AppUserEntity
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password
            };
        }
    }
}
