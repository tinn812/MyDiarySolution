namespace DiaryApp.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DiaryTag> DiaryTags { get; set; }
    }
}
