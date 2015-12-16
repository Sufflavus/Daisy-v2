using System;
using System.Net;
using System.Reflection;
using System.ServiceModel.Web;

using Daisy.Contracts;
using Daisy.Contracts.Article;
using Daisy.Contracts.Comment;
using Daisy.Contracts.Error;
using Daisy.Dal.Domain;
using Daisy.Dal.Repository.Interfaces;
using Daisy.WebService.Log;

using Nelibur.ObjectMapper;
using Nelibur.ServiceModel.Services.Operations;

using Ninject;


namespace Daisy.WebService.Processors
{
    public class CommentProcessor : IPost<CreateCommentRequest>,
        IDeleteOneWay<DeleteCommentRequest>
    {
        private ICommentRepository _commentRepository;
        private ILogger _logger;


        public CommentProcessor()
        {
            BuildDependencies();
            InitMapper();
        }


        public void DeleteOneWay(DeleteCommentRequest request)
        {
            try
            {
                _commentRepository.Remove(request.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new WebFaultException<FaultInfo>(CreateFaultInfo(ex.Message), HttpStatusCode.BadRequest);
            }
        }


        public object Post(CreateCommentRequest request)
        {
            var entity = TinyMapper.Map<CommentEntity>(request);

            try
            {
                _commentRepository.AddOrUpdate(entity);
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

            var response = TinyMapper.Map<CommentInfo>(entity);
            return response;
        }


        private void BuildDependencies()
        {
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            _commentRepository = kernel.Get<ICommentRepository>();
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

            TinyMapper.Bind<CommentEntity, CreateCommentRequest>();
            TinyMapper.Bind<CommentEntity, CommentInfo>();
        }
    }
}
