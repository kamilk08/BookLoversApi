using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;

namespace BookLovers.Base.Infrastructure.Queries
{
    public class SqlHelper
    {
        private readonly List<SqlParameter> _sqlParameters;
        internal readonly DbContext Context;

        private SqlHelper()
        {
        }

        protected SqlHelper(DbContext context)
        {
            Context = context;
            _sqlParameters = new List<SqlParameter>();
        }

        public static SqlHelper Initialize(DbContext context) =>
            new SqlHelper(context);

        public SqlHelper AddParameter(SqlParameter parameter)
        {
            _sqlParameters.Add(parameter);
            return this;
        }

        internal SqlParameter[] GetParameters() => _sqlParameters.ToArray();
    }
}