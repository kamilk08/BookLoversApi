using System.Data.SqlClient;

namespace BookLovers.Base.Infrastructure.Queries.SqlExtensionsParameter
{
    public static class RowCountParameter
    {
        private const string Name = "ROW_COUNT";

        public static SqlHelper AddRowCount(this SqlHelper helper, int count)
        {
            helper.AddParameter(new SqlParameter("ROW_COUNT", count));

            return helper;
        }
    }
}