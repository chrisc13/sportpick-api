namespace sportpick_domain
{
    public class DropInThread
    {
        public string? Id { get; set; }   
        public string Title { get; set; } 
        public string Body { get; set; }   
        public string CreatorImageUrl { get; set; }
        public string CreatorName { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment>? Comments { get; set; }   
        public List<Like> Likes { get; set; }       
        
        public Dictionary<string, object>? ExtraFields { get; set; } 
    }

    public class Like{
        public string Username {get; set;}
        public string Id {get;set;}
    }

    public class Comment{
        public string Username {get; set;}
        public string Id {get;set;}
        public string UserImageUrl { get; set; }
    }
}
