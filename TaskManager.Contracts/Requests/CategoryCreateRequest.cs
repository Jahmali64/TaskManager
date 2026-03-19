using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Contracts.Requests;

public sealed record CategoryCreateRequest(
    [Required]
    [MaxLength(100)]
    string Name,
    [RegularExpression(Color.HexColorPattern,
        ErrorMessage = "Color must be a valid hex code, e.g., #FF5733 or #F53")]
    string? Color = null
);
