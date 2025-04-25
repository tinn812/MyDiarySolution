using System;
using System.Collections.Generic;

namespace DiaryApp.Models
{
    public class Diary
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<DiaryTag> DiaryTags { get; set; }

        public string? ImagePath { get; set; }

    }
}
