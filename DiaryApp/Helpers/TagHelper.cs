using System;
using System.Collections.Generic;
using System.Linq;

namespace DiaryApp.Helpers
{
    public static class TagHelper
    {
        public static List<string> ParseTags(string input)
        {
            return input?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(tag => tag.Trim())
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Distinct() // 避免重複標籤
                .ToList() ?? new List<string>();
        }
    }
}
