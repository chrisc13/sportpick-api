using System;
using sportpick_domain;

namespace sportpick_dal
{
    public static class ProfileMapper
    {
        public static Profile ToDomain(ProfileEntity entity)
        {
            if (entity == null) return null;

            return new Profile
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Username = entity.Username,
                Bio = entity.Bio,
                Location = entity.Location,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                ProfileImageUrl = entity.ProfileImageUrl,
                SportLevel = entity.SportLevel
            };
        }

        public static ProfileEntity ToEntity(Profile profile)
        {
            if (profile == null) return null;

            return new ProfileEntity
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Username = profile.Username,
                Bio = profile.Bio,
                Location = profile.Location,
                Latitude = profile.Latitude,
                Longitude = profile.Longitude,
                ProfileImageUrl = profile.ProfileImageUrl,
                SportLevel = profile.SportLevel
            };
        }
    }
}
