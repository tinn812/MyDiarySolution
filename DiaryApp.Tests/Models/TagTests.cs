using Xunit;
using DiaryApp.Models;
using System.Collections.Generic;

namespace DiaryApp.Tests.Models
{
    public class TagTests
    {
        [Fact]
        public void Can_Create_Tag_With_Properties()
        {
            // Arrange
            var tag = new Tag
            {
                Id = 1,
                Name = "Travel",
                DiaryTags = new List<DiaryTag>()
            };

            // Assert
            Assert.Equal(1, tag.Id);
            Assert.Equal("Travel", tag.Name);
            Assert.NotNull(tag.DiaryTags);
        }
    }
}
