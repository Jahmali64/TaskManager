using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);
        builder.HasIndex(category => category.Name).IsUnique();
        builder.Property(category => category.Name).IsRequired().HasMaxLength(100);
        builder.Property(category => category.Color)
            .IsRequired()
            .HasMaxLength(7)
            .HasConversion(
                color => color.Value,
                value => Color.From(value)
            );
        builder.Property(category => category.CreatedAt).IsRequired();
        builder.Property(category => category.UpdatedAt).IsRequired();
    }
}
