namespace TaskManager.Contracts.DTOs;

public sealed record CategoryDto(
    int Id,
    string Name,
    string Color,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
