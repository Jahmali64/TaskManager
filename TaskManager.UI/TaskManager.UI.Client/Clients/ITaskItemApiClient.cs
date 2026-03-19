using Refit;
using TaskManager.Contracts.DTOs;
using TaskManager.Contracts.Requests;

namespace TaskManager.UI.Client.Clients;

public interface ITaskItemApiClient
{
    [Get("/api/v1.0/task-items")]
    Task<IReadOnlyList<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

    [Get("/api/v1.0/task-items/{id}")]
    Task<TaskItemDto> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

    [Post("/api/v1.0/task-items")]
    Task<TaskItemDto> CreateAsync([Body] TaskItemCreateRequest request, CancellationToken cancellationToken = default(CancellationToken));

    [Put("/api/v1.0/task-items/{id}")]
    Task<TaskItemDto> UpdateAsync(int id, [Body] TaskItemUpdateRequest request, CancellationToken cancellationToken = default(CancellationToken));

    [Delete("/api/v1.0/task-items/{id}")]
    Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

    [Post("/api/v1.0/task-items/{id}/start")]
    Task<TaskItemDto> StartAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

    [Post("/api/v1.0/task-items/{id}/complete")]
    Task<TaskItemDto> CompleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

    [Post("/api/v1.0/task-items/{id}/cancel")]
    Task<TaskItemDto> CancelAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

    [Post("/api/v1.0/task-items/{id}/reopen")]
    Task<TaskItemDto> ReopenAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

    [Patch("/api/v1.0/task-items/{id}/priority")]
    Task<TaskItemDto> ChangePriorityAsync(int id, [Body] TaskItemChangePriorityRequest request, CancellationToken cancellationToken = default(CancellationToken));

    [Put("/api/v1.0/task-items/{id}/category/{categoryId}")]
    Task<TaskItemDto> AssignCategoryAsync(int id, int categoryId, CancellationToken cancellationToken = default(CancellationToken));

    [Delete("/api/v1.0/task-items/{id}/category")]
    Task<TaskItemDto> RemoveCategoryAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
}