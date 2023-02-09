using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewHabr.Business.Services;
using NewHabr.DAL.EF;
using NewHabr.DAL.Repository;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Tests;

public class ArticleServiceTests
{
    private readonly ApplicationContext _context;
    private readonly IArticleService _articleService;
    private readonly IMapper _mapper;
    private readonly User _user;

    public ArticleServiceTests()
    {
        _mapper = Factories.GetMapper();
        _context = Factories.GetDataContext();
        _articleService = new ArticleService(new RepositoryManager(_context), _mapper);
        _user = _context.Add(new User
        {
            SecureAnswer = "test",
            SecureQuestion = new SecureQuestion
            {
                Id = default,
                Question = "test secure question"
            }
        }).Entity;
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetByIdAsync__NotExistsArticleId__ThrowNotFoundException()
    {
        // arrange
        var id = default(Guid);

        // act, assert
        await Assert.ThrowsAsync<ArticleNotFoundException>(async () => await _articleService.GetByIdAsync(id));
    }

    [Fact]
    public async Task GetByIdAsync__ExistsArticleId__ReturnsCorrectArticleDto()
    {
        // arrange
        var article = await CreateArticleAsync(saveChanges: true);
        var articleDto = _mapper.Map<ArticleDto>(article);

        // act
        var receivedArticleDto = await _articleService.GetByIdAsync(article.Id);

        // assert
        Assert.True(Equals(articleDto, receivedArticleDto));

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task GetUnpublishedAsync__ReturnsCorrectUnpublishedArticlesCount()
    {
        // arrange
        for (int i = 0; i < 3; i++)
        {
            var newArticle = await CreateArticleAsync();
            newArticle.Published = i % 2 == 0;
        }
        await _context.SaveChangesAsync();

        // act
        var result = await _articleService.GetUnpublishedAsync();

        // asset
        Assert.Equal(result.Count, _context.Articles.Count(a => !a.Deleted && !a.Published));

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task GetDeletedAsync__ReturnsCorrectDeletedArticlesCount()
    {
        // arrange
        for (int i = 0; i < 3; i++)
        {
            var newArticle = await CreateArticleAsync();
            newArticle.Deleted = i % 2 == 0;
        }
        await _context.SaveChangesAsync();

        // act
        var result = await _articleService.GetDeletedAsync();

        // asset
        Assert.Equal(result.Count, _context.Articles.Count(a => a.Deleted));

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task CreateAsync__NotExistsUserId__ThrowNotFoundException()
    {
        // arrange
        var request = new CreateArticleRequest
        {
            UserId = default(Guid),
            Title = "test",
            Content = "test"
        };

        // act, assert
        await Assert.ThrowsAsync<UserNotFoundException>(async () => await _articleService.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync__ValidRequest__CreatedCorrectEntity()
    {
        // arrange
        var request = new CreateArticleRequest
        {
            UserId = _user.Id,
            Title = "test",
            Content = "test"
        };

        // act
        var id = await _articleService.CreateAsync(request);

        // assert
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        Assert.NotNull(article);
        Assert.True(
            article.Content == request.Content &&
            article.Title == request.Title &&
            article.UserId == request.UserId);

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task CreateAsync__NotExistsCategory__ThrowNotFoundException()
    {
        // arrange
        var request = new CreateArticleRequest
        {
            UserId = _user.Id,
            Title = "test",
            Content = "test",
            Categories = new[] { new CreateCategoryRequest { Name = "test" } }
        };

        // act, assert
        await Assert.ThrowsAsync<CategoryNotFoundException>(async () => await _articleService.CreateAsync(request));
    }

    [Fact]
    public async Task CreatAsync__ExistsCategory__NotDuplicatedInCategorySet()
    {
        // arrange
        var existsCategory = await CreateCategoryAsync("test", saveChanges: true);
        var request = new CreateArticleRequest
        {
            UserId = _user.Id,
            Title = "test",
            Content = "test",
            Categories = new[]
            {
                new CreateCategoryRequest { Name = existsCategory.Name },
                new CreateCategoryRequest { Name = existsCategory.Name }
            },
        };

        // act
        await _articleService.CreateAsync(request);
        var count = _context.Categories.Count(c => c.Name == existsCategory.Name && !c.Deleted);

        // assert
        Assert.True(count == 1);

        await ClearContextFromEntitiesAsync<Article, Guid>();
        await ClearContextFromEntitiesAsync<Category, int>();
    }

    [Fact]
    public async Task CreateAsync__NotExistsTag__CreatedNewTag()
    {
        // arrange
        var tagNameToAdd = "test";
        var request = new CreateArticleRequest
        {
            UserId = _user.Id,
            Title = "test",
            Content = "test",
            Tags = new[] { new CreateTagRequest { Name = tagNameToAdd } },
        };

        // act
        var result = await _articleService.CreateAsync(request);

        // assert
        Assert.True(await _context.Tags.AnyAsync(t => t.Name == tagNameToAdd));

        await ClearContextFromEntitiesAsync<Article, Guid>();
        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task CreateAsync__NotExistsTag__NotDuplicatedInTagSet()
    {
        // arrange
        var tagNameToAdd = "test";
        var request = new CreateArticleRequest
        {
            UserId = _user.Id,
            Title = "test",
            Content = "test",
            Tags = new[]
            {
                new CreateTagRequest { Name = tagNameToAdd },
                new CreateTagRequest { Name = tagNameToAdd }
            }
        };

        // act
        await _articleService.CreateAsync(request);

        // assert
        var count = _context.Tags.Count(t => t.Name == tagNameToAdd && !t.Deleted);
        Assert.True(count == 1);
    }

    [Fact]
    public async Task CreateAsync__ExistsTag__NotDuplicatedInTagSet()
    {
        // arrange
        var existsTag = await CreateTagAsync("test", saveChanges: true);
        var request = new CreateArticleRequest
        {
            UserId = _user.Id,
            Title = "test",
            Content = "test",
            Tags = new[] { new CreateTagRequest { Name = existsTag.Name } },
        };

        // act
        var result = await _articleService.CreateAsync(request);

        // assert
        var count = _context.Tags.Count(t => t.Name == existsTag.Name && !t.Deleted);
        Assert.True(count == 1);

        await ClearContextFromEntitiesAsync<Article, Guid>();
        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task CreateAsync__ValidRequest__CreatedAt_ModifiedAt_ShouldChange()
    {
        // arrange
        var request = new CreateArticleRequest
        {
            UserId = _user.Id,
            Title = "test",
            Content = "test",
        };

        // act
        var id = await _articleService.CreateAsync(request);

        // assert
        var createdArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        Assert.True(createdArticle.CreatedAt != default);
        Assert.True(createdArticle.ModifiedAt != default);
    }

    [Fact]
    public async Task UpdateAsync__NotExistsArticleId__ThrowNotFoundException()
    {
        // arrange
        var existsArticle = await CreateArticleAsync(saveChanges: true);
        var request = new UpdateArticleRequest
        {
            Title = "test",
            Content = "test"
        };

        // act, assert
        await Assert.ThrowsAsync<ArticleNotFoundException>(async () => await _articleService.UpdateAsync(default(Guid), request));

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task UpdateAsync__ValidRequest__CorrectUpdatedEntity()
    {
        // arrange
        var existsArticle = await CreateArticleAsync(saveChanges: true);
        var request = new UpdateArticleRequest
        {
            Title = "test",
            Content = "test"
        };

        // act
        await _articleService.UpdateAsync(existsArticle.Id, request);

        // assert
        Assert.NotNull(existsArticle);
        Assert.True(request.Title == existsArticle.Title
            && request.Content == existsArticle.Content);

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task UpdateAsync__NotExistsCategory__ThrowNotFoundException()
    {
        // arrange
        var existsArticle = await CreateArticleAsync(saveChanges: true);
        var request = new UpdateArticleRequest
        {
            Title = "test",
            Content = "test",
            Categories = new[] { new CreateCategoryRequest { Name = "test" } }
        };

        // act, assert
        await Assert.ThrowsAsync<CategoryNotFoundException>(async () => await _articleService.UpdateAsync(existsArticle.Id, request));

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task UpdateAsync__ExistsCategory__NotDuplicatedInCategorySet()
    {
        // arrange
        var existsArticle = await CreateArticleAsync(saveChanges: true);
        var existsCategory = await CreateCategoryAsync("test", saveChanges: true);
        var request = new UpdateArticleRequest
        {
            Title = "test",
            Content = "test",
            Categories = new[] { new CreateCategoryRequest { Name = existsCategory.Name } }
        };

        // act
        await _articleService.UpdateAsync(existsArticle.Id, request);

        // assert
        var count = _context.Categories.Count(c => c.Name == existsCategory.Name && !c.Deleted);
        Assert.True(count == 1);

        await ClearContextFromEntitiesAsync<Article, Guid>();
        await ClearContextFromEntitiesAsync<Category, int>();
    }

    [Fact]
    public async Task UpdateAsync__NotExistsTag__CreateNewTag()
    {
        // arrange
        var tagNameToAdd = "test";
        var existsArticle = await CreateArticleAsync(saveChanges: true);
        var request = new UpdateArticleRequest
        {
            Title = "test",
            Content = "test",
            Tags = new[] { new CreateTagRequest { Name = tagNameToAdd } },
        };

        // act
        await _articleService.UpdateAsync(existsArticle.Id, request);

        // assert
        Assert.True(await _context.Tags.AnyAsync(t => t.Name == tagNameToAdd));

        await ClearContextFromEntitiesAsync<Article, Guid>();
        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task UpdateAsync__NotExistsTag__NotDuplicatedInTagSet()
    {
        // arrange
        var existsArticle = await CreateArticleAsync(saveChanges: true);
        var tagNameToAdd = "test";
        var request = new UpdateArticleRequest
        {
            Title = "test",
            Content = "test",
            Tags = new[]
            {
                new CreateTagRequest { Name = tagNameToAdd },
                new CreateTagRequest { Name = tagNameToAdd }
            }
        };

        // act
        await _articleService.UpdateAsync(existsArticle.Id, request);

        // assert
        var count = _context.Tags.Count(t => t.Name == tagNameToAdd && !t.Deleted);
        Assert.True(count == 1);

        await ClearContextFromEntitiesAsync<Article, Guid>();
        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task UpdateAsync__ExistsTag__NotDuplicatedInTagSet()
    {
        var existsArticle = await CreateArticleAsync(saveChanges: true);
        var existsTag = await CreateTagAsync("test", saveChanges: true);
        var request = new UpdateArticleRequest
        {
            Title = "test",
            Content = "test",
            Tags = new[]
            {
                new CreateTagRequest { Name = existsTag.Name },
                new CreateTagRequest { Name = existsTag.Name }
            }
        };

        // act
        await _articleService.UpdateAsync(existsArticle.Id, request);

        // assert
        var count = _context.Tags.Count(c => c.Name == existsTag.Name && !c.Deleted);
        Assert.True(count == 1);

        await ClearContextFromEntitiesAsync<Article, Guid>();
        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task UpdateAsync__ValidRequest__ModifiedAt_ShouldChange()
    {
        // arrange
        var existsArticle = await CreateArticleAsync();
        existsArticle.ModifiedAt = default(DateTimeOffset);
        await _context.SaveChangesAsync();
        var request = new UpdateArticleRequest { Title = "test", Content = "test" };

        // act
        await _articleService.UpdateAsync(existsArticle.Id, request);

        // assert
        var updatedArticle = await _context.Articles.FirstOrDefaultAsync();
        Assert.NotNull(updatedArticle);
        Assert.True(updatedArticle.ModifiedAt != default(DateTimeOffset));

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task DeleteByIdAsync__NotExistsArticleId__ThrowNotFoundException()
    {
        // act, assert
        await Assert.ThrowsAsync<ArticleNotFoundException>(async () => await _articleService.DeleteByIdAsync(default(Guid)));
    }

    [Fact]
    public async Task DeleteByIdAsync__ValidRequest__Published_PublishedAt_Deleted_DeletedAt_ShouldChange()
    {
        // arrange
        var existsArticle = await CreateArticleAsync();
        existsArticle.PublishedAt = default(DateTimeOffset);
        existsArticle.Published = true;
        await _context.SaveChangesAsync();

        // act
        await _articleService.DeleteByIdAsync(existsArticle.Id);

        // asset
        var deletedArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Id == existsArticle.Id && a.Deleted);

        Assert.NotNull(deletedArticle);
        Assert.NotNull(deletedArticle.DeletedAt);
        Assert.Null(deletedArticle.PublishedAt);
        Assert.True(deletedArticle.Deleted);
        Assert.False(deletedArticle.Published);

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task SetPublicationStatusAsync__NotExistsArticleId__ThrowNotFoundException()
    {
        // act, assert
        await Assert.ThrowsAsync<ArticleNotFoundException>(async () => await _articleService.SetPublicationStatusAsync(default(Guid), true));
    }

    [Fact]
    public async Task SetPublicationStatusAsync__TryPublishArticleWhenNotApprove__ThrowNotApprovedException()
    {
        // arrange
        var existsArticle = await CreateArticleAsync(saveChanges: true);

        // act, assert
        await Assert.ThrowsAsync<ArticleIsNotApproveException>(async () => await _articleService.SetPublicationStatusAsync(existsArticle.Id, true));

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task SetPublicationStatusAsync__PublishApproveArticle__PublishedAt_Published_ShouledChange()
    {
        // arrange
        var existsArticle = await CreateArticleAsync();
        existsArticle.ApproveState = ApproveState.Approved;

        // act
        await _articleService.SetPublicationStatusAsync(existsArticle.Id, true);

        // assert
        var updatedArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Id == existsArticle.Id && !a.Deleted);

        Assert.True(updatedArticle.Published);
        Assert.NotNull(updatedArticle.PublishedAt);

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task SetPublicationStatusAsync__UnpublishArticle__PublishedAt_Published_ShouledChange()
    {
        // arrange
        var existsArticle = await CreateArticleAsync();
        existsArticle.Published = true;
        existsArticle.PublishedAt = default(DateTimeOffset);

        // act
        await _articleService.SetPublicationStatusAsync(existsArticle.Id, false);

        // assert
        var updatedArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Id == existsArticle.Id && !a.Deleted);

        Assert.False(updatedArticle.Published);
        Assert.Null(updatedArticle.PublishedAt);

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }

    [Fact]
    public async Task SetApproveStateAsync__NotExistsArticleId__ThrowNotFoundException()
    {
        // act, assert
        await Assert.ThrowsAsync<ArticleNotFoundException>(
            async () => await _articleService.SetApproveStateAsync(default(Guid), ApproveState.Approved));
    }

    [Fact]
    public async Task SetApproveStateAsync__ValidRequest__CorrectApproveStateOfArticle()
    {
        // arrange
        var existsArticle = await CreateArticleAsync(saveChanges: true);

        // act
        await _articleService.SetApproveStateAsync(existsArticle.Id, ApproveState.Approved);

        // assert
        var updatedArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Id == existsArticle.Id && !a.Deleted);
        Assert.Equal(ApproveState.Approved, updatedArticle.ApproveState);

        await ClearContextFromEntitiesAsync<Article, Guid>();
    }


    private async Task<Article> CreateArticleAsync(bool saveChanges = true)
    {
        var newArticle = (await _context.AddAsync(new Article
        {
            UserId = _user.Id,
            Content = "test",
            Title = "test"
        })).Entity;

        if (saveChanges)
        {
            await _context.SaveChangesAsync();
        }

        return newArticle;
    }

    private async Task<Category> CreateCategoryAsync(string name, bool saveChanges = true)
    {
        var newCategory = (await _context.AddAsync(new Category { Name = name, })).Entity;

        if (saveChanges)
        {
            await _context.SaveChangesAsync();
        }

        return newCategory;
    }

    private async Task<Tag> CreateTagAsync(string name, bool saveChanges = true)
    {
        var newTag = (await _context.AddAsync(new Tag { Name = name, })).Entity;

        if (saveChanges)
        {
            await _context.SaveChangesAsync();
        }

        return newTag;
    }

    private async Task ClearContextFromEntitiesAsync<TEntity, TId>()
        where TEntity : class, IEntity<TId>
        where TId : struct
    {
        _context.Set<TEntity>().RemoveRange(_context.Set<TEntity>());
        await _context.SaveChangesAsync();
    }
}
