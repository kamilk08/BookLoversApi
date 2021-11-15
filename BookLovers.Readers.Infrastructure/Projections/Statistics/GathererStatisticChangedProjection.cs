using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Domain.Statistics;
using BookLovers.Readers.Events.Statistics;
using BookLovers.Readers.Infrastructure.Persistence;

namespace BookLovers.Readers.Infrastructure.Projections.Statistics
{
    internal class GathererStatisticChangedProjection :
        IProjectionHandler<GathererStatisticChanged>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public GathererStatisticChangedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(GathererStatisticChanged @event)
        {
            var gatherer = this._context.Statistics.Single(p => p.Guid == @event.AggregateGuid);

            gatherer.ReceivedLikes = @event.Statistics.ElementAt(ReceivedLikes.Position).Value;
            gatherer.GivenLikes = @event.Statistics.ElementAt(GivenLikes.Position).Value;
            gatherer.ShelvesCount = @event.Statistics.ElementAt(CurrentShelves.Position).Value;
            gatherer.ReviewsCount = @event.Statistics.ElementAt(CreatedReviews.Position).Value;

            gatherer.FollowersCount = @event.Statistics.ElementAt(Followers.Position).Value;
            gatherer.FollowingsCount = @event.Statistics.ElementAt(Followings.Position).Value;

            gatherer.BooksCount = @event.Statistics.ElementAt(BooksInBookcase.Position).Value;
            gatherer.AddedBooks = @event.Statistics.ElementAt(AddedBooks.Position).Value;
            gatherer.AddedAuthors = @event.Statistics.ElementAt(AddedAuthors.Position).Value;
            gatherer.AddedQuotes = @event.Statistics.ElementAt(AddedQuotes.Position).Value;

            this._context.Statistics.AddOrUpdate(p => p.Id, gatherer);

            this._context.SaveChanges();
        }
    }
}