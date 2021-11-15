using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Readers
{
    internal class ReaderUnFollowedProjection :
        IProjectionHandler<ReaderUnFollowed>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReaderUnFollowedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ReaderUnFollowed @event)
        {
            var firstReaderQuery = this._context.Readers.Include(p => p.Followers)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();
            var followerQuery = this._context.Readers.Where(p => p.Guid == @event.UnFollowedByGuid).FutureValue();

            var firstReader = firstReaderQuery.Value;
            var follower = followerQuery.Value;

            var entity = firstReader.Followers.Where(p => p.Follower != null)
                .Single(p => p.Follower.ReaderId == follower.ReaderId);

            firstReader.Followers.Remove(entity);

            this._context.FollowObjects.Remove(entity);

            this._context.Readers.AddOrUpdate(p => p.Id, firstReader);

            this._context.SaveChanges();
        }
    }
}