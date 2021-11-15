using System.Data.SqlClient;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices.SqlExtensions
{
    internal static class AuthorIdParameter
    {
        internal static SqlHelper AddAuthorId(this SqlHelper helper, int authorId)
        {
            var sqlParameter = new SqlParameter("AUTHOR_ID", authorId);

            return helper;
        }
    }
}