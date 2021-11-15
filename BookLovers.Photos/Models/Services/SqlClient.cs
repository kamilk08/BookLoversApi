using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace BookLovers.Photos.Models.Services
{
    public class SqlClient : IDisposable
    {
        private readonly DbConnection _connection;

        public SqlClient(string connectionString)
        {
            this._connection = new SqlConnection(connectionString);
        }

        public Task<T> QueryAsync<T>(string query, DynamicParameters parameters)
        {
            return this._connection.QueryFirstOrDefaultAsync<T>(query, parameters);
        }

        public Task<T> QueryAsync<T>(CommandDefinition commandDefinition)
        {
            return this._connection.QuerySingleOrDefaultAsync<T>(commandDefinition);
        }

        public void OpenConnection()
        {
            this._connection.Open();
        }

        public void CloseConnection()
        {
            this._connection.Close();
        }

        public void Dispose()
        {
            this._connection?.Dispose();
        }
    }
}