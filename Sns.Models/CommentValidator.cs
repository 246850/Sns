using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Calamus.Infrastructure.Extensions;

namespace Sns.Models
{
    public class CommentCreateValidator:AbstractValidator<CommentCreateRequestDTO>
    {
        public CommentCreateValidator()
        {
            RuleFor(model => model.QuoteId).GreaterThanOrEqualTo(0);
            RuleFor(model => model.ArticleId).GreaterThanOrEqualTo(1);
            RuleFor(model => model.Body).NotEmpty().Must(item => item.Trim().MustLength(1, 450)).WithMessage("评论字符串长度[1, 450]"); 
        }
    }
}
