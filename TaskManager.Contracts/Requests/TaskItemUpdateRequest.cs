using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.Contracts.Requests;

public sealed record TaskItemUpdateRequest(
    [Required]
    [MaxLength(200)]
    string Title,
    DateTimeOffset DueDate,
    [MaxLength(1000)]
    string? Description,
    TaskItemPriority Priority,
    int? CategoryId = null
);
