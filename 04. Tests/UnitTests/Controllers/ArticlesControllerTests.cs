using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Moq;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.WebApi.Controllers;
using Microsoft.AspNetCore.Http;

namespace UnitTests.Controllers;

public class ArticlesControllerTests
{
    private Mock<IArticleService> _articleServiceMock;
    private Mock<ILogger<ArticlesController>> _loggerMock;
    private readonly ArticlesController _sut;


    public ArticlesControllerTests()
    {
        _loggerMock = new();
        _articleServiceMock = new();

        _sut = new ArticlesController(_articleServiceMock.Object, _loggerMock.Object);
    }


    [Fact]
    public async Task CreateArticle_ModelWoCategoriesTags_ReturnsStatusCode201()
    {
        // Arrange
        var request = new ArticleCreateRequest
        {
            Title = "unittest title",
            Content = "unittest content",
            ImgURL = "unittest imgurl"
        };

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "unittestUser"),
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var response = await _sut.Create(request, default);

        // Assert
        Assert.IsType<StatusCodeResult>(response);
        Assert.Equal(201, ((StatusCodeResult)response).StatusCode);
        _articleServiceMock.Verify(service => service.CreateAsync(It.IsAny<ArticleCreateRequest>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

