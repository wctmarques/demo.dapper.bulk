using Demo.Dapper.Bulk.Models.Data.Interfaces;
using Demo.Dapper.Bulk.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Dapper.Bulk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkInsertController : ControllerBase
    {
        private readonly ISampleRepository _sampleRepository;

        public BulkInsertController(ISampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
        }

        private IEnumerable<Sample> SampleDataGenerator(int itemQuantity)
        {
            var samples = new List<Sample>();

            for (int i = 1; i <= itemQuantity; i++)
            {
                samples.Add(new Sample()  
                {
                    Name = $"Sample {i}",
                    Description = $"Description Sample {i}",
                });
            }

            return samples;
        }

        [HttpPost("ForeachDapperInsert")]
        public async Task<IActionResult> ForeachDapperInsertPost([FromQuery] int rowsQuantity)
        {
            IEnumerable<Sample> samples = SampleDataGenerator(rowsQuantity);

            var start = DateTime.UtcNow;
            int rowsAffected = 0;

            foreach(var entity in samples)
            {
                await _sampleRepository
                    .InsertAsync(entity)
                    .ConfigureAwait(false);
                
                rowsAffected++;
            }

            return
                Ok(new
                {
                    RowsAffected = rowsAffected,
                    Duration = (DateTime.UtcNow - start).ToString(@"hh\:mm\:ss"),
                });
        }

        [HttpPost("DapperExecuteInserts")]
        public async Task<IActionResult> ExecuteInsertsPost([FromQuery]  int rowsQuantity)
        {
            IEnumerable<Sample> samples = SampleDataGenerator(rowsQuantity);

            var start = DateTime.UtcNow;

            return
                Ok(new
                {
                    RowsAffected = 
                    await
                    _sampleRepository
                    .ExecuteInsertsAsync(samples)
                    .ConfigureAwait(false),
                    Duration = (DateTime.UtcNow - start).ToString(@"hh\:mm\:ss"),
                });
        }

        [HttpPost("SqlBulkCopy")]
        public async Task<IActionResult> BulkCopyPost([FromQuery] int rowsQuantity)
        {
            IEnumerable<Sample> samples = SampleDataGenerator(rowsQuantity);

            var start = DateTime.UtcNow;

            var rowsAffected = 
                await _sampleRepository
                .BulkInsertAsync(samples)
                .ConfigureAwait(false);

            return
                Ok(new
                {
                    RowsAffected = rowsAffected,
                    Duration = (DateTime.UtcNow - start).ToString(@"hh\:mm\:ss"),
                });
        }
    }
}
