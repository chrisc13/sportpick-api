using sportpick_domain;

namespace sportpick_dal;
    public static class DropEventMapper
    {
        /// <summary>
        /// Maps a persistence entity (DropEventEntity) to the clean domain model (DropEvent)
        /// </summary>
        public static DropEvent ToDomain(DropEventEntity entity)
        {
            if (entity == null) return null;

            return new DropEvent
            {
                Id = entity.Id,
                EventName = entity.EventName,
                SportType = entity.SportType,
                LocationName = entity.LocationName,
                City = entity.City,
                Date = entity.Date,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                MaxPlayers = entity.MaxPlayers,
                CurrentPlayers = entity.CurrentPlayers,
                OrganizerId = entity.OrganizerId,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude
            };
        }

        /// <summary>
        /// Maps a domain model (DropEvent) to a persistence entity (DropEventEntity)
        /// </summary>
        public static DropEventEntity ToEntity(DropEvent domain)
        {
            if (domain == null) return null;

            return new DropEventEntity
            {
                Id = domain.Id,  // Mongo _id
                EventName = domain.EventName,
                SportType = domain.SportType,
                LocationName = domain.LocationName,
                City = domain.City,
                Date = domain.Date,
                StartTime = domain.StartTime,
                EndTime = domain.EndTime,
                MaxPlayers = domain.MaxPlayers,
                CurrentPlayers = domain.CurrentPlayers,
                OrganizerId = domain.OrganizerId,
                Latitude = domain.Latitude,
                Longitude = domain.Longitude
            };
        }

        /// <summary>
        /// Maps a list of entities to a list of domain models
        /// </summary>
        public static List<DropEvent> ToDomainList(IEnumerable<DropEventEntity> entities)
        {
            if (entities == null) return new List<DropEvent>();
            return entities.Select(ToDomain).ToList();
        }

        /// <summary>
        /// Maps a list of domain models to a list of entities
        /// </summary>
        public static List<DropEventEntity> ToEntityList(IEnumerable<DropEvent> domains)
        {
            if (domains == null) return new List<DropEventEntity>();
            return domains.Select(ToEntity).ToList();
        }
    }
