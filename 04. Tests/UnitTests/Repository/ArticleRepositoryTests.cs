using System;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.DAL.Repository;
using NewHabr.Domain;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;
using NewHabr.Domain.ServiceModels;
using Xunit;

namespace UnitTests.Repository;

public class ArticleRepositoryTests
{
    private ApplicationContext _context;
    private IArticleRepository _sut;


    public ArticleRepositoryTests()
    {
        _context = Helper.InitContext();

        _sut = new ArticleRepository(_context);
    }


    [Fact]
    public async Task Create_SyccessfullyCreates()
    {
        // Arrange
        var article = Helper.CreateArticle();

        // Act
        _sut.Create(article);
        var c = await _context.SaveChangesAsync();

        // Assert
        var articles = _context.Set<Article>().ToList();
        Assert.NotNull(articles);
        Assert.Single<Article>(articles);
        Assert.Collection<Article>(articles, a => Assert.True(a.Id != Guid.Empty));
    }

    [Fact]
    public async Task GetPublishedAsync_ReturnsPublishedOnly()
    {
        // Arrange
        var article = Helper.CreateArticle();
        _context.Set<Article>().Add(article);

        article = Helper.CreateArticle();
        article.Published = true;
        article.PublishedAt = DateTimeOffset.UtcNow;
        _context.Set<Article>().Add(article);
        await _context.SaveChangesAsync();

        // Act
        var published = await _sut.GetPublishedAsync(Guid.Empty, false, new ArticleQueryParameters(), default);

        // Assert
        Assert.NotNull(published);
        Assert.NotEmpty(published);
        Assert.IsType<PagedList<ArticleModel>>(published);
        Assert.All<ArticleModel>(published, a => Assert.True(a.Published == true && a.Id.Equals(article.Id)));
    }

    [Fact]
    public async Task GetPublishedAsync_ReturnsNotDeletedOnly()
    {
        // Arrange
        var article = Helper.CreateArticle();
        article.Published = true;
        article.PublishedAt = DateTimeOffset.UtcNow;
        article.Deleted = true;
        article.DeletedAt = DateTimeOffset.UtcNow;
        _context.Set<Article>().Add(article);

        article = Helper.CreateArticle();
        article.Published = true;
        article.PublishedAt = DateTimeOffset.UtcNow;
        _context.Set<Article>().Add(article);
        await _context.SaveChangesAsync();

        // Act
        var published = await _sut.GetPublishedAsync(Guid.Empty, false, new ArticleQueryParameters(), default);

        // Assert
        Assert.NotNull(published);
        Assert.NotEmpty(published);
        Assert.IsType<PagedList<ArticleModel>>(published);
        Assert.All<ArticleModel>(published, a => Assert.True(a.Deleted == false && a.Id.Equals(article.Id)));
    }
}

