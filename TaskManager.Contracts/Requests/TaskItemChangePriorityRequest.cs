using TaskManager.Domain.Enums;

namespace TaskManager.Contracts.Requests;

public sealed record TaskItemChangePriorityRequest(TaskItemPriority Priority);
