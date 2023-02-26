using Demo.Dapper.Bulk.Models.Context;
using System.Data;

namespace Demo.Dapper.Bulk.Models.Data
{
    public class RepositoryBase<TEntity, TKey> : IDisposable
    {
        protected readonly IDbConnection _connection;
        protected readonly string _tableName;

        protected RepositoryBase(DefaultContext dbContext)
        {
            _connection = dbContext.CreateConnection();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _connection
                .Close();

            _connection
                .Dispose();
        }

    }
}
