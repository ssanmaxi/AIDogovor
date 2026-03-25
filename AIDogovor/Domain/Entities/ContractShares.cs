namespace AIDogovor.Domain.Entities;

public class ContractShares
{
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public string? Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
}