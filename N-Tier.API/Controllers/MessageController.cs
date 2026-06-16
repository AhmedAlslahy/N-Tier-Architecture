namespace N_Tier.API.Controllers;

[Route("api/messages")]
[Authorize]
[ApiController]
public class MessageController(IMessageService messageService) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> Create(CreateMessageDto dto, CancellationToken cancellation)
    {
        var result = await messageService.CreateAsync(dto, userId, cancellation);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessageById(int id, CancellationToken cancellation)
    {
        var result = await messageService.GetMessageById(id, userId, cancellation);
        return HandleResult(result);
    }

    [HttpPost("Starred/{id}")]
    public async Task<IActionResult> StarredMessageById(int id, CancellationToken cancellation)
    {
        var result = await messageService.StarredMessageById(id, userId, cancellation);
        return HandleResult(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllByUserId()
    {
        var result = await messageService.GetAllByUserId(userId);
        return HandleResult(result);
    }

    [HttpGet("all-Sender")]
    public async Task<IActionResult> GetAllSenderByUserId(CancellationToken cancellation)
    {
        var result = await messageService.GetAllSenderByUserId(userId, cancellation);
        return HandleResult(result);
    }

    [HttpGet("all-starred")]
    public async Task<IActionResult> GetAllStarredByUserId()
    {
        var result = await messageService.GetAllStarredByUserId(userId);
        return HandleResult(result);
    }

    [HttpGet("all-unread")]
    public async Task<IActionResult> GetAllUnreadByUserId()
    {
        var result = await messageService.GetAllUnreadByUserId(userId);
        return HandleResult(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> UnreadCount(CancellationToken cancellation)
    {
        var result = await messageService.UnreadCountByUserId(userId, cancellation);
        return HandleResult(result);
    }
}