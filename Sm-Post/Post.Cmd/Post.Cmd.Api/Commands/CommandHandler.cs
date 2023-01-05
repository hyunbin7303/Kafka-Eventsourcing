using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;
        public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }
        public async Task HandlerAsync(NewPostCommand cmd)
        {
            var aggregate = new PostAggregate(cmd.Id, cmd.Author, cmd.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        public async Task HandlerAsync(EditMessageCommand cmd)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(cmd.Id);
            aggregate.EditMessage(cmd.Message);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        public async Task HandlerAsync(LikePostCommand cmd)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(cmd.Id);
            aggregate.LikePost();
            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        public async Task HandlerAsync(AddCommentCommand cmd)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(cmd.Id);
            aggregate.AddComment(cmd.Comment, cmd.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        public async Task HandlerAsync(EditCommentCommand cmd)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(cmd.Id);
            aggregate.EditComment(cmd.CommentId, cmd.Comment, cmd.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(RemoveCommentCommand cmd)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(cmd.Id);
            aggregate.RemoveComment(cmd.CommentId, cmd.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        public async Task HandlerAsync(DeletePostCommand cmd)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(cmd.Id);
            aggregate.DeletePost(cmd.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(RestoreReadDbCommand cmd)
        { 
        }
    }
}