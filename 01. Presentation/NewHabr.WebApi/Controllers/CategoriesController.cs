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

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _categoryService.GetAllAsync(cancellationToken));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _categoryService.CreateAsync(request, cancellationToken);
            return Ok();
        }
        catch (CategoryAlreadyExistsException)
        {
            return BadRequest();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] CategoryDto categoryToUpdate, CancellationToken cancellationToken)
    {
        try
        {
            await _categoryService.UpdateAsync(categoryToUpdate, cancellationToken);
            return Ok();
        }
        catch (CategoryNotFoundException)
        {
            return NotFound();
        }
        catch (CategoryAlreadyExistsException)
        {
            return BadRequest();
        }
        catch
        {
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
        catch (CategoryNotFoundException)
        {
            return NotFound();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
