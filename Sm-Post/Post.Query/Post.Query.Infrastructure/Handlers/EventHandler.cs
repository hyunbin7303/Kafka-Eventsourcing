using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public EventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task On(PostCreatedEvent @event)
        {
            var post = new PostEntity
            {
                PostId = @event.Id,
                Author = @event.Author,
                DatePosted = @event.DatePosted,
                Message = @event.Message
            };
            await _postRepository.CreateAsync(post);
        }

        public async Task On(MessageUpdatedEvent @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);
            if (post == null) return;

            post.Message = @event.Message;
            await _postRepository.UpdateAsync(post);

        }

        public async Task On(PostLikedEvent @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);
            if (post == null) return;
            post.Likes++;
            await _postRepository.UpdateAsync(post);
        }

        public async Task On(CommentAddedEvent @event)
        {
            var cmt = new CommentEntity
            {
                PostId = @event.Id,
                CommentId = @event.CommentId,
                CommentDate = @event.CommentDate,
                Comment = @event.Comment,
                Username = @event.Username,
                Edited = false
            };

            await _commentRepository.CreateAsync(cmt);
        }

        public async Task On(CommentUpdatedEvent @event)
        {
            var cmt = await _commentRepository.GetByIdAsync(@event.CommentId);
            if (cmt == null) return;

            cmt.Comment = @event.Comment;
            cmt.Edited = true;
            cmt.CommentDate = @event.EditDate;
            await _commentRepository.UpdateAsync(cmt);
        }

        public async Task On(CommentRemovedEvent @event)
        {
            await _commentRepository.DeleteAsync(@event.CommentId);
        }

        public async Task On(PostRemovedEvent @event)
        {
            await _postRepository.DeleteAsync(@event.Id);
        }
    }
}
