using DiaryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Pages.Diaries
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DiaryViewModel Diary { get; set; } = new DiaryViewModel();

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var diary = await _context.Diaries
                .Include(d => d.DiaryTags)
                .ThenInclude(dt => dt.Tag)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (diary == null) return NotFound();

            Diary.Id = diary.Id;
            Diary.Title = diary.Title;
            Diary.Date = diary.Date.ToLocalTime();
            Diary.Content = diary.Content;
            Diary.Tags = string.Join(", ", diary.DiaryTags.Select(dt => dt.Tag.Name));
            Diary.ExistingImagePath = diary.ImagePath;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var diaryToUpdate = await _context.Diaries
                .Include(d => d.DiaryTags)
                .ThenInclude(dt => dt.Tag)
                .FirstOrDefaultAsync(d => d.Id == Diary.Id);

            if (diaryToUpdate == null) return NotFound();

            diaryToUpdate.Title = Diary.Title;
            diaryToUpdate.Date = Diary.Date;
            diaryToUpdate.Content = Diary.Content;

            // 刪除圖片
            if (Diary.DeleteImage && !string.IsNullOrEmpty(diaryToUpdate.ImagePath))
            {
                var oldPath = Path.Combine("wwwroot", diaryToUpdate.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);

                diaryToUpdate.ImagePath = null;
            }
            // 上傳新圖片
            if (Upload != null && Upload.Length > 0)
            {
                diaryToUpdate.ImagePath = await SaveImageAsync(Upload);
            }

            // 處理標籤
            await AttachTagsToDiaryAsync(diaryToUpdate, Diary.Tags);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "日誌已成功更新！";
            return RedirectToPage("Index");
        }

        // 儲存圖片到伺服器並返回路徑
        private async Task<string> SaveImageAsync(IFormFile upload)
        {
            var uploadFolder = Path.Combine("wwwroot", "uploads");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder); // 如果沒資料夾就自動建立
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(upload.FileName);
            var filePath = Path.Combine(uploadFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await upload.CopyToAsync(stream);

            return "/uploads/" + fileName;
        }

        // 解析標籤並附加到日誌
        // 這裡的 AttachTagsToDiaryAsync 方法會清除舊的標籤並添加新的標籤
        private async Task AttachTagsToDiaryAsync(Diary diary, string? tags)
        {
            diary.DiaryTags.Clear();

            var tagNames = (tags ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Distinct();

            foreach (var tagName in tagNames)
            {
                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                var tag = existingTag ?? new Tag { Name = tagName };

                if (existingTag == null)
                    _context.Tags.Add(tag);

                diary.DiaryTags.Add(new DiaryTag
                {
                    DiaryId = diary.Id,
                    Tag = tag
                });
            }
        }
    }
}
