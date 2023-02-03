using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;

namespace NewHabr.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class TagController : Controller
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetAllAsync(CancellationToken cancellationToken = default)
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
    public async Task<ActionResult> CreateAsync([FromRoute] string name, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult> UpdateAsync([FromQuery] TagDto tagToUpdate, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult> DeleteByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
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

    [HttpDelete()]
    public async Task<ActionResult> DeleteByNameAsync([FromQuery] string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest();
        }

        try
        {
            await _tagService.DeleteByNameAsync(name, cancellationToken);
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
