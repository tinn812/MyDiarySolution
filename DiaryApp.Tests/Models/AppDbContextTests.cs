using Xunit;
using Microsoft.EntityFrameworkCore;
using DiaryApp.Models;
using System.Linq;

namespace DiaryApp.Tests
{
    public class AppDbContextTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void Can_Add_And_Retrieve_Diary()
        {
            using var context = GetInMemoryDbContext();

            var diary = new Diary
            {
                Title = "Test Diary",
                Content = "Hello world!",
                Date = DateTime.Today,
                CreatedAt = DateTime.Now
            };

            context.Diaries.Add(diary);
            context.SaveChanges();

            var saved = context.Diaries.FirstOrDefault();
            Assert.NotNull(saved);
            Assert.Equal("Test Diary", saved.Title);
        }

        [Fact]
        public void Can_Associate_Diary_With_Tags()
        {
            using var context = GetInMemoryDbContext();

            var diary = new Diary
            {
                Title = "Diary with Tags",
                Content = "With some tags",
                Date = DateTime.Today,
                CreatedAt = DateTime.Now
            };

            var tag = new Tag
            {
                Name = "Life"
            };

            context.Diaries.Add(diary);
            context.Tags.Add(tag);
            context.SaveChanges();

            var diaryTag = new DiaryTag
            {
                DiaryId = diary.Id,
                TagId = tag.Id
            };

            context.DiaryTags.Add(diaryTag);
            context.SaveChanges();

            var loaded = context.Diaries
                .Include(d => d.DiaryTags)
                .ThenInclude(dt => dt.Tag)
                .FirstOrDefault();

            Assert.NotNull(loaded);
            Assert.Single(loaded.DiaryTags);
            Assert.Equal("Life", loaded.DiaryTags.First().Tag.Name);
        }
    }
}
