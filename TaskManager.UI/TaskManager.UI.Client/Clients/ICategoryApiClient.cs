using Refit;
using TaskManager.Contracts.DTOs;
using TaskManager.Contracts.Requests;

namespace TaskManager.UI.Client.Clients;

public interface ICategoryApiClient
{
    [Get("/api/v1.0/categories")]
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

    [Get("/api/v1.0/categories/{id}")]
    Task<CategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

    [Post("/api/v1.0/categories")]
    Task<CategoryDto> CreateAsync([Body] CategoryCreateRequest request, CancellationToken cancellationToken = default(CancellationToken));

    [Put("/api/v1.0/categories/{id}")]
    Task<CategoryDto> UpdateAsync(int id, [Body] CategoryUpdateRequest request, CancellationToken cancellationToken = default(CancellationToken));

    [Delete("/api/v1.0/categories/{id}")]
    Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
}
