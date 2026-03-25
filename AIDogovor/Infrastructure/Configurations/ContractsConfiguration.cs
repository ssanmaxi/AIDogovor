using AIDogovor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIDogovor.Infrastructure.Configurations;

public class ContractsConfiguration : IEntityTypeConfiguration<Contracts>
{
    public void Configure(EntityTypeBuilder<Contracts> builder)
    {
        builder.ToTable("contracts");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.UserId);
        builder.Property(b => b.Title);
        builder.Property(b => b.Status);
        builder.Property(b => b.PdfPath);
        builder.Property(b => b.CreatedAt);
        builder.Property(b => b.UpdatedAt);
    }
}