using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Moq;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using AutoFixture;
using Microsoft.AspNetCore.Authorization;

namespace UnitTests.Controllers;

public class ArticlesControllerTests
{
    public class CreateArticle : ControllerTestsBase
    {
        private readonly Mock<IArticleService> _articleServiceMock;
        private readonly ArticlesController _sut;


        public CreateArticle()
        {
            _articleServiceMock = new();

            _sut = new ArticlesController(_articleServiceMock.Object, _authorizationService.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task SHOULD_return_created_article_and_StatusCode_201()
        {
            // Arrange
            var request = _fixture.Create<ArticleCreateRequest>();

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
            var articleDto = new ArticleDto();
            _articleServiceMock
                .Setup(s => s.CreateAsync(request, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(articleDto);

            // Act
            var response = await _sut.Create(request, default);

            // Assert
            Assert.IsAssignableFrom<ActionResult<ArticleDto>>(response);

            Assert.IsAssignableFrom<ObjectResult>(response.Result);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)response.Result!).StatusCode);

            _articleServiceMock.Verify(service => service.CreateAsync(It.IsAny<ArticleCreateRequest>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    public class GetById : ControllerTestsBase
    {
        private Mock<IArticleService> _articleServiceMock;
        private readonly ArticlesController _sut;


        public GetById()
        {
            _articleServiceMock = new();
            _sut = new ArticlesController(_articleServiceMock.Object, _authorizationService.Object, _loggerMock.Object);
        }

        [Fact]
        public void SHOULD_call_service_WHEN_user_is_authorized()
        {
            // Arrange
            var userId = _fixture.Create<Guid>();
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, "unittestUser"),
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            var articleId = _fixture.Create<Guid>();

            _articleServiceMock
                .Setup(s => s.GetByIdAsync(articleId, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ArticleDto());

            // Act
            var articledto = _sut.GetById(articleId, default);

            // Assert
            _articleServiceMock
                .Verify(s => s.GetByIdAsync(articleId, userId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void SHOULD_call_service_WHEN_user_is_not_authorized()
        {
            // Arrange
            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };
            var articleId = _fixture.Create<Guid>();

            _articleServiceMock
                .Setup(s => s.GetByIdAsync(articleId, Guid.Empty, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ArticleDto());

            // Act
            var articledto = _sut.GetById(articleId, default);

            // Assert
            _articleServiceMock
                .Verify(s => s.GetByIdAsync(articleId, Guid.Empty, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}

