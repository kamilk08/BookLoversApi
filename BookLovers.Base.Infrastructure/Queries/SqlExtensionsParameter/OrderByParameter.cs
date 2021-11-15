using System.Data.SqlClient;

namespace BookLovers.Base.Infrastructure.Queries.SqlExtensionsParameter
{
    public static class OrderByParameter
    {
        private const string Name = "ORDER_BY";

        public static SqlHelper AddSorting(this SqlHelper helper, bool descending)
        {
            helper.AddParameter(new SqlParameter("ORDER_BY", @descending ? 1 : 0));

            return helper;
        }
    }
}