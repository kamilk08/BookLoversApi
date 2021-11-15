using System.Data.SqlClient;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Services.SqlExtensions
{
    internal static class BookcaseIdParameter
    {
        private const string Name = "BOOKCASE_ID";

        internal static SqlHelper AddBookcaseIdParameter(this SqlHelper helper, int bookcaseId)
        {
            helper.AddParameter(new SqlParameter("BOOKCASE_ID", bookcaseId));

            return helper;
        }
    }
}