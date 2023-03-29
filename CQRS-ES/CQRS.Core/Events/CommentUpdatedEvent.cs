using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Events
{
    public class CommentUpdatedEvent : BaseEvent
    {
        public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
        {
        }

        public Guid CommentId { get; set; }
        public string Comment { get; set; } = default!;
        public string Username { get; set; } = default!;
        public DateTime DateUpdate { get; set; } 
    }
}
