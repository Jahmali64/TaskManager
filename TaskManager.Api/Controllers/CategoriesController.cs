using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;
using TaskManager.Contracts.DTOs;
using TaskManager.Contracts.Requests;

namespace TaskManager.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/categories")]
public sealed class CategoriesController : ControllerBase
{
    private const string ApiVersion = "1.0";

    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<CategoryDto> categories = await _categoryService.GetAllAsync(cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id, CancellationToken cancellationToken)
    {
        CategoryDto category = await _categoryService.GetByIdAsync(id, cancellationToken);
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryCreateRequest request, CancellationToken cancellationToken)
    {
        CategoryDto category = await _categoryService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById),
            routeValues: new
            {
                id = category.Id,
                version = ApiVersion
            },
            category);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] CategoryUpdateRequest request, CancellationToken cancellationToken)
    {
        CategoryDto category = await _categoryService.UpdateAsync(id, request, cancellationToken);
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}