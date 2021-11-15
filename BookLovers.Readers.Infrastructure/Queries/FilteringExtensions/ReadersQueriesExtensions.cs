using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Queries.FilteringExtensions
{
    internal static class ReadersQueriesExtensions
    {
        internal static IQueryable<FollowReadModel> FilterFollowersByUserName(
            this IQueryable<FollowReadModel> query,
            string phrase)
        {
            return query.WhereIf(
                p => p.Follower.Status == AggregateStatus.Active.Value &&
                     p.Follower.UserName.ToUpper().StartsWith(phrase.Trim().ToUpper()),
                !phrase.IsEmpty());
        }

        internal static IQueryable<FollowReadModel> FilterFollowingsByUserName(
            this IQueryable<FollowReadModel> query,
            string phrase)
        {
            return query.WhereIf(
                p => p.Followed.Status == AggregateStatus.Active.Value &&
                     p.Followed.UserName.ToUpper().StartsWith(phrase.Trim().ToUpper()),
                !phrase.IsEmpty());
        }

        internal static IQueryable<ProfileFavouriteReadModel> WithReader(
            this IQueryable<ProfileFavouriteReadModel> query,
            int readerId)
        {
            return query.Where(p => p.ReaderId == readerId);
        }

        internal static IQueryable<ProfileFavouriteReadModel> OnlyFavouriteBooks(
            this IQueryable<ProfileFavouriteReadModel> query)
        {
            return query.Where(p => p.FavouriteType == FavouriteType.FavouriteBook.Value);
        }

        internal static IQueryable<ProfileFavouriteReadModel> OnlyFavouriteAuthors(
            this IQueryable<ProfileFavouriteReadModel> query)
        {
            return query.Where(p => p.FavouriteType == FavouriteType.FavouriteAuthor.Value);
        }

        internal static IOrderedQueryable<ReaderReadModel> OrderByUserName(
            this IQueryable<ReaderReadModel> query,
            bool reverse = false)
        {
            if (!reverse)
                return query.OrderBy(p => p.UserName);

            return query.OrderByDescending(p => p.UserName);
        }

        internal static IQueryable<ReaderReadModel> UserNameStartsWith(
            this IQueryable<ReaderReadModel> query,
            string phrase)
        {
            return query.WhereIf(p => p.UserName.ToUpper().StartsWith(phrase.ToUpper()), !phrase.IsEmpty());
        }
    }
}