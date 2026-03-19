using TaskManager.Domain.Enums;

namespace TaskManager.Contracts.DTOs;

public sealed record TaskItemDto(
    int Id,
    string Title,
    string? Description,
    TaskItemPriority Priority,
    TaskItemStatus Status,
    DateTimeOffset DueDate,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? CompletedAt,
    int? CategoryId,
    string? CategoryName,
    bool IsOverdue
);
