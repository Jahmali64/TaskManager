using TaskManager.Contracts.DTOs;
using TaskManager.Contracts.Requests;

namespace TaskManager.Application.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<CategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    Task<CategoryDto> CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken = default(CancellationToken));
    Task<CategoryDto> UpdateAsync(int id, CategoryUpdateRequest request, CancellationToken cancellationToken = default(CancellationToken));
    Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
}
