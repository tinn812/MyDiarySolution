using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DiaryApp.Models;
using Xunit;

namespace DiaryApp.Tests.Models
{
    public class DiaryViewModelTests
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void Valid_Model_Should_Pass_Validation()
        {
            var model = new DiaryViewModel
            {
                Title = "今天的心情",
                Date = DateTime.Now,
                Content = "心情還不錯",
                Tags = "心情,開心",
                DeleteImage = false
            };

            var results = ValidateModel(model);

            Assert.Empty(results); // 沒有驗證錯誤
        }

        [Fact]
        public void Missing_Title_Should_Fail_Validation()
        {
            var model = new DiaryViewModel
            {
                Title = "", // x 標題為空
                Date = DateTime.Now,
                Content = "今天有點空虛"
            };

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.ErrorMessage == "請輸入標題");
        }

        [Fact]
        public void Null_Title_Should_Fail_Validation()
        {
            var model = new DiaryViewModel
            {
                Title = null, //  null 也不行
                Date = DateTime.Now,
                Content = "啥都沒寫"
            };

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.ErrorMessage == "請輸入標題");
        }

        [Fact]
        public void Optional_Fields_Can_Be_Empty()
        {
            var model = new DiaryViewModel
            {
                Title = "空白日記",
                Date = DateTime.Now,
                Content = "", // 可以空
                Tags = null,  // 可以不輸入
                ExistingImagePath = null,
                DeleteImage = false
            };

            var results = ValidateModel(model);

            Assert.Empty(results);
        }
    }
}
