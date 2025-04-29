using DiaryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace DiaryApp.Pages.Diaries
{
    public class CalendarModel : PageModel
    {
        private readonly AppDbContext _context;

        public CalendarModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGetEvents()
        {
            var events = _context.Diaries
                .OrderBy(d => d.Date)
                .Select(d => new
                {
                    title = d.Title,
                    start = d.Date.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss"),
                    url = Url.Page("/Diaries/Details", new { id = d.Id })
                })
                .ToList();

            return new JsonResult(events, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) // 支援中文不亂碼
            });
        }

        public void OnGet()
        {
        }
    }
}
