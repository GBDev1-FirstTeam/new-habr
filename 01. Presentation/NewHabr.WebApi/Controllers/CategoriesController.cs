using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;

namespace NewHabr.WebApi.Controllers;

[Route("[controller]")]
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
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("{name}")]
    public async Task<ActionResult> Create([FromBody] string name, CancellationToken cancellationToken)
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
    public async Task<ActionResult> Update([FromBody] CategoryDto categoryToUpdate, CancellationToken cancellationToken)
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
    public async Task<ActionResult> DeleteById([FromRoute] int id, CancellationToken cancellationToken)
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
}
