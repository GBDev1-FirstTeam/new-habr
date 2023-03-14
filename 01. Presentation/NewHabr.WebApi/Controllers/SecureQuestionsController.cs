using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;

[ApiController, Route("api/[controller]")]
[Authorize(Roles = "Administrator, Moderator")]
public class SecureQuestionsController : ControllerBase
{
    private readonly ISecureQuestionsService _secureQuestionsService;


    public SecureQuestionsController(ISecureQuestionsService secureQuestionsService)
    {
        _secureQuestionsService = secureQuestionsService;
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetQuestions(CancellationToken cancellationToken)
    {
        var questions = await _secureQuestionsService.GetAllAsync(cancellationToken);
        return Ok(questions);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuestion(SecureQuestionCreateRequest request, CancellationToken cancellationToken)
    {
        await _secureQuestionsService.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestion([FromRoute] int id, [FromBody] SecureQuestionUpdateRequest request, CancellationToken cancellationToken)
    {
        await _secureQuestionsService.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _secureQuestionsService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}

