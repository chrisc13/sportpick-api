using Microsoft.AspNetCore.Mvc;
using sportpick_domain;
using sportpick_bll;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace sportpick_api.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase{

    IDropEventService _dropEventService;

    public EventController(IDropEventService dropEventService){
        _dropEventService = dropEventService;
    }

    [HttpGet(Name = "GetEvents")]
    public IActionResult Get(){
        List<DropEvent> events = new List<DropEvent>();
        
        events = _dropEventService.GetAllDropEvents();
        if (events == null || events.Count == 0){
             return BadRequest();
        }

        return Ok(events);
    }

    [HttpGet("GetTopThreePopularEvents")]
    public IActionResult GetTopThreePopularEvents(){
        List<DropEvent> events = new List<DropEvent>();
        
        events = _dropEventService.GetTopThreePopular();
        if (events == null || events.Count == 0){
             return BadRequest();
        }

        return Ok(events);
    }

    [Authorize]
    [HttpPost("CreateEvent")]
    public IActionResult CreateEvent([FromBody] DropEvent dropEvent)
    {   
        Console.WriteLine(JsonSerializer.Serialize(dropEvent, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        dropEvent.OrganizerId = userId;

         var creatorName = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        dropEvent.OrganizerName = creatorName;

        var result = _dropEventService.CreateEvent(dropEvent); // use dropEvent, not 'event'
        if (!result)
        {
            return BadRequest();
        }

        return Ok(true);
    }

    [Authorize]
    [HttpPost("{eventid}/Attendees")]
    public IActionResult AttendEvent(string eventid)
    {   
        var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var attendee = new Attendee(){Username = userName, Id = userId};

        var result = _dropEventService.AttendEvent(attendee, eventid);
        if (!result)
        {
            return BadRequest();
        }

        return Ok(true);
    }

    
}

