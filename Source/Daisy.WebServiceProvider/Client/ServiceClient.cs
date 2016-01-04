using System;
using System.Collections.Generic;

using Daisy.Contracts;
using Daisy.Contracts.Article;
using Daisy.Contracts.Comment;

using Nelibur.ServiceModel.Clients;


namespace Daisy.WebServiceProvider.Client
{
    public sealed class ServiceClient : IServiceClient
    {
        public List<ArticleInfo> GetAllArticles()
        {
            var client = new JsonServiceClient(SettingsProvider.GetServiceUrl());
            var request = new GetAllArticlesRequest();
            return client.Get<List<ArticleInfo>>(request);
        }


        public ArticleInfo GetArticleById(Guid id)
        {
            try
            {
                var client = new JsonServiceClient(SettingsProvider.GetServiceUrl());
                var request = new GetArticleRequest { Id = id };
                return client.Get<ArticleInfo>(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void RemoveArticle(Guid id)
        {
            var client = new JsonServiceClient(SettingsProvider.GetServiceUrl());
            var request = new DeleteArticleRequest { Id = id };
            client.Delete(request);
        }


        public void RemoveComment(Guid id)
        {
            var client = new JsonServiceClient(SettingsProvider.GetServiceUrl());
            var request = new DeleteCommentRequest { Id = id };
            client.Delete(request);
        }


        public Guid SaveArticle(ArticleInfo article)
        {
            var client = new JsonServiceClient(SettingsProvider.GetServiceUrl());
            var createRequest = new CreateArticleRequest
            {
                Title = article.Title,
                CreateDate = article.CreateDate.Value,
                Text = article.Text
            };
            var result = client.Post<ArticleInfo>(createRequest);
            return result.Id.Value;
        }


        public Guid SaveComment(CommentInfo comment)
        {
            var client = new JsonServiceClient(SettingsProvider.GetServiceUrl());
            var createRequest = new CreateCommentRequest
            {
                ArticleId = comment.ArticleId,
                CreateDate = comment.CreateDate.Value,
                Text = comment.Text
            };
            var result = client.Post<ArticleInfo>(createRequest);
            return result.Id.Value;
        }
    }
}
