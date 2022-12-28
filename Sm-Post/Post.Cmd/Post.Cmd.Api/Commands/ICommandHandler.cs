namespace Post.Cmd.Api.Commands
{
    public interface ICommandHandler
    {
        Task HandlerAsync(NewPostCommand cmd);
        Task HandlerAsync(EditMessageCommand cmd);
        Task HandlerAsync(LikePostCommand cmd);
        Task HandlerAsync(AddCommentCommand cmd);
        Task HandlerAsync(EditCommentCommand cmd);
        Task HandlerAsync(RemoveCommentCommand cmd);
        Task HandlerAsync(DeletePostCommand cmd);

    }
}