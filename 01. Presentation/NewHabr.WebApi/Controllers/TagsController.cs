using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;

namespace NewHabr.WebApi.Controllers;

[Route("[controller]")]
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
            await _tagService.CreateAsync(name, cancellationToken);
            return Ok();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] TagDto tagToUpdate, CancellationToken cancellationToken)
    {
        if (tagToUpdate is null)
        {
            return BadRequest();
        }

        try
        {
            await _tagService.UpdateAsync(tagToUpdate, cancellationToken);
            return Ok();
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
            await _tagService.DeleteByIdAsync(id, cancellationToken);
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
