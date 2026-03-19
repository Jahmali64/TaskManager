using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public sealed class TaskItem
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskItemPriority Priority { get; private set; }
    public TaskItemStatus Status { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    public int? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    public bool IsCompleted => Status == TaskItemStatus.Completed;
    public bool IsCancelled => Status == TaskItemStatus.Cancelled;
    public bool IsOverdue => !IsCompleted && !IsCancelled && DueDate < DateTimeOffset.UtcNow;

    private TaskItem() { }

    public static TaskItem Create(string title, DateTimeOffset dueDate, string? description = null, TaskItemPriority priority = TaskItemPriority.Medium, Category? category = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ValidateDueDate(dueDate);

        return new TaskItem
        {
            Title = title.Trim(),
            Description = description?.Trim(),
            Priority = priority,
            Status = TaskItemStatus.NotStarted,
            DueDate = dueDate,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            CategoryId = category?.Id,
            Category = category
        };
    }

    public void Update(string title, string? description, DateTimeOffset dueDate, TaskItemPriority priority)
    {
        EnsureActive();
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ValidateDueDate(dueDate);

        Title = title.Trim();
        Description = description?.Trim();
        DueDate = dueDate;
        Priority = priority;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Start()
    {
        EnsureActive();
        if (Status == TaskItemStatus.InProgress)
        {
            throw new InvalidOperationException("Task is already in progress.");
        }

        SetStatus(TaskItemStatus.InProgress);
    }

    public void Cancel()
    {
        EnsureActive();
        SetStatus(TaskItemStatus.Cancelled);
    }

    public void Complete()
    {
        EnsureActive();
        SetStatus(TaskItemStatus.Completed);
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void Reopen()
    {
        if (!IsCompleted && !IsCancelled)
        {
            throw new InvalidOperationException("Task is not completed or cancelled.");
        }

        SetStatus(TaskItemStatus.NotStarted);
        CompletedAt = null;
    }

    public void AssignCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);
        CategoryId = category.Id;
        Category = category;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RemoveCategory()
    {
        CategoryId = null;
        Category = null;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangePriority(TaskItemPriority priority)
    {
        EnsureActive();
        Priority = priority;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    private void SetStatus(TaskItemStatus status)
    {
        Status = status;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static void ValidateDueDate(DateTimeOffset dueDate)
    {
        if (dueDate < DateTimeOffset.UtcNow) throw new ArgumentException("Due date cannot be in the past.");
    }

    private void EnsureActive()
    {
        if (IsCompleted) throw new InvalidOperationException("Cannot modify a completed task.");
        if (IsCancelled) throw new InvalidOperationException("Cannot modify a cancelled task.");
    }
}
