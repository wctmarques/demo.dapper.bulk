using Demo.Dapper.Bulk.Models.Entity;

namespace Demo.Dapper.Bulk.Models.Data.Interfaces
{
    public interface ISampleRepository
    {
        Task<int> InsertAsync(Sample sample);
        Task<int> ExecuteInsertsAsync(IEnumerable<Sample> samples);
        Task<int> BulkInsertAsync(IEnumerable<Sample> samples);
    }
}
