using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using QueryableProcessor.Data.Context;

using QueryBlaze.Processor;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueryableProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly TestDbContext _context;
        private readonly ISortQueryProcessor _processor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, TestDbContext context, ISortQueryProcessor processor)
        {
            _logger = logger;
            _context = context;
            _processor = processor;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestData([FromQuery] List<string> sort)
        {
            await _context.Database.EnsureCreatedAsync();
            var query = _context.People.AsQueryable();
            
            var sortParams = new SortParams();
            sortParams.SortProperties.AddRange(sort);

            var processed = _processor.ApplySorting(query, sortParams);

            return Ok(await processed.ToListAsync());
        }
    }
}
