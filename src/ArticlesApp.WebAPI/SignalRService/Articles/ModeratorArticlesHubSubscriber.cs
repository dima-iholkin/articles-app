﻿using ArticlesApp._Utilities._Helpers;
using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;
using ArticlesApp.WebAPI.ArticlesService;
using ArticlesApp.WebAPI.Models;
using Microsoft.AspNetCore.SignalR;



namespace ArticlesApp.WebAPI.SignalRService.Articles;



public class ModeratorArticlesHubSubscriber
{
    private readonly ArticlesConverter_WebAPI _articlesConverter;
    private readonly IHubContext<ModeratorArticlesHub> _articlesHubContext;
    private readonly ILogger<MyArticlesHubSubscriber> _logger;

    public ModeratorArticlesHubSubscriber(
        ArticlesConverter_WebAPI articlesConverter,
        IHubContext<ModeratorArticlesHub> articlesHubContext,
        ILogger<MyArticlesHubSubscriber> logger
    )
    {
        _articlesConverter = articlesConverter;
        _articlesHubContext = articlesHubContext;
        _logger = logger;
    }



    public void OnArticleCreated(
        object? sender,
        ArticleArgs articleArgs
    )
    {
        EventHelpers.FireAndForget(
            () => _OnArticleCreated(sender, articleArgs),
            _logger
        );
    }



    public void OnArticleModeratorDecision(
        object? sender,
        ArticleArgs articleArgs
    )
    {
        EventHelpers.FireAndForget(
            () => _OnArticleModeratorDecision(sender, articleArgs),
            _logger
        );
    }



    public void OnArticleSoftDeleted(
        object? sender,
        ArticleArgs articleArgs
    )
    {
        EventHelpers.FireAndForget(
            () => _OnArticleSoftDeleted(sender, articleArgs),
            _logger
        );
    }



    private async Task _OnArticleCreated(
                    object? sender,
        ArticleArgs articleArgs
    )
    {
        Article article = articleArgs.Article;
        Article_WebAPI article_WebAPI = _articlesConverter.ToEntity<Article_WebAPI>(article);
        ArticlePayload payload = new ArticlePayload(article_WebAPI);

        await _articlesHubContext.Clients.All.SendAsync(
            "PendingArticleCreated",
            payload
        );
    }



    private async Task _OnArticleModeratorDecision(
        object? sender,
        ArticleArgs articleArgs
    )
    {
        Article article = articleArgs.Article;
        Article_WebAPI article_WebAPI = _articlesConverter.ToEntity<Article_WebAPI>(article);
        ArticlePayload payload = new ArticlePayload(article_WebAPI);

        await _articlesHubContext.Clients.All.SendAsync(
            "ArticleModeratorDecision",
            payload
        );
    }



    private async Task _OnArticleSoftDeleted(
        object? sender,
        ArticleArgs articleArgs
    )
    {
        Article article = articleArgs.Article;
        Article_WebAPI article_WebAPI = _articlesConverter.ToEntity<Article_WebAPI>(article);
        ArticlePayload payload = new ArticlePayload(article_WebAPI);

        await _articlesHubContext.Clients.All.SendAsync(
            "ArticleSoftDeleted",
            payload
        );
    }
}