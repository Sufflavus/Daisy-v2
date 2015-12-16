using System;


namespace Daisy.Contracts.Article
{
    public sealed class CreateArticleRequest
    {
        public DateTime CreateDate { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
    }
}
