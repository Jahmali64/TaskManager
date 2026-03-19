using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities;

public sealed class Category
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Color Color { get; private set; } = Color.Default;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private readonly List<TaskItem> _taskItems = [];
    public IReadOnlyCollection<TaskItem> TaskItems => _taskItems.AsReadOnly();

    private Category() { }

    public static Category Create(string name, Color? color = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Category
        {
            Name = name.Trim(),
            Color = color ?? Color.Default,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Rename(string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        Name = newName.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangeColor(Color newColor)
    {
        ArgumentNullException.ThrowIfNull(newColor);
        Color = newColor;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
