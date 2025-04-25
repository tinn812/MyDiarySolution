using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiaryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Pages.Diaries
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DiaryViewModel Diary { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public void OnGet()
        {
            Diary = new DiaryViewModel
            {
                Date = DateTime.Today,
                Tags = string.Empty
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var newDiary = new Diary
            {
                Title = Diary.Title,
                Date = Diary.Date == default ? DateTime.Today : Diary.Date,
                Content = Diary.Content,
                CreatedAt = DateTime.Now
            };

            if (Upload != null && Upload.Length > 0)
            {
                newDiary.ImagePath = await SaveImageAsync(Upload);
            }

            _context.Diaries.Add(newDiary);
            await _context.SaveChangesAsync();

            await AttachTagsToDiaryAsync(newDiary, Diary.Tags);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "日誌建立成功！";
            return RedirectToPage("Index");
        }

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

        private async Task AttachTagsToDiaryAsync(Diary diary, string? tags)
        {
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

                _context.DiaryTags.Add(new DiaryTag
                {
                    DiaryId = diary.Id,
                    Tag = tag
                });
            }
        }
    }
}
