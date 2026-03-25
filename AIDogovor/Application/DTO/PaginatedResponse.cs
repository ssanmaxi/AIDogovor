namespace AIDogovor.Application.DTO;

public class PaginatedResponse
{
    public List<NotificationResponse>? Items { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
}