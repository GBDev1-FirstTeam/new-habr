using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;

[Authorize]
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

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _categoryService.GetAllAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _categoryService.CreateAsync(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (CategoryAlreadyExistsException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nrequest: {@request}"), request);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nrequest: {@request}"), request);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(
        [FromRoute, Range(1, int.MaxValue)] int id,
        [FromBody] UpdateCategoryRequest categoryToUpdate,
        CancellationToken cancellationToken)
    {
        try
        {
            await _categoryService.UpdateAsync(id, categoryToUpdate, cancellationToken);
            return Ok();
        }
        catch (CategoryNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\ncategoryToUpdate: {@categoryToUpdate}"), categoryToUpdate);
            return NotFound(ex.Message);
        }
        catch (CategoryAlreadyExistsException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\ncategoryToUpdate: {@categoryToUpdate}"), categoryToUpdate);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\ncategoryToUpdate: {@categoryToUpdate}"), categoryToUpdate);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteById([FromRoute, Range(1, int.MaxValue)] int id, CancellationToken cancellationToken)
    {
        try
        {
            await _categoryService.DeleteByIdAsync(id, cancellationToken);
            return Ok();
        }
        catch (CategoryNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
