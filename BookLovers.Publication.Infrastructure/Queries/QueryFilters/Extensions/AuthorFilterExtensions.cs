using System.Linq;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;

namespace BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions
{
    internal static class AuthorFilterExtensions
    {
        internal static IQueryable<AuthorReadModel> WithAuthorFullName(
            this IQueryable<AuthorReadModel> query,
            string authorName)
        {
            return query.Where(p =>
                p.FullName.ToUpper().StartsWith(authorName.ToUpper()) ||
                (p.SecondName.ToUpper().StartsWith(authorName.ToUpper()) ||
                 p.FirstName.ToUpper().StartsWith(authorName.ToUpper())));
        }
    }
}