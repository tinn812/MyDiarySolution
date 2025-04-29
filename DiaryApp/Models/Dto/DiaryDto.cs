namespace DiaryApp.Models.Dto
{
    public class DiaryDto
    {
        public int Id { get; set; } 
        public string Title { get; set; } ="";
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}