using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Services.SqlExtensions
{
    internal static class ShelvesIdsParameter
    {
        private const string Name = "SHELVES_IDS";

        internal static SqlHelper AddShelvesIdsParameter(
            this SqlHelper helper,
            List<int> shelvesIds)
        {
            var parameter = new SqlParameter(
                "SHELVES_IDS",
                !shelvesIds.Any() ? DBNull.Value : (object) shelvesIds.ToStringValueList());

            helper.AddParameter(parameter);

            return helper;
        }
    }
}