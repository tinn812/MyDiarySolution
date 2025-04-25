using System;
using Xunit;
using DiaryApp.Models;

namespace DiaryApp.Tests.Models
{
    public class DiaryTests
    {
        [Fact]
        public void Diary_Constructor_InitializesDiaryTags()
        {
            // Arrange & Act
            var diary = new Diary();

            // Assert
            Assert.NotNull(diary.DiaryTags);         // 確保不是 null
            Assert.Empty(diary.DiaryTags);           // 預設應為空集合
        }

        [Fact]
        public void Can_Set_And_Get_Diary_Properties()
        {
            // Arrange
            var diary = new Diary
            {
                Id = 1,
                Title = "Test Title",
                Date = new DateTime(2025, 4, 25),
                Content = "This is a test content.",
                CreatedAt = DateTime.Now,
                ImagePath = "uploads/test.jpg"
            };

            // Act & Assert
            Assert.Equal(1, diary.Id);
            Assert.Equal("Test Title", diary.Title);
            Assert.Equal("This is a test content.", diary.Content);
            Assert.Equal("uploads/test.jpg", diary.ImagePath);
        }
    }
}
