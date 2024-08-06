using Microsoft.AspNetCore.Mvc;
using sportpick_domain;

namespace sportpick_api.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase{

    [HttpGet(Name = "GetEvents")]
    public ICollection<Event> Get(){
        DateTime now = new DateTime();  
        List<Event> events = new List<Event>();
        Event myEvent = new Event("id123", "phx", now);
        events.Add(myEvent);

        return events;
    }

}

