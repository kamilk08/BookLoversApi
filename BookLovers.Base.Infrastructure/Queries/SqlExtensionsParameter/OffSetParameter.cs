using System.Data.SqlClient;

namespace BookLovers.Base.Infrastructure.Queries.SqlExtensionsParameter
{
    public static class OffSetParameter
    {
        private const string Name = "SKIP";

        public static SqlHelper AddOffSet(this SqlHelper helper, int page, int count)
        {
            helper.AddParameter(new SqlParameter("SKIP", count * page));

            return helper;
        }
    }
}