using Microsoft.EntityFrameworkCore;
using AIDogovor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIDogovor.Infrastructure.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.UserId);
        builder.Property(b => b.Type).HasMaxLength(64);
        builder.Property(b => b.Title).HasMaxLength(256);
        builder.Property(b => b.Body);
        builder.Property(b => b.ContractId);
        builder.Property(b => b.IsRead);
        builder.HasIndex(b => b.CreatedAt).IsDescending();
    }
}