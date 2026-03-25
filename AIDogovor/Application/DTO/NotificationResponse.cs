using AIDogovor.Domain.Entities;

namespace AIDogovor.Application.DTO;

public class NotificationResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Type { get; set; }
    public string? Body { get; set; }
    public string? Title { get; set; }
    public Guid? ContractId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public static NotificationResponse FromDomain(Notification n) => new NotificationResponse
    {
        Id = n.Id,
        UserId = n.UserId,
        Type = n.Type,
        Body = n.Body,
        Title = n.Title, 
        ContractId = n.ContractId,
        IsRead = n.IsRead,
        CreatedAt = n.CreatedAt
    };
}