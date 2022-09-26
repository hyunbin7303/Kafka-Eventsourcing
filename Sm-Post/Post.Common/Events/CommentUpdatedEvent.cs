using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentUpdatedEvent : BaseEvent
    {
        public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
        {

        }
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public DateTime EditDate { get; set; }
    }
    public class CommentRemovedEvent : BaseEvent
    {
        public CommentRemovedEvent() : base(nameof(CommentRemovedEvent))
        {

        }
        public Guid CommentId { get; set; }
    }
}