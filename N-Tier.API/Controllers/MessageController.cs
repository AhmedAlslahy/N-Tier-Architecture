using static N_Tier.Application.Features.Message.CreateMessage;
using static N_Tier.Application.Features.Message.GetAllMessageByUserId;
using static N_Tier.Application.Features.Message.GetAllMessageStarredByUserId;
using static N_Tier.Application.Features.Message.GetAllSenderMessageByUserId;
using static N_Tier.Application.Features.Message.GetAllUnreadMessageByUserId;
using static N_Tier.Application.Features.Message.GetMessageById;
using static N_Tier.Application.Features.Message.StarredMessageById;
using static N_Tier.Application.Features.Message.UnreadCountMessageByUserId;

namespace N_Tier.API.Controllers;

[Route("api/messages")]
[Authorize]
[ApiController]
public class MessageController(CreateMessageHandler createMessage, GetMessageByIdHandler getMessageById
    , StarredMessageByIdHandler starredMessageById, GetAllSenderMessageByUserIdHandler getAllSenderMessageByUserId
    , GetAllMessageStarredByUserIdHandler getAllMessageStarredByUserId, GetAllUnreadMessageByUserIdHandler getAllUnreadMessageByUserId
    , UnreadCountMessageByUserIdHandler unreadCountByUserId, GetAllMessageByUserIdHandler getAllMessageByUserId) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> Create(CreateMessageReq req)
    {
        var result = await createMessage.Handle(req, userId);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessageById(int id)
    {
        var result = await getMessageById.Handle(id, userId);
        return HandleResult(result);
    }

    [HttpPost("Starred/{id}")]
    public async Task<IActionResult> StarredMessageById(int id)
    {
        var result = await starredMessageById.Handle(id, userId);
        return HandleResult(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllMessagesByUserId()
    {
        var result = await getAllMessageByUserId.Handle(userId);
        return HandleResult(result);
    }

    [HttpGet("all-Sender")]
    public async Task<IActionResult> GetAllSenderMessagesByUserId()
    {
        var result = await getAllSenderMessageByUserId.Handle(userId);
        return HandleResult(result);
    }

    [HttpGet("all-starred")]
    public async Task<IActionResult> GetAllStarredMessagesByUserId()
    {
        var result = await getAllMessageStarredByUserId.Handle(userId);
        return HandleResult(result);
    }

    [HttpGet("all-unread")]
    public async Task<IActionResult> GetAllUnreadMessagesByUserId()
    {
        var result = await getAllUnreadMessageByUserId.Handle(userId);
        return HandleResult(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> UnreadCountByUserId()
    {
        var result = await unreadCountByUserId.Handle(userId);
        return HandleResult(result);
    }
}