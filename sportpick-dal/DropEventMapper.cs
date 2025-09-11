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
                EventDetails = entity.EventDetails,
                Sport = entity.Sport,
                Location = entity.Location,
                Start = entity.Start,
                End = entity.End,
                MaxPlayers = entity.MaxPlayers,
                CurrentPlayers = entity.CurrentPlayers,
                Attendees = entity.Attendees,
                OrganizerName = entity.OrganizerName,
                OrganizerId = entity.OrganizerId,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                ExtraFields = entity.ExtraFields
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
                EventDetails = domain.EventDetails,
                Sport = domain.Sport,
                Location= domain.Location,
                Start = domain.Start,
                End = domain.End,
                MaxPlayers = domain.MaxPlayers,
                CurrentPlayers = domain.CurrentPlayers,
                Attendees = domain.Attendees,
                OrganizerName = domain.OrganizerName,
                OrganizerId = domain.OrganizerId,
                Latitude = domain.Latitude,
                Longitude = domain.Longitude,
                ExtraFields = domain.ExtraFields
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

