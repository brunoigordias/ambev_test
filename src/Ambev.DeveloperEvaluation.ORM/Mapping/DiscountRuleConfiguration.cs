using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework configuration for DiscountRule entity
/// </summary>
public class DiscountRuleConfiguration : IEntityTypeConfiguration<DiscountRule>
{
    public void Configure(EntityTypeBuilder<DiscountRule> builder)
    {
        builder.ToTable("DiscountRules");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(r => r.MinQuantity)
            .IsRequired();

        builder.Property(r => r.MaxQuantity)
            .IsRequired();

        builder.Property(r => r.DiscountPercentage)
            .IsRequired()
            .HasColumnType("decimal(5,2)");

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired(false);

        // Create index on IsActive for filtering active rules
        builder.HasIndex(r => r.IsActive);

        // Create index on MinQuantity and MaxQuantity for faster lookups
        builder.HasIndex(r => new { r.MinQuantity, r.MaxQuantity });

        // Ensure no overlapping ranges for active rules (optional constraint)
        // This would require a database function or trigger
    }
}

