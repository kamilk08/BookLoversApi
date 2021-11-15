using System.Data.SqlClient;

namespace BookLovers.Base.Infrastructure.Queries.SqlExtensionsParameter
{
    public static class TitleParameter
    {
        private const string Name = "TITLE";

        public static SqlHelper AddTitle(this SqlHelper helper, string title)
        {
            helper.AddParameter(new SqlParameter("TITLE", title == null ? string.Empty : (object) title));

            return helper;
        }
    }
}