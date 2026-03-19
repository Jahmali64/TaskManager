using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.Contracts.Requests;

public sealed record TaskItemCreateRequest(
    [Required]
    [MaxLength(200)]
    string Title,
    DateTimeOffset DueDate,
    [MaxLength(1000)]
    string? Description = null,
    TaskItemPriority Priority = TaskItemPriority.Medium,
    int? CategoryId = null
);
