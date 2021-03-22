using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Calamus.Infrastructure.Extensions;

namespace Sns.Models
{
    public class FeedbackCreateRequestDTO
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Content { get; set; }
    }

    public class FeedbackCreateValidator : AbstractValidator<FeedbackCreateRequestDTO>
    {
        public FeedbackCreateValidator()
        {
            RuleFor(model => model.Name).NotEmpty().Must(item=> item.MustLengthNotSpace(1, 8)); 
            RuleFor(model => model.Contact).NotEmpty().Must(item => item.MustLengthNotSpace(1, 20));
            RuleFor(model => model.Content).NotEmpty().Must(item => item.MustLength(1, 260));
        }
    }
}
