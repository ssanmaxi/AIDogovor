namespace AIDogovor.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string? Type { get; private set; }
    public string? Title { get; private set; }
    public string? Body { get; private set; }
    public Guid? ContractId { get; private set; }
    public bool IsRead { get; private set; } = false;
    public DateTime CreatedAt { get; private set; }

    public Notification(Guid id, Guid userId, string type,
        string title, string body, Guid? contractId)
    {
        Id = id;
        UserId = userId;
        Type = type;
        Title = title;
        Body = body;
        ContractId = contractId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Read()
    {
        IsRead = true;
    }
}