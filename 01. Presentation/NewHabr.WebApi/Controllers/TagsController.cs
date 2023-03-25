using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
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

    /// <summary>
    /// Retrieves all Tags
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _tagService.GetAllAsync(cancellationToken));
    }

    /// <summary>
    /// Creates a new tag
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create([FromBody] TagCreateRequest request, CancellationToken cancellationToken)
    {
        await _tagService.CreateAsync(request, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Updates a specific tag
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(
        [FromRoute, Range(1, int.MaxValue)] int id,
        [FromBody] TagUpdateRequest tagToUpdate,
        CancellationToken cancellationToken)
    {
        await _tagService.UpdateAsync(id, tagToUpdate, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Deletes a specific tag
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteById([FromRoute, Range(1, int.MaxValue)] int id, CancellationToken cancellationToken)
    {
        await _tagService.DeleteByIdAsync(id, cancellationToken);
        return Ok();
    }
}
