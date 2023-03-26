using AutoFixture;
using AutoMapper;
using Moq;
using NewHabr.Business.AutoMapperProfiles;
using NewHabr.Business.Services;
using NewHabr.Domain;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;
using NewHabr.Domain.ServiceModels;

namespace UnitTests.Services;

public class ArticeServiceTests
{
    private IMapper _mapper;
    private Mock<IRepositoryManager> _repositoryManagerMock;
    private Mock<IArticleRepository> _articleRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<INotificationService> _notificationServiceMock;
    private IFixture _fixture = new Fixture();

    //private readonly User _user;
    private readonly ArticleService _sut;


    public ArticeServiceTests()
    {
        ConfigureMapper();
        ConfigureArticleRepository();
        ConfigureUserRepository();
        ConfigureCategoryRepository();
        ConfigureTagRepository();
        ConfigureRepositoryManager();
        ConfigureNotificationService();

        //_user = new User { Id = Guid.NewGuid() };

        _sut = new ArticleService(_repositoryManagerMock.Object, _mapper, _notificationServiceMock.Object);
    }

    [Fact(Skip = "ban restrictions replaced to controller and centralized authorization service")]
    public async Task CreateAsync_UserBanned_CannotCreatesArticle()
    {
        // Arrange
        var createRequest = new ArticleCreateRequest
        {
            Title = "unittest title",
            Content = "unittest content",
            ImgURL = "unittest imgurl",
        };

        var requestingUser = new User { Id = Guid.NewGuid(), Banned = true, BannedAt = DateTimeOffset.UtcNow };
        _userRepositoryMock
            .Setup(ur => ur.GetByIdAsync(requestingUser.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(requestingUser);

        // Act
        var Act = async () => await _sut.CreateAsync(createRequest, requestingUser.Id, default);

        // Assert
        await Assert.ThrowsAsync<UserBannedException>(Act);
    }

    [Fact]
    public async Task CreateAsync_UserNotBanned_CreatesArticle()
    {
        // Arrange
        var createRequest = new ArticleCreateRequest
        {
            Title = "unittest title",
            Content = "unittest content",
            ImgURL = "unittest imgurl",
        };

        var requestingUser = new User { Id = Guid.NewGuid() };
        _userRepositoryMock
            .Setup(ur => ur.GetByIdAsync(requestingUser.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(requestingUser);

        var callArgs = new List<Article>();
        _articleRepositoryMock
            .Setup(ar => ar.Create(It.IsAny<Article>()))
            .Callback((Article article) => callArgs.Add(article));

        // Act
        await _sut.CreateAsync(createRequest, requestingUser.Id, default);

        // Assert
        _repositoryManagerMock.Verify(repman => repman.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Single(callArgs);
        Assert.All(callArgs, item =>
        {
            Assert.InRange(item.CreatedAt, DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddMinutes(1));
            Assert.InRange(item.ModifiedAt, DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddMinutes(1));
            Assert.False(item.Deleted);
            Assert.Equal(default, item.DeletedAt);
            Assert.False(item.Published);
            Assert.Equal(ApproveState.NotApproved, item.ApproveState);
            Assert.Equal(requestingUser.Id, item.UserId);
            Assert.Equal(createRequest.Title, item.Title);
            Assert.Equal(createRequest.Content, item.Content);
        });
    }

    [Theory]
    [InlineData("<p>one</p><p>two</p>", "<p>one</p><p>two</p><p>three</p><p>four</p>")]
    [InlineData("<p>one</p>", "<p>one</p>")]
    [InlineData("<p>one</p><p>two</p>", "<p>one</p><p>two</p>")]
    [InlineData("content without p tags", "content without p tags")]
    [InlineData("", "")]
    public async Task GetPublishedAsync_returns_preview_articles(string expected, string content)
    {
        // Arrange
        var articles = _fixture
            .Build<ArticleModel>()
            .Without(a => a.Comments)
            .Without(a => a.Categories)
            .Without(a => a.Tags)
            .With(a => a.Content, content)
            .CreateMany(10)
            .ToList();
        _articleRepositoryMock
            .Setup(r => r.GetPublishedAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedList<ArticleModel>(articles, 10, 1, 10));
        // Act
        var pagedArticles = await _sut.GetPublishedAsync(Guid.Empty, new ArticleQueryParametersDto(), default);

        // Assert
        Assert.NotNull(articles);
        Assert.Equal(articles.Count, pagedArticles.Articles.Count);
        Assert.All(pagedArticles.Articles, item => Assert.True(item.Content.Equals(expected)));
    }




    private void ConfigureArticleRepository()
    {
        _articleRepositoryMock = new();

    }
    private void ConfigureUserRepository()
    {
        _userRepositoryMock = new();
        //_userRepositoryMock
        //    .Setup(ur => ur.GetByIdAsync(_user.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(_user);

    }
    private void ConfigureCategoryRepository()
    {
        _categoryRepositoryMock = new();
        _categoryRepositoryMock
            .Setup(cr => cr.GetAvaliableAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>
            {
                new Category{Name = "unittest category 01"}
            });
    }
    private void ConfigureTagRepository()
    {
        _tagRepositoryMock = new();
        _tagRepositoryMock
            .Setup(tr => tr.GetAvaliableAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Tag>
            {
                new Tag{Name = "unittestTag01"}
            });
    }
    private void ConfigureNotificationService()
    {
        _notificationServiceMock = new();
    }
    private void ConfigureRepositoryManager()
    {
        _repositoryManagerMock = new();
        _repositoryManagerMock
            .Setup(repman => repman.UserRepository)
            .Returns(_userRepositoryMock.Object);
        _repositoryManagerMock
            .Setup(repman => repman.CategoryRepository)
            .Returns(_categoryRepositoryMock.Object);
        _repositoryManagerMock
            .Setup(repman => repman.TagRepository)
            .Returns(_tagRepositoryMock.Object);
        _repositoryManagerMock
            .Setup(repman => repman.ArticleRepository)
            .Returns(_articleRepositoryMock.Object);
    }

    private void ConfigureMapper()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(typeof(MapperProfile))));
    }
}
