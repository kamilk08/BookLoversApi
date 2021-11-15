using System.Threading.Tasks;
using BookLovers.Auth.Events;
using BookLovers.Base.Infrastructure.Events.DomainEvents;

namespace BookLovers.Auth.Application.Events
{
    internal class UserPromotedToLibrarianEventHandler :
        IDomainEventHandler<UserPromotedToLibrarianEvent>
    {
        public Task HandleAsync(UserPromotedToLibrarianEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}