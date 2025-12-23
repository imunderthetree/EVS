using System.Data;
using Microsoft.Data.SqlClient;

namespace EVS.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected async Task<DataTable> ExecuteQueryAsync(string query, Dictionary<string, object>? parameters = null)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            using var adapter = new SqlDataAdapter(command);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        protected async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object>? parameters = null)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            return await command.ExecuteNonQueryAsync();
        }

        protected async Task<object?> ExecuteScalarAsync(string query, Dictionary<string, object>? parameters = null)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            using var command = GetCommand(query, connection);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            return await command.ExecuteScalarAsync();
        }

        private static SqlCommand GetCommand(string query, SqlConnection connection)
        {
            return new SqlCommand(query, connection);
        }
    }
}