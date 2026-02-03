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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetThread(string id)
    {
        var thread = await _dropInThreadService.GetDropInThreadByIdAsync(id);

        if (thread == null)
             return BadRequest();

        return Ok(thread);
    }

    [Authorize]
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(string id, [FromBody] Comment comment)
    {
        comment.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        comment.Username = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        comment.CreatedAt = DateTime.UtcNow;

        var result = await _dropInThreadService.AddThreadCommentAsync(comment, id);
        if (!result) return BadRequest();

        return Ok(true);
    }

    // POST: api/DropInThread/{id}/likes
    [Authorize]
    [HttpPost("{id}/likes")]
    public async Task<IActionResult> AddLike(string id)
    {
        var like = new Like
        {
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
            Username = User.FindFirst(ClaimTypes.Name)?.Value ?? "",
            CreatedAt = DateTime.UtcNow
        };

        var result = await _dropInThreadService.AddThreadLikeAsync(like, id);
        if (!result) return BadRequest();

        return Ok(true);
    }

    // DELETE: api/DropInThread/likes/{likeId}
    [Authorize]
    [HttpDelete("likes/{likeId}")]
    public async Task<IActionResult> RemoveLike(string likeId)
    {
        var result = await _dropInThreadService.RemoveThreadLikeAsync(likeId);
        if (!result) return NotFound();

        return Ok(true);
    }
//revert
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

