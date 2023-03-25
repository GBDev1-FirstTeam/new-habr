using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;

[ApiController, Route("api/[controller]")]
[Produces("application/json")]
[Authorize(Roles = "Administrator, Moderator")]
public class SecureQuestionsController : ControllerBase
{
    private readonly ISecureQuestionsService _secureQuestionsService;


    public SecureQuestionsController(ISecureQuestionsService secureQuestionsService)
    {
        _secureQuestionsService = secureQuestionsService;
    }


    /// <summary>
    /// Retrieves all secure questions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult<ICollection<SecureQuestionDto>>> GetQuestions(CancellationToken cancellationToken)
    {
        var questions = await _secureQuestionsService.GetAllAsync(cancellationToken);
        return Ok(questions);
    }

    /// <summary>
    /// Creates a new secure question
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateQuestion(SecureQuestionCreateRequest request, CancellationToken cancellationToken)
    {
        await _secureQuestionsService.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Updates a specific Notification
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateQuestion([FromRoute] int id, [FromBody] SecureQuestionUpdateRequest request, CancellationToken cancellationToken)
    {
        await _secureQuestionsService.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific Notification
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteQuestion([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _secureQuestionsService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}

