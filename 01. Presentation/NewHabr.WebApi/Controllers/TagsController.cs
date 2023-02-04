using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _tagService.GetAllAsync(cancellationToken));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTagRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _tagService.CreateAsync(request, cancellationToken);
            return Ok();
        }
        catch (TagAlreadyExistsException)
        {
            return BadRequest();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] TagDto tagToUpdate, CancellationToken cancellationToken)
    {
        try
        {
            await _tagService.UpdateAsync(tagToUpdate, cancellationToken);
            return Ok();
        }
        catch (TagNotFoundException)
        {
            return NotFound();
        }
        catch (TagAlreadyExistsException)
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
            await _tagService.DeleteByIdAsync(id, cancellationToken);
            return Ok();
        }
        catch (TagNotFoundException)
        {
            return NotFound();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
