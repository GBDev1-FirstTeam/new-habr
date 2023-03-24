using System.Security.Claims;
using AutoFixture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using NewHabr.WebApi.Controllers;

namespace UnitTests.Controllers;
public class ControllerTestsBase
{
    protected Mock<IAuthorizationService> _authorizationService;
    protected Mock<ILogger<ArticlesController>> _loggerMock = new();
    protected Fixture _fixture = new();

    public ControllerTestsBase()
    {
        _authorizationService = new Mock<IAuthorizationService>();
        _authorizationService
            .Setup(s => s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(() => AuthorizationResult.Success());
    }
}
