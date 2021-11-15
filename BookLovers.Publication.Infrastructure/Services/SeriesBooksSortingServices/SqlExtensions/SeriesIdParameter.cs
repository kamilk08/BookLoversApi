using System.Data.SqlClient;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices.SqlExtensions
{
    internal static class SeriesIdParameter
    {
        private const string Name = "SERIES_ID";

        internal static SqlHelper AddSeriesId(this SqlHelper helper, int seriesId)
        {
            helper.AddParameter(new SqlParameter("SERIES_ID", seriesId));

            return helper;
        }
    }
}