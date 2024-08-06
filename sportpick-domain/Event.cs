namespace sportpick_domain;
public class Event
{
    public string id {get; set;}
    public string location {get; set;}
    public DateTime date {get; set;}

    public Event(string id, string location, DateTime date){
        this.id = id;
        this.location = location;
        this.date = date;
    }

}
