using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;

[ApiController, Route("api/[controller]")]
[Authorize(Roles = "Administrator")]
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
        try
        {
            await _secureQuestionsService.CreateAsync(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestion([FromRoute] int id, [FromBody] SecureQuestionUpdateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _secureQuestionsService.UpdateAsync(id, request, cancellationToken);
        }
        catch (SecureQuestionNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            await _secureQuestionsService.DeleteAsync(id, cancellationToken);
        }
        catch (SecureQuestionNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}

