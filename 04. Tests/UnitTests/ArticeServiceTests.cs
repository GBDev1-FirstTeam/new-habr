using AutoMapper;
using Moq;
using NewHabr.Business.AutoMapperProfiles;
using NewHabr.Business.Services;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace UnitTests;

public class ArticeServiceTests
{
    private IMapper _mapper;
    private Mock<IRepositoryManager> _repositoryManagerMock;
    private Mock<IArticleRepository> _articleRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<INotificationService> _notificationServiceMock;

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


    [Fact]
    public async Task CreateAsync_ValidModel_CreatesArticle()
    {
        // Arrange
        var createRequest = new ArticleCreateRequest
        {
            Title = "unittest title",
            Content = "unittest content",
            ImgURL = "unittest imgurl",
            //Categories = new CategoryUpdateRequest[]
            //{
            //    new CategoryUpdateRequest { Name = "unittest category 01"}
            //},
            //Tags = new TagCreateRequest[]
            //{
            //    new TagCreateRequest{Name = "unittestTag01"}
            //}
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
