using AIDogovor.Application.Interfaces.Repository_Interfaces;
using AIDogovor.Presentation.Controllers;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AIDogovor.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AIDogovor.Tests.NotificationTests;

public class NotificationControllerTests
{
    [Fact]
    public async Task NotificationControllerGetUnreadCountAsync_Num()
    {
        var moq = new Mock<INotificationRepository>();
        moq.Setup(repo => repo.GetUnreadCountAsync(It.IsAny<Guid>()))
            .ReturnsAsync(5);
        var controller = new NotificationsController(moq.Object);
        
        var userId = Guid.NewGuid();
        var claims = new[] { new Claim("sub", userId.ToString()) };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var res = await controller.GetUnreadCount();
        var okRes = Assert.IsType<OkObjectResult>(res);
        
        var json = System.Text.Json.JsonSerializer.Serialize(okRes.Value);
        Assert.Contains("5", json);
    }

    [Fact]
    public async Task ReadById_Ok()
    {
        var userId = Guid.NewGuid();
        Notification notif = new Notification(Guid.NewGuid(), userId,null, null, null, null);
        var moq1 = new Mock<INotificationRepository>();
        moq1.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(notif);
        
        var controller = new NotificationsController(moq1.Object);
        
        var claims = new[] { new Claim("sub", userId.ToString()) };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var res = await controller.Read(notif.Id);
        Assert.IsType<OkResult>(res);
    }

    [Fact]
    public async Task ReadById_NotFound()
    {
        var userId = Guid.NewGuid();
        Notification notif = new Notification(Guid.NewGuid(), userId,null, null, null, null);
        var moq = new Mock<INotificationRepository>();
        moq.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Notification)null);
        
        var controller = new NotificationsController(moq.Object);
        
        var claims = new[] { new Claim("sub", userId.ToString()) };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        
        var res = await controller.Read(notif.Id);
        Assert.IsType<NotFoundResult>(res);
    }
}