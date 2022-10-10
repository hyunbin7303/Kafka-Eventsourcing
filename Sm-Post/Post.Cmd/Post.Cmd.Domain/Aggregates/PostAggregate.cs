using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();
        public bool Active {
            get => _active; set => _active = value;   
        }

        public PostAggregate()
        {
            
        }
        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now
            });
        }
        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }
        public void EditMessage(string msg)
        {
            if(!_active){
                throw new InvalidOperationException("You cannot edit the message of an Inactive post!");
            }
            if(string.IsNullOrWhiteSpace(msg))
            {
                throw new InvalidOperationException($"The value of {nameof(msg)} cannot be null or empty. Please provide a valid {nameof(msg)}");
            }

            RaiseEvent(new MessageUpdatedEvent{

                Id = _id,
                Message = msg
            });
        }
        public void Apply(MessageUpdatedEvent @event){
            _id = @event.Id;
        }
        public void LikePost()
        {
            if(_active)
            {
                throw new InvalidOperationException("You cabnnot like an Inactive post!");
            }
            RaiseEvent(new PostLikedEvent{
                Id = _id
            });
        }
        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }
    }
}