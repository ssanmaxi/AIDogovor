using AIDogovor.Application.Interfaces.Repository_Interfaces;
using AIDogovor.Domain.Entities;
using AIDogovor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace AIDogovor.Infrastructure.Repostitory;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _db;

    public NotificationRepository(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<List<Notification>> GetByUserIdAsync(Guid id, int page, int limit, bool unreadOnly)
    {
        var notif = _db.Notifications.Where(i => i.UserId == id);
        if (unreadOnly) 
            notif = notif.Where(n => !n.IsRead);

        return await notif
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        var query = _db.Notifications.Where(q => q.UserId == userId);
        var ans = await query.CountAsync(q => !q.IsRead);

        return ans;
    }

    public async Task MarkAsReadAsync(Guid id, Guid userId)
    {
        var notification = await _db.Notifications.FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

        if (notification == null) throw new Exception("ID or User ID not found!");

        notification.Read();

        await _db.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        var notification = await _db.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));
    }

    public async Task<int> GetCountAsync(Guid userId, bool unreadOnly)
    {
        var count = await _db.Notifications
            .CountAsync(n => n.UserId == userId && (!unreadOnly || !n.IsRead));

        return count;
    }

    public async Task<Notification> GetByIdAsync(Guid id)
    {
        var notif = await _db.Notifications
            .FirstOrDefaultAsync(n => n.Id == id);

        return notif;
    }

    public async Task AddAsync(Notification notif)
    {
        _db.Notifications.Add(notif);

        await _db.SaveChangesAsync();
    }

    public async Task<bool> ShareExpChecker(Guid contractId)
    {
        var res = await _db.Notifications.AnyAsync(r =>
            r.ContractId == contractId && r.Type == "share_expiring" && r.CreatedAt >= DateTime.UtcNow.AddDays(-7));
        return res;
    }
}