using sportpick_domain;

namespace sportpick_dal;
    public static class DropInThreadMapper
    {
        /// <summary>
        /// Maps a persistence entity (DropInThreadEntity) to the clean domain model (DropInThread)
        /// </summary>
        public static DropInThread ToDomain(DropInThreadEntity entity, List<Comment>? comments = null, List<Like>? likes = null)
        {
            return new DropInThread
            {
                Id = entity.Id,
                Title = entity.Title,
                Body = entity.Body,
                CreatorId = entity.CreatorId,
                CreatorName = entity.CreatorName,
                CreatorImageUrl = entity.CreatorImageUrl,
                CreatedAt = entity.CreatedAt,
                Comments = comments ?? new List<Comment>(),
                Likes = likes ?? new List<Like>(),
                ExtraFields = entity.ExtraFields
            };
        }


        /// <summary>
        /// Maps a domain model (DropInThread) to a persistence entity (DropInThreadEntity)
        /// </summary>
            public static DropInThreadEntity ToEntity(DropInThread domain)
            {
                if (domain == null) throw new ArgumentNullException(nameof(domain));

                return new DropInThreadEntity
                {
                    Id = domain.Id,  // Mongo _id
                    Title = domain.Title,
                    Body = domain.Body,
                    CreatorImageUrl = domain.CreatorImageUrl,
                    CreatorId = domain.CreatorId,
                    CreatorName = domain.CreatorName,
                    CreatedAt = domain.CreatedAt,
                    ExtraFields = domain.ExtraFields
                };
            }


    }

