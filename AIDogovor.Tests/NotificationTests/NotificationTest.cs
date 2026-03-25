using System;
using AIDogovor.Domain.Entities;
using Xunit;
namespace AIDogovor.Tests.NotificationTests;

public class NotificationTest
{
    [Fact]
    public void NotificationConstructor_Correct()
    {
        Guid id = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        string type = "Push";
        string title = "Messenger";
        string body = "Message from a friend";
        Guid contractId = Guid.NewGuid();
        
        
        Notification notif = new Notification(id, userId, type, title, body, contractId);

        Assert.Equal(id, notif.Id);
        Assert.Equal(userId, notif.UserId);
        Assert.Equal(type, notif.Type);
        Assert.Equal(title, notif.Title);
        Assert.Equal(body, notif.Body);
        Assert.Equal(contractId, notif.ContractId);
    }

    [Fact]
    public void MarkAsRead_ChangeIsReadToTrue()
    {
        Guid id = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        string type = "Push";
        string title = "Messenger";
        string body = "Message from a friend";
        Guid contractId = Guid.NewGuid();
        
        
        Notification notif = new Notification(id, userId, type, title, body, contractId);

        notif.Read();
        Assert.True(notif.IsRead);
    }
}