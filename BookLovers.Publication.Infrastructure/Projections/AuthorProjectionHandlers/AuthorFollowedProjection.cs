using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorFollowedProjection : IProjectionHandler<AuthorFollowed>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorFollowedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorFollowed @event)
        {
            var authorQuery = this._context.Authors
                .Include(p => p.AuthorFollowers)
                .Where(p => p.Guid == @event.AggregateGuid)
                .FutureValue();

            var readerQuery = this._context.Readers
                .Where(p => p.ReaderGuid == @event.FollowedBy)
                .FutureValue();

            var author = authorQuery.Value;
            var reader = readerQuery.Value;

            var follower = new AuthorFollowerReadModel()
            {
                Author = author,
                FollowedById = reader.Id
            };

            author.AuthorFollowers.Add(follower);
            this._context.AuthorFollowers.Add(follower);

            this._context.SaveChanges();
        }
    }
}