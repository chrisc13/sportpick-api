using sportpick_domain;

namespace sportpick_dal;
    public static class DropInThreadMapper
    {
        /// <summary>
        /// Maps a persistence entity (DropInThreadEntity) to the clean domain model (DropInThread)
        /// </summary>
        public static DropInThread ToDomain(DropInThreadEntity entity)
        {
            if (entity == null) return null;

            return new DropInThread
            {
                Id = entity.Id,
                Title = entity.Title,
                Body = entity.Body,
                CreatorImageUrl = entity.CreatorImageUrl,
                CreatorId = entity.CreatorId,
                CreatorName = entity.CreatorName,
                CreatedAt = entity.CreatedAt,
                Comments = entity.Comments,
                Likes = entity.Likes,
                ExtraFields = entity.ExtraFields
            };
        }

        /// <summary>
        /// Maps a domain model (DropInThread) to a persistence entity (DropInThreadEntity)
        /// </summary>
        public static DropInThreadEntity ToEntity(DropInThread domain)
        {
            if (domain == null) return null;

            return new DropInThreadEntity
            {
                Id = domain.Id,  // Mongo _id
                Title = domain.Title,
                Body = domain.Body,
                CreatorImageUrl = domain.CreatorImageUrl,
                CreatorId = domain.CreatorId,
                CreatorName = domain.CreatorName,
                CreatedAt = domain.CreatedAt,
                Comments = domain.Comments,
                Likes = domain.Likes,
                ExtraFields = domain.ExtraFields
            };
        }

        /// <summary>
        /// Maps a list of entities to a list of domain models
        /// </summary>
        public static List<DropInThread> ToDomainList(IEnumerable<DropInThreadEntity> entities)
        {
            if (entities == null) return new List<DropInThread>();
            return entities.Select(ToDomain).ToList();
        }

        /// <summary>
        /// Maps a list of domain models to a list of entities
        /// </summary>
        public static List<DropInThreadEntity> ToEntityList(IEnumerable<DropInThread> domains)
        {
            if (domains == null) return new List<DropInThreadEntity>();
            return domains.Select(ToEntity).ToList();
        }
    }

