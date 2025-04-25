using Xunit;
using DiaryApp.Helpers;

namespace DiaryApp.Tests
{
    public class TagHelperTests
    {
        [Fact]
        public void ParseTags_ShouldSplitAndTrimTags()
        {
            var input = " 程式 , 學習,生活 ";
            var result = TagHelper.ParseTags(input);

            Assert.Equal(3, result.Count);
            Assert.Contains("程式", result);
            Assert.Contains("學習", result);
            Assert.Contains("生活", result);
        }

        [Fact]
        public void ParseTags_ShouldIgnoreEmptyAndDuplicateTags()
        {
            var input = " , 程式, , 程式 , 學習";
            var result = TagHelper.ParseTags(input);

            Assert.Equal(2, result.Count);
            Assert.Contains("程式", result);
            Assert.Contains("學習", result);
        }
    }
}
