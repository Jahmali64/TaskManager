using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Contracts.DTOs;
using TaskManager.Contracts.Requests;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Services;

public sealed class TaskItemService : ITaskItemService
{
    private readonly AppDbContext _appDbContext;

    public TaskItemService(AppDbContext appDbContext) => _appDbContext = appDbContext;

    public async Task<IReadOnlyList<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        List<TaskItem> items = await _appDbContext.TaskItems
            .Include(navigationPropertyPath: item => item.Category)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return items.ConvertAll(MapToDto);
    }

    public async Task<TaskItemDto> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        TaskItem taskItem = await _appDbContext.TaskItems
            .Include(navigationPropertyPath: item => item.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new NotFoundException($"Task with ID {id} not found.");

        return MapToDto(taskItem);
    }

    public async Task<TaskItemDto> CreateAsync(TaskItemCreateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            Category? category = null;
            if (request.CategoryId.HasValue)
            {
                category = await _appDbContext.Categories.FindAsync([request.CategoryId.Value], cancellationToken) ?? throw new NotFoundException($"Category with ID {request.CategoryId.Value} not found.");
            }

            TaskItem taskItem = TaskItem.Create(request.Title, request.DueDate, request.Description, request.Priority, category);
            _appDbContext.TaskItems.Add(taskItem);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return MapToDto(taskItem);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while creating the task. Please ensure the data is valid and try again.", ex);
        }
    }

    public async Task<TaskItemDto> UpdateAsync(int id, TaskItemUpdateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            TaskItem taskItem = await FindOrThrowAsync(id, cancellationToken);
            taskItem.Update(request.Title, request.Description, request.DueDate, request.Priority);

            if (request.CategoryId.HasValue && request.CategoryId.Value != taskItem.CategoryId)
            {
                Category category = await _appDbContext.Categories.FindAsync([request.CategoryId.Value], cancellationToken)
                    ?? throw new NotFoundException($"Category with ID {request.CategoryId.Value} not found.");
                taskItem.AssignCategory(category);
            }
            else if (!request.CategoryId.HasValue && taskItem.CategoryId.HasValue)
            {
                taskItem.RemoveCategory();
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return MapToDto(taskItem);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while updating the task. Please ensure the data is valid and try again.", ex);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        TaskItem taskItem = await FindOrThrowAsync(id, cancellationToken);
        _appDbContext.TaskItems.Remove(taskItem);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TaskItemDto> CompleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        TaskItem taskItem = await FindOrThrowAsync(id, cancellationToken);
        taskItem.Complete();
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return MapToDto(taskItem);
    }

    public async Task<TaskItemDto> StartAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        TaskItem taskItem = await FindOrThrowAsync(id, cancellationToken);
        taskItem.Start();
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return MapToDto(taskItem);
    }

    public async Task<TaskItemDto> CancelAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        TaskItem taskItem = await FindOrThrowAsync(id, cancellationToken);
        taskItem.Cancel();
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return MapToDto(taskItem);
    }

    public async Task<TaskItemDto> ReopenAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        TaskItem taskItem = await FindOrThrowAsync(id, cancellationToken);
        taskItem.Reopen();
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return MapToDto(taskItem);
    }

    public async Task<TaskItemDto> ChangePriorityAsync(int id, TaskItemPriority priority, CancellationToken cancellationToken = default(CancellationToken))
    {
        TaskItem taskItem = await FindOrThrowAsync(id, cancellationToken);
        taskItem.ChangePriority(priority);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return MapToDto(taskItem);
    }

    public async Task<TaskItemDto> AssignCategoryAsync(int id, int categoryId, CancellationToken cancellationToken = default(CancellationToken))
    {
        TaskItem taskItem = await FindOrThrowAsync(id, cancellationToken);
        Category category = await _appDbContext.Categories.FindAsync([categoryId], cancellationToken) ?? throw new NotFoundException($"Category with ID {categoryId} not found.");

        taskItem.AssignCategory(category);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return MapToDto(taskItem);
    }

    public async Task<TaskItemDto> RemoveCategoryAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        TaskItem taskItem = await FindOrThrowAsync(id, cancellationToken);
        taskItem.RemoveCategory();
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return MapToDto(taskItem);
    }

    private async Task<TaskItem> FindOrThrowAsync(int id, CancellationToken cancellationToken)
    {
        return await _appDbContext.TaskItems
            .Include(navigationPropertyPath: item => item.Category)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
         ?? throw new NotFoundException($"Task with ID {id} not found.");
    }

    private static TaskItemDto MapToDto(TaskItem t) => new(
        t.Id,
        t.Title,
        t.Description,
        t.Priority,
        t.Status,
        t.DueDate,
        t.CreatedAt,
        t.UpdatedAt,
        t.CompletedAt,
        t.CategoryId,
        t.Category?.Name,
        t.IsOverdue
    );
}
