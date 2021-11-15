using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorUnFollowedProjection :
        IProjectionHandler<AuthorUnFollowed>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorUnFollowedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorUnFollowed @event)
        {
            var authorQuery = this._context.Authors.Include(p => p.AuthorFollowers)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();
            var readerQuery = this._context.Readers
                .Where(p => p.ReaderGuid == @event.FollowedBy).FutureValue();

            var author = authorQuery.Value;
            var reader = readerQuery.Value;

            var follower = author.AuthorFollowers
                .SingleOrDefault(p => p.FollowedBy == reader);

            author.AuthorFollowers.Remove(follower);

            this._context.AuthorFollowers.Remove(follower);

            this._context.Readers.AddOrUpdate(p => p.Id, reader);

            this._context.SaveChanges();
        }
    }
}