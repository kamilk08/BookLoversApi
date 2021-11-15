using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Services.SqlExtensions
{
    internal static class BookIdsParameter
    {
        private const string Name = "BOOK_IDS";

        internal static SqlHelper AddBookIdsParameter(this SqlHelper helper, List<int> bookIds)
        {
            var parameter = new SqlParameter(
                "BOOK_IDS",
                bookIds.Count == 0 ? DBNull.Value : (object) bookIds.ToStringValueList());

            helper.AddParameter(parameter);

            return helper;
        }
    }
}