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

public class CategoryServiceTests
{
    private readonly ApplicationContext _context;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoryServiceTests()
    {
        _mapper = Factories.GetMapper();
        _context = Factories.GetDataContext();
        _categoryService = new CategoryService(new RepositoryManager(_context), _mapper);
    }


    [Fact]
    public async Task GetAllAsync__ReturnsCorrectAvaliableCategoriesCount()
    {
        // arrange
        for (int i = 0; i < 3; i++)
        {
            await CreateCategoryAsync($"test: {i}");
        }

        // act
        var result = await _categoryService.GetAllAsync();

        // asset
        Assert.Equal(result.Count, _context.Categories.Count(a => !a.Deleted));

        await ClearContextFromEntitiesAsync<Category, int>();
    }

    [Fact]
    public async Task CreateAsync__ExistsCategory__ThrowAlreadyExistsException()
    {
        // arrange
        var existsCategory = await CreateCategoryAsync("test");
        var request = new CreateCategoryRequest { Name = existsCategory.Name };

        // act, assert
        await Assert.ThrowsAsync<CategoryAlreadyExistsException>(async () => await _categoryService.CreateAsync(request));

        await ClearContextFromEntitiesAsync<Category, int>();
    }

    [Fact]
    public async Task CreateAsync__NotExistsCategory__CreatedNewCategory()
    {
        // arrange
        var request = new CreateCategoryRequest { Name = "test" };

        // act
        await _categoryService.CreateAsync(request);

        // assert
        var newCategory = await _context.Categories.FirstOrDefaultAsync();

        Assert.NotNull(newCategory);
        Assert.Equal(request.Name, newCategory.Name);

        await ClearContextFromEntitiesAsync<Category, int>();
    }

    [Fact]
    public async Task UpdateAsync__NotExistsCategory__ThrowNotFoundException()
    {
        // arrange
        var request = new UpdateCategoryRequest { Name = "test" };

        // act, assert
        await Assert.ThrowsAsync<CategoryNotFoundException>(async () => await _categoryService.UpdateAsync(1, request));
    }

    [Fact]
    public async Task UpdateAsync__CategoryWithNewNameIsAlreadyExists__ThrowAlreadyExistsException()
    {
        // arrange
        var existsCategory1 = await CreateCategoryAsync("test1");
        var existsCategory2 = await CreateCategoryAsync("test2");
        var request = new UpdateCategoryRequest { Name = existsCategory2.Name };

        // act, assert
        await Assert.ThrowsAsync<CategoryAlreadyExistsException>(async () => await _categoryService.UpdateAsync(existsCategory1.Id, request));

        await ClearContextFromEntitiesAsync<Category, int>();
    }

    [Fact]
    public async Task UpdateAsync__ValidRequest__ExistsCategoryUpdatedSuccess()
    {
        // arrange
        var existsCategory1 = await CreateCategoryAsync("test");
        var newName = "test1";
        var request = new UpdateCategoryRequest { Name = newName };

        // act
        await _categoryService.UpdateAsync(existsCategory1.Id, request);

        // assert
        Assert.Equal(existsCategory1.Name, newName);

        await ClearContextFromEntitiesAsync<Category, int>();
    }

    [Fact]
    public async Task DeleteByIdAsync__NotExistsCategory__ThrowNotFoundException()
    {
        await Assert.ThrowsAsync<CategoryNotFoundException>(async () => await _categoryService.DeleteByIdAsync(1));
    }

    [Fact]
    public async Task DeleteByIdAsync__ExistsCategory__DeletedSuccessAndClearLinks()
    {
        // arrange
        var existsCategory = await CreateCategoryAsync("test");
        var categories = new List<Category>();
        categories.Add(existsCategory);
        var article = await _context.AddAsync(new Article
        {
            Title = "test",
            Content = "test",
            UserId = default(Guid),
            Categories = categories
        });

        // act
        await _categoryService.DeleteByIdAsync(existsCategory.Id);

        // assert
        Assert.True(existsCategory.Deleted);
        Assert.Empty(article.Entity.Categories);

        await ClearContextFromEntitiesAsync<Article, Guid>();
        await ClearContextFromEntitiesAsync<Category, int>();
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

    private async Task ClearContextFromEntitiesAsync<TEntity, TId>()
            where TEntity : class, IEntity<TId>
            where TId : struct
    {
        _context.Set<TEntity>().RemoveRange(_context.Set<TEntity>());
        await _context.SaveChangesAsync();
    }
}
