using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;

namespace NewHabr.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return Ok(await _categoryService.GetAllAsync(cancellationToken));
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("{name}")]
    public async Task<ActionResult> CreateAsync([FromRoute] string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest();
        }

        try
        {
            await _categoryService.CreateAsync(name, cancellationToken);
            return Ok();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromQuery] CategoryDto categoryToUpdate, CancellationToken cancellationToken = default)
    {
        if(categoryToUpdate is null)
        {
            return BadRequest();
        }

        try
        {
            await _categoryService.UpdateAsync(categoryToUpdate, cancellationToken);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _categoryService.DeleteByIdAsync(id, cancellationToken);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteByNameAsync([FromQuery] string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest();
        }

        try
        {
            await _categoryService.DeleteByNameAsync(name, cancellationToken);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
