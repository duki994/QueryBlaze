using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using QueryableProcessor.Data.Context;

using QueryBlaze.Processor;
using QueryBlaze.Processor.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueryableProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TestDbContext _context;
        private readonly ISortQueryProcessor _processor;

        public TestController(
            TestDbContext context,
            ISortQueryProcessor processor)
        {
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
