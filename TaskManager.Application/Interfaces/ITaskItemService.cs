using TaskManager.Contracts.DTOs;
using TaskManager.Contracts.Requests;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Interfaces;

public interface ITaskItemService
{
    Task<IReadOnlyList<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> CreateAsync(TaskItemCreateRequest request, CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> UpdateAsync(int id, TaskItemUpdateRequest request, CancellationToken cancellationToken = default(CancellationToken));
    Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> CompleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> StartAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> CancelAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> ReopenAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> ChangePriorityAsync(int id, TaskItemPriority priority, CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> AssignCategoryAsync(int id, int categoryId, CancellationToken cancellationToken = default(CancellationToken));
    Task<TaskItemDto> RemoveCategoryAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
}
