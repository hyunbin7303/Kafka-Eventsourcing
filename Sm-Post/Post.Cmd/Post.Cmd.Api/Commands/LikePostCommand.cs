using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class LikePostCommand : BaseCommand
    {
        public string Comment {get;set; }
        public string Username {get; set;}
    }

    public class EditCommentCommand : BaseCommand
    {

        public Guid CommentId {get; set;}
        public string Comment { get; set; }
        public string Username { get; set; }   
    }
}