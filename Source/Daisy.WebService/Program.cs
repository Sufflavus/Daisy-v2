using System;
using System.ServiceModel.Web;

using Daisy.Contracts.Article;
using Daisy.Contracts.Comment;
using Daisy.WebService.Processors;

using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Default;


namespace Daisy.WebService
{
    internal class Program
    {
        private static WebServiceHost _service;


        private static void ConfigureService()
        {
            NeliburRestService.Configure(x => x.Bind<CreateArticleRequest, ArticleProcessor>());
            NeliburRestService.Configure(x => x.Bind<DeleteArticleRequest, ArticleProcessor>());
            NeliburRestService.Configure(x => x.Bind<GetArticleRequest, ArticleProcessor>());
            NeliburRestService.Configure(x => x.Bind<GetAllArticlesRequest, ArticleProcessor>());

            NeliburRestService.Configure(x => x.Bind<CreateCommentRequest, CommentProcessor>());
            NeliburRestService.Configure(x => x.Bind<DeleteCommentRequest, CommentProcessor>());
        }


        private static void Main(string[] args)
        {
            ConfigureService();

            _service = new WebServiceHost(typeof(JsonServicePerCall));
            _service.Open();

            Console.WriteLine("MailService is running");
            Console.WriteLine("Press any key to exit\n");

            Console.ReadKey();
            _service.Close();
        }
    }
}
