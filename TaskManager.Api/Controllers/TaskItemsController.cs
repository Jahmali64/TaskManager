using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;
using TaskManager.Contracts.DTOs;
using TaskManager.Contracts.Requests;

namespace TaskManager.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/task-items")]
public sealed class TaskItemsController : ControllerBase
{
    private const string ApiVersion = "1.0";

    private readonly ITaskItemService _taskItemService;

    public TaskItemsController(ITaskItemService taskItemService) => _taskItemService = taskItemService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TaskItemDto>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<TaskItemDto> items = await _taskItemService.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItemDto>> GetById(int id, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.GetByIdAsync(id, cancellationToken);
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> Create([FromBody] TaskItemCreateRequest request, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById),
            routeValues: new
            {
                id = item.Id,
                version = ApiVersion
            },
            item);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskItemDto>> Update(int id, [FromBody] TaskItemUpdateRequest request, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.UpdateAsync(id, request, cancellationToken);
        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _taskItemService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/complete")]
    public async Task<ActionResult<TaskItemDto>> Complete(int id, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.CompleteAsync(id, cancellationToken);
        return Ok(item);
    }

    [HttpPost("{id:int}/start")]
    public async Task<ActionResult<TaskItemDto>> Start(int id, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.StartAsync(id, cancellationToken);
        return Ok(item);
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<ActionResult<TaskItemDto>> Cancel(int id, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.CancelAsync(id, cancellationToken);
        return Ok(item);
    }

    [HttpPost("{id:int}/reopen")]
    public async Task<ActionResult<TaskItemDto>> Reopen(int id, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.ReopenAsync(id, cancellationToken);
        return Ok(item);
    }

    [HttpPatch("{id:int}/priority")]
    public async Task<ActionResult<TaskItemDto>> ChangePriority(int id, [FromBody] TaskItemChangePriorityRequest request, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.ChangePriorityAsync(id, request.Priority, cancellationToken);
        return Ok(item);
    }

    [HttpPut("{id:int}/category/{categoryId:int}")]
    public async Task<ActionResult<TaskItemDto>> AssignCategory(int id, int categoryId, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.AssignCategoryAsync(id, categoryId, cancellationToken);
        return Ok(item);
    }

    [HttpDelete("{id:int}/category")]
    public async Task<ActionResult<TaskItemDto>> RemoveCategory(int id, CancellationToken cancellationToken)
    {
        TaskItemDto item = await _taskItemService.RemoveCategoryAsync(id, cancellationToken);
        return Ok(item);
    }
}