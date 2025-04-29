namespace DiaryApp.Models.Dto
{
    public class DiaryDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
