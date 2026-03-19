using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Persistence.Configurations;

public sealed class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(taskItem => taskItem.Id);
        builder.Property(taskItem => taskItem.Title).IsRequired().HasMaxLength(200);
        builder.Property(taskItem => taskItem.Description).HasMaxLength(1000);
        builder.Property(taskItem => taskItem.Priority)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                priority => priority.ToString(),
                value => Enum.Parse<TaskItemPriority>(value)
            );
        builder.Property(taskItem => taskItem.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<TaskItemStatus>(value)
            );
        builder.Property(taskItem => taskItem.DueDate).IsRequired();
        builder.Property(taskItem => taskItem.CreatedAt).IsRequired();
        builder.Property(taskItem => taskItem.UpdatedAt).IsRequired();
        builder.Property(taskItem => taskItem.CompletedAt);
        
        builder.Ignore(taskItem => taskItem.IsOverdue);
        builder.Ignore(taskItem => taskItem.IsCompleted);
        builder.Ignore(taskItem => taskItem.IsCancelled);
        
        builder.HasOne(taskItem => taskItem.Category)
            .WithMany(category => category.TaskItems)
            .HasForeignKey(taskItem => taskItem.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
