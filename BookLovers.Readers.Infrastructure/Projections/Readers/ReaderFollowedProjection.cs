using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Readers
{
    internal class ReaderFollowedProjection : IProjectionHandler<ReaderFollowed>, IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReaderFollowedProjection(ReadersContext context) => this._context = context;

        public void Handle(ReaderFollowed @event)
        {
            var firstReaderQuery = this._context.Readers.Include(p => p.Followers)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();
            var secondReaderQuery = this._context.Readers.Include(p => p.Followers)
                .Where(p => p.Guid == @event.FollowedByGuid).FutureValue();

            var firstReader = firstReaderQuery.Value;
            var secondReader = secondReaderQuery.Value;

            var follower = new FollowReadModel()
            {
                Followed = firstReader,
                Follower = secondReader
            };

            firstReader.Followers.Add(follower);

            this._context.Readers.AddOrUpdate(p => p.Id, firstReader);

            this._context.SaveChanges();
        }
    }
}