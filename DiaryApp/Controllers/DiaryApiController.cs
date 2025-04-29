using Microsoft.AspNetCore.Mvc;
using DiaryApp.Models;
using Microsoft.EntityFrameworkCore;
using DiaryApp.Models.Dto;

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
                .Select(d => new DiaryDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Date = d.Date.ToLocalTime(),
                    Tags = d.DiaryTags.Select(dt => dt.Tag.Name).ToList()
                })
                .ToListAsync();

            return Ok(diaries);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var diary = await _context.Diaries
                .Include(d => d.DiaryTags)
                .ThenInclude(dt => dt.Tag)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (diary == null)
                return NotFound();

            var result = new DiaryDetailDto
            {
                Id = diary.Id,
                Title = diary.Title,
                Content = diary.Content,
                Date = diary.Date.ToLocalTime(),
                Tags = diary.DiaryTags.Select(dt => dt.Tag.Name).ToList()
            };

            return Ok(result);
        }




        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDiaryDto dto)
        {
            var diary = new Diary
            {
                Title = dto.Title,
                Content = dto.Content,
                Date = dto.Date.ToUniversalTime(), // 儲存成 UTC
                DiaryTags = new List<DiaryTag>()
            };

            foreach (var tagName in dto.Tags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName)
                        ?? new Tag { Name = tagName };

                diary.DiaryTags.Add(new DiaryTag { Tag = tag });
            }

            _context.Diaries.Add(diary);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = diary.Id }, new DiaryDto
            {
                Id = diary.Id,
                Title = diary.Title,
                Date = diary.Date.ToLocalTime(),
                Tags = diary.DiaryTags.Select(dt => dt.Tag.Name).ToList()
            });

        }




    }
}
