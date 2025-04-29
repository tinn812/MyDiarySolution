using Microsoft.AspNetCore.Mvc;
using DiaryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiariesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DiariesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var diaries = await _context.Diaries
                .Include(d => d.DiaryTags)
                .ThenInclude(dt => dt.Tag)
                .ToListAsync();

            return Ok(diaries);
        }
    }
}
