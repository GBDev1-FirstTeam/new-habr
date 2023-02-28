using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;
using NewHabr.WebApi.Controllers;
using NewHabr.Domain.Exceptions;
using NewHabr.WebApi.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UnitTests.Controllers;

public class CommentsControllerTests
{
    private readonly CommentsController _commentsController;
    private readonly Mock<ICommentService> _commentServiceMock;
    private readonly Mock<ILogger<CommentsController>> _loggerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IUserStore<User>> _userStoreMock;
    private readonly Comment _comment;
    private readonly User _user;


    public CommentsControllerTests()
    {
        _commentServiceMock = new Mock<ICommentService>();
        _loggerMock = new Mock<ILogger<CommentsController>>();
        _comment = new Comment() { Id = Guid.NewGuid() };
        _userStoreMock = new Mock<IUserStore<User>>();

        _commentsController = new CommentsController(_commentServiceMock.Object, _loggerMock.Object);

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
        _commentsController.ControllerContext = new ControllerContext();
        _commentsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
        _user = new User() { Id = user.GetUserId() };
    }


    [Fact]
    public async Task GetAll_ShouldCall_GetAllAsync_From_Service_Once()
    {
        //Arrange
        //Act
        var result = await _commentsController.GetAll(It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ActionResult<List<CommentDto>>>(result);
        _commentServiceMock.Verify(service => service.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_ShouldCall_CreateAsync_From_Service_Once()
    {
        //Arrange        
        //Act
        var result = await _commentsController.Create(It.IsAny<CommentCreateRequest>(), It.IsAny<CancellationToken>());
        var okResult = result as OkResult;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, okResult.StatusCode);

        _commentServiceMock.Verify(service => service.CreateAsync(_user.Id,
            It.IsAny<CommentCreateRequest>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

    }

    [Fact]
    public async Task Update_ShouldCall_UpdateAsync_From_Service_Once()
    {
        //Arrange               
        //Act
        var result = await _commentsController.Update(_comment.Id,
            It.IsAny<CommentUpdateRequest>(),
            It.IsAny<CancellationToken>());
        var okResult = result as OkResult;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, okResult.StatusCode);

        _commentServiceMock.Verify(service =>
            service.UpdateAsync(_comment.Id,
                _user.Id,
                It.IsAny<CommentUpdateRequest>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldCall_DeleteAsync_From_Service_Once()
    {
        //Arrange        
        //Act
        var result = await _commentsController.Delete(_comment.Id, It.IsAny<CancellationToken>());
        var okResult = result as OkResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, okResult.StatusCode);
        _commentServiceMock.Verify(service => service.DeleteAsync(_comment.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SetLike_ShouldCall_SetLikeAsync_From_Service_Once()
    {
        //Arrange       
        //Act
        var result = await _commentsController.SetLike(_comment.Id, It.IsAny<CancellationToken>());
        var okResult = result as NoContentResult;
        // Assert
        Assert.NotNull(result);

        Assert.Equal(204, okResult.StatusCode);

        _commentServiceMock.Verify(service =>
            service.SetLikeAsync(_comment.Id,
                _user.Id,
                It.IsAny<CancellationToken>()),
                Times.Once);
    }

    [Fact]
    public async Task UnsetLike_ShouldCall_UnsetLikeAsync_From_Service_Once()
    {
        //Arrange
        //Act
        var result = await _commentsController.UnsetLike(_comment.Id, It.IsAny<CancellationToken>());
        var okResult = result as NoContentResult;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(204, okResult.StatusCode);

        _commentServiceMock.Verify(service =>
            service.UnsetLikeAsync(_comment.Id,
                _user.Id,
                It.IsAny<CancellationToken>()),
                Times.Once);
    }

    [Fact]
    public async Task Create_WhenUserBanned_Returns403()
    {
        //Arrange
        _commentServiceMock.Setup(s =>
            s.CreateAsync(
                _user.Id,
                It.IsAny<CommentCreateRequest>(),
                It.IsAny<CancellationToken>()))
            .Throws(new UserBannedException(DateTimeOffset.Now));
        //Act
        var result = await _commentsController.Create(It.IsAny<CommentCreateRequest>(), It.IsAny<CancellationToken>());
        var okResult = result as ObjectResult;
        //Assert        
        Assert.NotNull(okResult);
        Assert.Equal(403, okResult.StatusCode);
    }

    [Fact]
    public async Task Update_WhenUserBanned_Returns403()
    {
        //Arrange        
        _commentServiceMock.Setup(s =>
            s.UpdateAsync(_comment.Id,
                _user.Id,
                It.IsAny<CommentUpdateRequest>(),
                It.IsAny<CancellationToken>()))
            .Throws(new UserBannedException(DateTimeOffset.Now));
        //Act
        var result = await _commentsController.Update(
            _comment.Id,
            It.IsAny<CommentUpdateRequest>(),
            It.IsAny<CancellationToken>());
        var okResult = result as ObjectResult;
        //Assert        
        Assert.NotNull(okResult);
        Assert.Equal(403, okResult.StatusCode);
    }

    [Fact]
    public async Task SetLike_WhenUserBanned_Returns403()
    {
        //Arrange        
        _commentServiceMock.Setup(s => s.SetLikeAsync(_comment.Id, _user.Id, It.IsAny<CancellationToken>()))
            .Throws(new UserBannedException(DateTimeOffset.Now));
        //Act
        var result = await _commentsController.SetLike(_comment.Id, It.IsAny<CancellationToken>());
        var okResult = result as ObjectResult;
        //Assert        
        Assert.NotNull(okResult);
        Assert.Equal(403, okResult.StatusCode);
    }

    [Fact]
    public async Task Update_WhenCommentNotFound_Returns404()
    {
        //Arrange        
        _commentServiceMock.Setup(s =>
            s.UpdateAsync(
                _comment.Id,
                _user.Id,
                It.IsAny<CommentUpdateRequest>(),
                It.IsAny<CancellationToken>()))
            .Throws(new CommentNotFoundException());
        //Act
        var result = await _commentsController.Update(
            _comment.Id,
            It.IsAny<CommentUpdateRequest>(),
            It.IsAny<CancellationToken>());
        var okResult = result as NotFoundObjectResult;
        //Assert        
        Assert.NotNull(okResult);
        Assert.Equal(404, okResult.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenCommentNotFound_Returns404()
    {
        //Arrange        
        _commentServiceMock.Setup(s =>
            s.DeleteAsync(
                _comment.Id,
                It.IsAny<CancellationToken>()))
            .Throws(new CommentNotFoundException());
        //Act
        var result = await _commentsController.Delete(
            _comment.Id,
            It.IsAny<CancellationToken>());
        var okResult = result as NotFoundResult;
        //Assert        
        Assert.NotNull(okResult);
        Assert.Equal(404, okResult.StatusCode);
    }

    [Fact]
    public async Task SetLike_WhenCommentNotFound_Returns404()
    {
        //Arrange        
        _commentServiceMock.Setup(s =>
            s.SetLikeAsync(
                _comment.Id,
                _user.Id,
                It.IsAny<CancellationToken>()))
            .Throws(new CommentNotFoundException());
        //Act
        var result = await _commentsController.SetLike(
            _comment.Id,
            It.IsAny<CancellationToken>());
        var okResult = result as NotFoundObjectResult;
        //Assert        
        Assert.NotNull(okResult);
        Assert.Equal(404, okResult.StatusCode);
    }

    [Fact]
    public async Task UnsetLike_WhenCommentNotFound_Returns404()
    {
        //Arrange        
        _commentServiceMock.Setup(s =>
            s.UnsetLikeAsync(
                _comment.Id,
                _user.Id,
                It.IsAny<CancellationToken>()))
            .Throws(new CommentNotFoundException());
        //Act
        var result = await _commentsController.UnsetLike(
            _comment.Id,
            It.IsAny<CancellationToken>());
        var okResult = result as NotFoundObjectResult;
        //Assert        
        Assert.NotNull(okResult);
        Assert.Equal(404, okResult.StatusCode);
    }

    [Fact]
    public async Task GetAll_WhenAnyException_Returns500()
    {
        //Arrange
        _commentServiceMock.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .Throws(new Exception());
        //Act
        var result = await _commentsController.GetAll(It.IsAny<CancellationToken>());

        var okResult = result.Result as StatusCodeResult;

        //Assert
        Assert.NotNull(okResult);
        Assert.Equal(500, okResult.StatusCode);
    }

    [Fact]
    public async Task Create_WhenAnyException_Returns500()
    {
        //Arrange
        _commentServiceMock.Setup(s =>
            s.CreateAsync(_user.Id,
                It.IsAny<CommentCreateRequest>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());
        //Act
        var result = await _commentsController.Create(It.IsAny<CommentCreateRequest>(), It.IsAny<CancellationToken>());

        var okResult = result as StatusCodeResult;

        //Assert
        Assert.NotNull(okResult);
        Assert.Equal(500, okResult.StatusCode);
    }

    [Fact]
    public async Task Update_WhenAnyException_Returns500()
    {
        //Arrange
        _commentServiceMock.Setup(s => s.UpdateAsync(_comment.Id, _user.Id, It.IsAny<CommentUpdateRequest>(), It.IsAny<CancellationToken>())).Throws(new Exception());
        //Act
        var result = await _commentsController.Update(_comment.Id, It.IsAny<CommentUpdateRequest>(), It.IsAny<CancellationToken>());

        var okResult = result as StatusCodeResult;

        //Assert
        Assert.NotNull(okResult);
        Assert.Equal(500, okResult.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenAnyException_Returns500()
    {
        //Arrange
        _commentServiceMock.Setup(s => s.DeleteAsync(_comment.Id, It.IsAny<CancellationToken>())).Throws(new Exception());
        //Act
        var result = await _commentsController.Delete(_comment.Id, It.IsAny<CancellationToken>());

        var okResult = result as StatusCodeResult;

        //Assert
        Assert.NotNull(okResult);
        Assert.Equal(500, okResult.StatusCode);
    }
}
