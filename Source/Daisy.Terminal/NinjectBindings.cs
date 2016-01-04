using System;

using Daisy.BusinessLogic.Services;
using Daisy.Terminal.Log;
using Daisy.WebServiceProvider.Client;

using Ninject.Modules;


namespace Daisy.Terminal
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<FileLogger>();
            Bind<IArticleService>().To<ArticleService>();
            Bind<ICommentService>().To<CommentService>();
            Bind<IServiceClient>().To<ServiceClient>();
        }
    }
}
