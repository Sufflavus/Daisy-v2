using System;
using System.Net;
using System.Reflection;
using System.ServiceModel.Web;

using Daisy.Contracts;
using Daisy.Contracts.Article;
using Daisy.Contracts.Error;
using Daisy.Dal.Domain;
using Daisy.Dal.Repository.Interfaces;
using Daisy.WebService.Log;

using Nelibur.ObjectMapper;
using Nelibur.ServiceModel.Services.Operations;

using Ninject;


namespace Daisy.WebService.Processors
{
    public class ArticleProcessor : IPost<CreateArticleRequest>,
        IDeleteOneWay<DeleteArticleRequest>,
        IGet<GetArticleRequest>,
        IGet<GetAllArticlesRequest>
    {
        private IArticleRepository _articleRepository;
        private ILogger _logger;


        public ArticleProcessor()
        {
            BuildDependencies();
            InitMapper();
        }


        public void DeleteOneWay(DeleteArticleRequest request)
        {
            try
            {
                _articleRepository.Remove(request.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new WebFaultException<FaultInfo>(CreateFaultInfo(ex.Message), HttpStatusCode.BadRequest);
            }
        }


        public object Get(GetAllArticlesRequest request)
        {
            try
            {
                return _articleRepository.GetAll()
                    .ConvertAll(x => TinyMapper.Map<ArticleInfo>(x));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new WebFaultException<FaultInfo>(CreateFaultInfo(ex.Message), HttpStatusCode.BadRequest);
            }
        }


        public object Get(GetArticleRequest request)
        {
            ArticleEntity entity;

            try
            {
                entity = _articleRepository.GetById(request.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new WebFaultException<FaultInfo>(CreateFaultInfo(ex.Message), HttpStatusCode.BadRequest);
            }

            if (entity == null)
            {
                return null;
            }

            return TinyMapper.Map<ArticleInfo>(entity);
        }


        public object Post(CreateArticleRequest request)
        {
            var entity = TinyMapper.Map<ArticleEntity>(request);

            try
            {
                _articleRepository.AddOrUpdate(entity);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex);

                throw new WebFaultException<ArgumentFaultInfo>(CreateArgumentFaultInfo(ex.Message, ex.ParamName),
                    HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new WebFaultException<FaultInfo>(CreateFaultInfo(ex.Message), HttpStatusCode.BadRequest);
            }

            var response = TinyMapper.Map<ArticleInfo>(entity);
            return response;
        }


        private void BuildDependencies()
        {
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            _articleRepository = kernel.Get<IArticleRepository>();
            _logger = kernel.Get<ILogger>();
        }


        private ArgumentFaultInfo CreateArgumentFaultInfo(string errorMessage, string paramName)
        {
            return new ArgumentFaultInfo
            {
                ErrorMessage = errorMessage,
                ParamName = paramName
            };
        }


        private FaultInfo CreateFaultInfo(string errorMessage)
        {
            return new FaultInfo
            {
                ErrorMessage = errorMessage
            };
        }


        private void InitMapper()
        {
            TinyMapper.Bind<ArticleEntity, CreateArticleRequest>();
            TinyMapper.Bind<ArticleEntity, ArticleInfo>();
            TinyMapper.Bind<CommentEntity, CommentInfo>();
        }
    }
}
