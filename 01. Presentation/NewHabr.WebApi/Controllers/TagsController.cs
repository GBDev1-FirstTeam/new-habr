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
    private readonly ILogger<TagsController> _logger;

    public TagsController(ITagService tagService, ILogger<TagsController> logger)
    {
        _tagService = tagService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _tagService.GetAllAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTagRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _tagService.CreateAsync(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (TagAlreadyExistsException ex)
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
        [FromBody] UpdateTagRequest tagToUpdate,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tagService.UpdateAsync(id, tagToUpdate, cancellationToken);
            return Ok();
        }
        catch (TagNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\ntagToUpdate: {@tagToUpdate}"), tagToUpdate);
            return NotFound(ex.Message);
        }
        catch (TagAlreadyExistsException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\ntagToUpdate: {@tagToUpdate}"), tagToUpdate);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\ntagToUpdate: {@tagToUpdate}"), tagToUpdate);
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
        catch (TagNotFoundException ex)
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
