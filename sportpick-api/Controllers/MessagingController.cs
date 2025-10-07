using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MessagingController : ControllerBase
{
    private readonly MessagingService _service;

    public MessagingController(MessagingService service)
    {
        _service = service;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatMessage msg)
    {
        var id = await _service.SendMessage(msg.User1, msg.User2, msg.Sender, msg.Message);
        return Ok(new { status = "sent", messageId = id });
    }

    [HttpGet("read")]
    public async Task<IActionResult> ReadMessages([FromQuery] string user1, [FromQuery] string user2, [FromQuery] string lastId = "0-0")
    {
        var messages = await _service.GetMessages(user1, user2, lastId);
        return Ok(messages);
    }
    [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest("Username is required");

            var conversations = await _service.GetUserConversationsAsync(username);
            return Ok(conversations);
        }
}

// DTO
public class ChatMessage
{
    public string User1 { get; set; }
    public string User2 { get; set; }
    public string Sender { get; set; }
    public string Message { get; set; }
}
