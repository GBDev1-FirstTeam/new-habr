using AutoFixture;
using AutoMapper;
using Moq;
using NewHabr.Business.AutoMapperProfiles;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Repositories;

namespace UnitTests.Services;

public abstract class ServiceTestsBase
{
    protected readonly Mock<IRepositoryManager> _repositoryManagerMock;
    protected readonly Mock<IUserRepository> _userRepositoryMock;
    protected readonly Mock<INotificationRepository> _notificationRepositoryMock;
    protected readonly Mock<IArticleRepository> _articleRepositoryMock;
    protected readonly IMapper _mapper;
    protected readonly IFixture _fixture;

    protected ServiceTestsBase()
    {
        _mapper = new Mapper(
            configuration: new MapperConfiguration(
                configure: cfg => cfg.AddMaps(typeof(MapperProfile))));
        _fixture = new Fixture();

        _userRepositoryMock = new Mock<IUserRepository>();
        _articleRepositoryMock = new Mock<IArticleRepository>();
        _notificationRepositoryMock = new Mock<INotificationRepository>();
        _repositoryManagerMock = new Mock<IRepositoryManager>();

        _repositoryManagerMock
            .Setup(repman => repman.UserRepository)
            .Returns(_userRepositoryMock.Object);
        _repositoryManagerMock
            .Setup(repman => repman.ArticleRepository)
            .Returns(_articleRepositoryMock.Object);
        _repositoryManagerMock
            .Setup(repman => repman.NotificationRepository)
            .Returns(_notificationRepositoryMock.Object);
    }
}
