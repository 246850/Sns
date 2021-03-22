using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Calamus.Infrastructure.Extensions;
using System.Text.RegularExpressions;

namespace Sns.Models
{
    public class ArticleCreateOrUpdateValidator : AbstractValidator<ArticleCreateOrUpdateRequestDTO>
    {
        public ArticleCreateOrUpdateValidator()
        {
            RuleFor(model => model.Title).Must(item => item.MustLengthNotSpace(4, 30)).WithMessage("标题字符长度[4, 30]且不能包含空格");
            When(model => !string.IsNullOrWhiteSpace(model.TopicName), () =>
             {
                 RuleFor(model => model.TopicName).Must(item => item.MustLengthNotSpace(1, 20)).WithMessage("话题字符长度[1, 20]且不能包含空格");
             });
            RuleFor(model => model.Body).Must(item => item.FilterHtml().TrimStart().MustLength(10, 2000)).WithMessage("内容字符长度[10, 2000]");
        }
    }

    public class ImageUploadValidator : AbstractValidator<ImageUploadRequestDTO>
    {
        public ImageUploadValidator()
        {
            RuleFor(model => model.Base64).NotEmpty().Must(item => Regex.IsMatch(item, "data:image/(gif|jpg|png|jpeg|bmp);base64", RegexOptions.IgnoreCase)).WithMessage("仅支持jpg,jpeg,png,bmp,gif格式的图片");
        }
    }
}
