using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DiaryApp.Models;

namespace DiaryApp.Pages.Diaries
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Diary Diary { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Diary = await _context.Diaries
                .Include(d => d.DiaryTags)
                    .ThenInclude(dt => dt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Diary == null)
                return NotFound();

            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var diary = await _context.Diaries
                .Include(d => d.DiaryTags)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (diary == null)
                return NotFound();

            // 如果有圖片，順便刪除檔案（可選）
            if (!string.IsNullOrEmpty(diary.ImagePath))
            {
                var filePath = Path.Combine("wwwroot", diary.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Diaries.Remove(diary);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "日誌刪除成功 ✅";
            return RedirectToPage("Index");
        }
    }
}
