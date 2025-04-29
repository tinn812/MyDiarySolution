namespace DiaryApp.Models.Dto
{
    public class CreateDiaryDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
