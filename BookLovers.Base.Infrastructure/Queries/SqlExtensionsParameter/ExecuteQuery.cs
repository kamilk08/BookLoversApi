using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Queries.SqlExtensionsParameter
{
    public static class ExecuteQuery
    {
        public static Task<T> ExecuteAndGetValueAsync<T>(this SqlHelper helper, string sql)
        {
            return helper.Context.Database
                .SqlQuery<T>(sql, helper.GetParameters())
                .SingleOrDefaultAsync();
        }

        public static Task<List<T>> ExecuteAndGetValuesAsync<T>(
            this SqlHelper helper,
            string sql)
        {
            return helper.Context.Database
                .SqlQuery<T>(sql, helper.GetParameters())
                .ToListAsync();
        }
    }
}