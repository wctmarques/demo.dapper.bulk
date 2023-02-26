using Dapper;
using Demo.Dapper.Bulk.Models.Context;
using Demo.Dapper.Bulk.Models.Data.Extensions;
using Demo.Dapper.Bulk.Models.Data.Interfaces;
using Demo.Dapper.Bulk.Models.Entity;

namespace Demo.Dapper.Bulk.Models.Data
{
    public class SampleRepository : RepositoryBase<Sample, int>, ISampleRepository
    {
        #region script create table DEMO_SAMPLE

        //CREATE TABLE DEMO_SAMPLE
        //(
        //     ID_DEMO_SAMPLE INT          NOT NULL IDENTITY(1,1)
        //    ,NAME           VARCHAR(50)  NOT NULL
        //    ,DESCRIPTION    VARCHAR(100) NULL
        //)
        //GO

        #endregion

        public SampleRepository(DefaultContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> BulkInsertAsync(IEnumerable<Sample> samples)
        {
            var tempTableName = $"[#TEMP_DEMO_SAMPLE_{Guid.NewGuid().ToString()}]";

            var sqlCreateTemp =
                $"SELECT * INTO {tempTableName} FROM DEMO_SAMPLE WITH(NOLOCK) WHERE 1=2";

            var sqlInsertDestinationTable =
                "INSERT INTO DEMO_SAMPLE (NAME,DESCRIPTION) " +
                $"SELECT NAME, DESCRIPTION FROM {tempTableName} WITH(NOLOCK) ORDER BY ID_DEMO_SAMPLE";

            return 
                await _connection
                .SqlBulkInsertCustomAsync<Sample>(tempTableName, sqlCreateTemp, sqlInsertDestinationTable, samples)
                .ConfigureAwait(false);

        }

        public async Task<int> ExecuteInsertsAsync(IEnumerable<Sample> samples)
        {
            var sql = $"INSERT INTO DEMO_SAMPLE (NAME, DESCRIPTION) VALUES(@Name, @Description)";

            return
                await _connection
                .ExecuteAsync(sql, samples).ConfigureAwait(false);
        }

        public async Task<int> InsertAsync(Sample sample)
        {
            var sql = $"INSERT INTO DEMO_SAMPLE (NAME, DESCRIPTION) VALUES(@Name, @Description)" +
                $" SELECT SCOPE_IDENTITY()";

            return
                await _connection
                .ExecuteScalarAsync<int>(sql, new
                {
                    Name = sample.Name,
                    Description = sample.Description,
                })
                .ConfigureAwait(false);
        }
    }
}
