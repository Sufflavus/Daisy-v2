﻿using System;

using Daisy.Dal.Context;
using Daisy.Dal.Context.Interfaces;
using Daisy.Dal.Repository;
using Daisy.Dal.Repository.Interfaces;
using Daisy.WebService.Log;

using Ninject.Modules;


namespace Daisy.WebService
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<FileLogger>();
            Bind<IArticleRepository>().To<ArticleRepository>();
            Bind<ICommentRepository>().To<CommentRepository>();
            Bind<IContext>().To<NHibernateContext>();
            Bind<IConnectionFactory>().To<ConnectionFactory>();
        }
    }
}
