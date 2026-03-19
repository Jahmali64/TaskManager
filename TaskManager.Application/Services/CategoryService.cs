using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Contracts.DTOs;
using TaskManager.Contracts.Requests;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.ValueObjects;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly AppDbContext _appDbContext;

    public CategoryService(AppDbContext appDbContext) => _appDbContext = appDbContext;

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        List<Category> categories = await _appDbContext.Categories.AsNoTracking().ToListAsync(cancellationToken);

        return categories.ConvertAll(MapToDto);
    }

    public async Task<CategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        Category category = await _appDbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new NotFoundException($"Category with ID {id} not found.");

        return MapToDto(category);
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            Color? color = request.Color is not null ? Color.From(request.Color) : null;
            Category category = Category.Create(request.Name, color);
            _appDbContext.Categories.Add(category);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return MapToDto(category);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while creating the category. Please ensure the data is valid and try again.", ex);
        }
    }

    public async Task<CategoryDto> UpdateAsync(int id, CategoryUpdateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            Category category = await _appDbContext.Categories.FindAsync([id], cancellationToken) ?? throw new NotFoundException($"Category with ID {id} not found.");

            category.Rename(request.Name);
            if (request.Color is not null)
            {
                category.ChangeColor(Color.From(request.Color));
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return MapToDto(category);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while updating the category. Please ensure the data is valid and try again.", ex);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            Category category = await _appDbContext.Categories.FindAsync([id], cancellationToken) ?? throw new NotFoundException($"Category with ID {id} not found.");

            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while deleting the category. Please ensure the category is not associated with any tasks and try again.", ex);
        }
    }

    private static CategoryDto MapToDto(Category c) => new(
        c.Id,
        c.Name,
        c.Color.Value,
        c.CreatedAt,
        c.UpdatedAt
    );
}