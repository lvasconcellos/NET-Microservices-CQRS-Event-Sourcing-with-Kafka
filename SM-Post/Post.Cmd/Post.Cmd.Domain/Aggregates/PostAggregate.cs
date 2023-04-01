using CQRS.Core.Domain;
using Post.Cmd.Domain.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author = default!;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active { get { return _active; } set { _active = value; } }

        public PostAggregate() { }
        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now,
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            CheckIfPostIsInactive(action: "edit the message");
            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}");
            }

            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _id,
                Message = message
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;

        }
        public void LikePost()
        {
            CheckIfPostIsInactive(action: "like");

            RaiseEvent(new PostLikedEvent
            {
                Id = _id
            });
        }
        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;

        }
        public void AddComment(string comment, string username)
        {
            CheckIfPostIsInactive(action: "add comment to");

            if (string.IsNullOrEmpty(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}");
            }

            RaiseEvent(new CommentAddedEvent
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.UtcNow
            });
        }
        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.Id, new Tuple<string, string>(@event.Comment, @event.Username));
        }
        public void EditComment(Guid commentId, string comment, string username)
        {
            CheckIfPostIsInactive(action: "edit the comment of");
            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment made by another user!");
            }
            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditDate = DateTime.UtcNow
            });
        }
        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }
        public void RemoveComment(Guid commentId, string username)
        {
            CheckIfPostIsInactive(action: "remove a comment of");
            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment made by another user!");
            }
            RaiseEvent(new CommentRemovedEvent
            {
                Id = _id,
                CommentId = commentId,
            });
        }
        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }
        public void DeletePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Post already deleted");
            }
            if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a post made by another user!");
            }

            RaiseEvent(new PostRemovedEvent
            {
                Id = _id
            });
        }
        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }

        private void CheckIfPostIsInactive(string action)
        {
            if (!_active)
            {
                throw new InvalidOperationException($"You cannot {action} an inactive post!");
            }
        }


    }
}
