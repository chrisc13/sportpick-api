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
    public async Task<IActionResult> Get(){
        List<DropEvent> events = new List<DropEvent>();
        
        events = await _dropEventService.GetAllDropEventsAsync();
        if (events == null || events.Count == 0){
             return BadRequest();
        }

        return Ok(events);
    }

    [HttpGet("GetTopThreePopularEvents")]
    public async Task<IActionResult> GetTopThreePopularEvents(){
        List<DropEvent> events = new List<DropEvent>();
        
        events = await _dropEventService.GetTopThreePopularAsync();
        if (events == null || events.Count == 0){
             return BadRequest();
        }

        return Ok(events);
    }

    [Authorize]
    [HttpPost("CreateEvent")]
    public async Task<IActionResult> CreateEvent([FromBody] DropEvent dropEvent)
    {   
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        dropEvent.OrganizerId = userId;

         var creatorName = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        dropEvent.OrganizerName = creatorName;

        var result = await _dropEventService.CreateEventAsync(dropEvent); // use dropEvent, not 'event'
        if (!result)
        {
            return BadRequest();
        }

        return Ok(true);
    }

    [Authorize]
    [HttpPost("{eventid}/Attendees")]
    public async Task<IActionResult> AttendEvent(string eventid)
    {   
        var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var attendee = new Attendee(){Username = userName, Id = userId};

        var result = await _dropEventService.AttendEventAsync(attendee, eventid);
        if (!result)
        {
            return BadRequest();
        }

        return Ok(true);
    }    

    [HttpGet("nearby", Name = "GetNearbyEvents")]
    public async Task<IActionResult> GetNearbyEvents(double maxDistanceMiles, double latitude, double longitude)
    {
        if (maxDistanceMiles <= 0)
            return BadRequest("Max distance must be greater than 0.");

        var events = await _dropEventService.GetNearbyEventsAsync(maxDistanceMiles, (latitude, longitude));

        if (events == null || events.Count == 0)
            return Ok(new List<DropEvent>()); 

        return Ok(events);
    }

    
}

