using System;


namespace Daisy.Contracts.Comment
{
    public sealed class CreateCommentRequest
    {
        public Guid ArticleId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Text { get; set; }
    }
}
