namespace DiaryApp.Models
{
    public class DiaryTag
    {
        public int DiaryId { get; set; }
        public Diary Diary { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
