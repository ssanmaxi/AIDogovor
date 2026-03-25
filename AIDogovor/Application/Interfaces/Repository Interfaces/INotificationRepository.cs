using AIDogovor.Domain.Entities;

namespace AIDogovor.Application.Interfaces.Repository_Interfaces;

public interface INotificationRepository
{
    public Task<List<Notification>> GetByUserIdAsync(Guid id, int page, int limit, bool unreadOnly);
    public Task<int> GetUnreadCountAsync(Guid userId);
    public Task MarkAsReadAsync(Guid id, Guid userId);
    public Task MarkAllAsReadAsync(Guid userId);
    public Task<int> GetCountAsync(Guid userId, bool unreadOnly);
    public Task<Notification> GetByIdAsync(Guid id);
    public Task AddAsync(Notification notification);
    public Task<bool> ShareExpChecker(Guid contractId);
}