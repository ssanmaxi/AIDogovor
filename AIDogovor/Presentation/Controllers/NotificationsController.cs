using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AIDogovor.Application.DTO;
using AIDogovor.Application.Interfaces.Repository_Interfaces;
using AIDogovor.Domain.Entities;

namespace AIDogovor.Presentation.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationRepository _nr;

    public NotificationsController(INotificationRepository nr)
    {
        _nr = nr;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<PaginatedResponse> GetNotificationsByUserId(int page, int limit, bool unreadOnly)
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);

        var notif = await _nr.GetByUserIdAsync(userId, page,limit, unreadOnly);

        var total = await _nr.GetCountAsync(userId, unreadOnly);

        return new PaginatedResponse
        {
            Items = notif.Select(NotificationResponse.FromDomain).ToList(),
            Page = page,
            Limit = limit,
            Total = total
        };
    }

    [Authorize]
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);

        var count = await _nr.GetUnreadCountAsync(userId);
        
        return Ok(new {count = count});
    }

    [Authorize]
    [HttpPatch("{id}/read")]
    public async Task<IActionResult> Read(Guid id)
    {
        var notif = await _nr.GetByIdAsync(id);
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);
        
        if (notif == null || notif.UserId != userId) return NotFound();

        await _nr.MarkAsReadAsync(notif.Id, notif.UserId);
        return Ok();
    }

    [Authorize]
    [HttpPatch("read-all")]
    public async Task<IActionResult> ReadAll()
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);

        await _nr.MarkAllAsReadAsync(userId);
        return Ok();
    }
}