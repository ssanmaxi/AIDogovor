using AIDogovor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIDogovor.Infrastructure.Configurations;

public class ContractSharesConfigurations : IEntityTypeConfiguration<ContractShares>
{
    public void Configure(EntityTypeBuilder<ContractShares> builder)
    {
        builder.ToTable("contract_shares");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.ContractId);
        builder.Property(b => b.Token);
        builder.Property(b => b.ExpiresAt);
        builder.Property(b => b.CreatedAt);
    }
}