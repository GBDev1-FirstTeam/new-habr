using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.GetAllAsync(cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CategoryCreateRequest request, CancellationToken cancellationToken)
    {
        await _categoryService.CreateAsync(request, cancellationToken);
        return Ok();

    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(
        [FromRoute, Range(1, int.MaxValue)] int id,
        [FromBody] CategoryUpdateRequest categoryToUpdate,
        CancellationToken cancellationToken)
    {
        await _categoryService.UpdateAsync(id, categoryToUpdate, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteById([FromRoute, Range(1, int.MaxValue)] int id, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteByIdAsync(id, cancellationToken);
        return Ok();
    }
}
