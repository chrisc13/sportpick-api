namespace sportpick_domain
{
    public class DropEvent
    {
        public string? Id { get; set; }    
        public string EventName { get; set; } 
        public string EventDetails { get; set; }   
        public string Sport { get; set; }          
        public string Location { get; set; }           
        public string? LocationDetails { get; set; }   
        public DateTime Start { get; set; }           
        public DateTime? End { get; set; }            
        public int? MaxPlayers { get; set; }       
        public int? CurrentPlayers { get; set; } 
        public List<Attendee>? Attendees { get; set; } 
        public string OrganizerName { get; set; } 
        public string OrganizerId { get; set; }  
        public double? Latitude { get; set; }          
        public double? Longitude { get; set; }         
        
        public Dictionary<string, object>? ExtraFields { get; set; } 
    }
    public class Attendee{
        public string Username {get; set;}
        public string Id {get;set;}
    }
}
