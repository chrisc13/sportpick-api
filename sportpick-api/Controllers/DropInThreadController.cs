using Microsoft.AspNetCore.Mvc;
using sportpick_domain;
using sportpick_bll;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace sportpick_api.Controllers;

[ApiController]
[Route("[controller]")]
public class DropInThreadController : ControllerBase{

    IDropInThreadService _dropInThreadService;

    public DropInThreadController(IDropInThreadService dropInThreadService){
        _dropInThreadService = dropInThreadService;
    }

    [HttpGet(Name = "GetDropInThreads")]
    public async Task<IActionResult> Get(){
        List<DropInThread> dropInThreads = new List<DropInThread>();
        
        dropInThreads = await _dropInThreadService.GetFifteenDropInThreadAsync();
        if (dropInThreads == null || dropInThreads.Count == 0){
             return BadRequest();
        }

        return Ok(dropInThreads);
    }

    // [HttpGet("GetTopThreePopularEvents")]
    // public async Task<IActionResult> GetTopThreePopularEvents(){
    //     List<DropEvent> events = new List<DropEvent>();
        
    //     events = await _dropEventService.GetTopThreePopularAsync();
    //     if (events == null || events.Count == 0){
    //          return BadRequest();
    //     }

    //     return Ok(events);
    // }

    // [HttpGet("GetTopThreeUpcomingEvents")]
    // public async Task<IActionResult> GetTopThreeUpcomingEvents(){
    //     List<DropEvent> events = new List<DropEvent>();
        
    //     events = await _dropEventService.GetTopThreeUpcomingAsync();
    //     if (events == null || events.Count == 0){
    //          return BadRequest();
    //     }

    //     return Ok(events);
    // }

    [Authorize]
    [HttpPost("CreateDropInThread")]
    public async Task<IActionResult> CreateEvent([FromBody] DropInThread dropInThread)
    {   
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        dropInThread.CreatorId = userId;

         var creatorName = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        dropInThread.CreatorName = creatorName;

        var result = await _dropInThreadService.CreateDropInThreadAsync(dropInThread);
        if (!result)
        {
            return BadRequest();
        }

        return Ok(true);
    }
    
}

