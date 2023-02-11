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

public class TagServiceTests
{
    private readonly ApplicationContext _context;
    private readonly ITagService _tagService;
    private readonly IMapper _mapper;

    public TagServiceTests()
    {
        _mapper = Factories.GetMapper();
        _context = Factories.GetDataContext();
        _tagService = new TagService(new RepositoryManager(_context), _mapper);
    }

    [Fact]
    public async Task GetAllAsync__ReturnsCorrectAvaliableTagCount()
    {
        // arrange
        for (int i = 0; i < 3; i++)
        {
            await CreateTagAsync($"test: {i}");
        }

        // act
        var result = await _tagService.GetAllAsync();

        // asset
        Assert.Equal(result.Count, _context.Tags.Count(a => !a.Deleted));

        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task CreateAsync__ExistsTag__ThrowAlreadyExistsException()
    {
        // arrange
        var existsTag = await CreateTagAsync("test");
        var request = new CreateTagRequest { Name = existsTag.Name };

        // act, assert
        await Assert.ThrowsAsync<TagAlreadyExistsException>(async () => await _tagService.CreateAsync(request));

        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task CreateAsync__NotExistsTag__CreatedNewTag()
    {
        // arrange
        var request = new CreateTagRequest { Name = "test" };

        // act
        await _tagService.CreateAsync(request);

        // assert
        var newTag = await _context.Tags.FirstOrDefaultAsync();

        Assert.NotNull(newTag);
        Assert.Equal(request.Name, newTag.Name);

        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task UpdateAsync__NotExistsTag__ThrowNotFoundException()
    {
        // arrange
        var request = new UpdateTagRequest { Name = "test" };

        // act, assert
        await Assert.ThrowsAsync<TagNotFoundException>(async () => await _tagService.UpdateAsync(1, request));
    }

    [Fact]
    public async Task UpdateAsync__TagWithNewNameIsAlreadyExists__ThrowAlreadyExistsException()
    {
        // arrange
        var existsTag1 = await CreateTagAsync("test1");
        var existsTag2 = await CreateTagAsync("test2");
        var request = new UpdateTagRequest { Name = existsTag2.Name };

        // act, assert
        await Assert.ThrowsAsync<TagAlreadyExistsException>(async () => await _tagService.UpdateAsync(existsTag1.Id, request));

        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task UpdateAsync__ValidRequest__ExistsTagUpdatedSuccess()
    {
        // arrange
        var existsTag1 = await CreateTagAsync("test");
        var newName = "test1";
        var request = new UpdateTagRequest { Name = newName };

        // act
        await _tagService.UpdateAsync(existsTag1.Id, request);

        // assert
        Assert.Equal(existsTag1.Name, newName);

        await ClearContextFromEntitiesAsync<Tag, int>();
    }

    [Fact]
    public async Task DeleteByIdAsync__NotExistsTag__ThrowNotFoundException()
    {
        await Assert.ThrowsAsync<TagNotFoundException>(async () => await _tagService.DeleteByIdAsync(1));
    }

    [Fact]
    public async Task DeleteByIdAsync__ExistsTag__DeletedSuccessAndClearLinks()
    {
        // arrange
        var existsTag = await CreateTagAsync("test");
        var tags = new List<Tag>();
        tags.Add(existsTag);
        var article = await _context.AddAsync(new Article
        {
            Title = "test",
            Content = "test",
            UserId = default(Guid),
            Tags = tags
        });

        // act
        await _tagService.DeleteByIdAsync(existsTag.Id);

        // assert
        Assert.True(existsTag.Deleted);
        Assert.Empty(article.Entity.Categories);

        await ClearContextFromEntitiesAsync<Article, Guid>();
        await ClearContextFromEntitiesAsync<Tag, int>();
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
