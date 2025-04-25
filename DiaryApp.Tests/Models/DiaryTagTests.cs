using Xunit;
using DiaryApp.Models;

namespace DiaryApp.Tests.Models
{
    public class DiaryTagTests
    {
        [Fact]
        public void Can_Set_And_Get_DiaryTag_Properties()
        {
            // Arrange
            var diary = new Diary { Id = 1, Title = "Sample" };
            var tag = new Tag { Id = 2, Name = "TestTag" };

            var diaryTag = new DiaryTag
            {
                DiaryId = diary.Id,
                Diary = diary,
                TagId = tag.Id,
                Tag = tag
            };

            // Assert
            Assert.Equal(1, diaryTag.DiaryId);
            Assert.Equal("Sample", diaryTag.Diary.Title);
            Assert.Equal(2, diaryTag.TagId);
            Assert.Equal("TestTag", diaryTag.Tag.Name);
        }
    }
}
