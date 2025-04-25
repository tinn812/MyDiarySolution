using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Models
{
    public class DiaryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "請輸入標題")]
        public string Title { get; set; } = "";

        public DateTime Date { get; set; }

        public string Content { get; set; } = "";

        public string? Tags { get; set; }

        public string? ExistingImagePath { get; set; }  // 顯示用

        public bool DeleteImage { get; set; } // 新增：是否刪除圖片
    }
}
