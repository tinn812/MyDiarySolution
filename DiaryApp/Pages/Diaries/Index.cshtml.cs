using DiaryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DiaryApp.Pages.Diaries
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public int DiaryCount { get; set; }

        public string SearchKeyword { get; set; }
        [TempData]
        public string? SuccessMessage { get; set; }

        [BindProperty]
        public int DeleteId { get; set; }

        public List<Diary> Diaries { get; set; }

        public async Task OnGetAsync(string searchKeyword)
        {
            SearchKeyword = searchKeyword;

            var query = _context.Diaries
                .Include(d => d.DiaryTags)
                    .ThenInclude(dt => dt.Tag)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(d =>
                    d.Title.Contains(searchKeyword) ||
                    d.Content.Contains(searchKeyword) ||
                    d.DiaryTags.Any(dt => dt.Tag.Name.Contains(searchKeyword))
                );
            }

            Diaries = await query
                .OrderByDescending(d => d.Date)
                .ThenByDescending(d => d.CreatedAt)
                .Include(d => d.DiaryTags)
                .ThenInclude(dt => dt.Tag)
                .ToListAsync();

            DiaryCount = Diaries.Count;
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var diary = await _context.Diaries
                .Include(d => d.DiaryTags)
                .FirstOrDefaultAsync(d => d.Id == DeleteId);

            if (diary == null)
            {
                return NotFound();
            }

            _context.Diaries.Remove(diary);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "刪除成功！";
            return RedirectToPage(); // 回到列表
        }
    }
}
