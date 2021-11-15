using System.Data.SqlClient;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices.SqlExtensions
{
    internal static class PublisherIdParameter
    {
        private const string Name = "PUBLISHER_ID";

        internal static SqlHelper AddPublisherId(this SqlHelper helper, int publisherId)
        {
            helper.AddParameter(new SqlParameter("PUBLISHER_ID", publisherId));
            return helper;
        }
    }
}