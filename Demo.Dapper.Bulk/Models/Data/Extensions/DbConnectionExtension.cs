using System.Data.SqlClient;
using System.Data;

namespace Demo.Dapper.Bulk.Models.Data.Extensions
{
    public static class DbConnectionExtension
    {
        public static async Task<int> SqlBulkInsertCustomAsync<T>(
            this IDbConnection connection,
            string temporaryTableName,
            string scriptCreateTemporaryTable,
            string scriptInsertDestinationTable,
            IEnumerable<T> entities,
            int batchSize = 1000)
        {
            int rowsAffected = 0;

            using (var sqlConnection = (SqlConnection)connection)
            {
                await sqlConnection.OpenAsync().ConfigureAwait(false);

                using (var command = new SqlCommand(scriptCreateTemporaryTable, sqlConnection))
                {
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                using (var bulkCopy = new SqlBulkCopy(sqlConnection))
                {
                    bulkCopy.BatchSize = batchSize;
                    bulkCopy.DestinationTableName = temporaryTableName;
                    bulkCopy.EnableStreaming = true;

                    using (var dataTable = entities.ToDataTable())
                    {
                        await bulkCopy.WriteToServerAsync(dataTable);
                    }
                }

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    using (var command = new SqlCommand(scriptInsertDestinationTable, sqlConnection, transaction))
                    {
                        rowsAffected =
                            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }

                    transaction.Commit();
                }

            }

            return rowsAffected;
        }
    }
}
