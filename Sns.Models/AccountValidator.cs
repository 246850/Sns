using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sns.Models
{
    public class AccountRegisterValidator : AbstractValidator<AccountRegisterRequestDTO>
    {
        public AccountRegisterValidator()
        {
            RuleFor(model => model.Account).NotEmpty().Must(item =>
            {
                return Regex.IsMatch(item, @"^[a-z|A-Z|_]\w{3,20}$");
            }).WithMessage("账号仅限下划线、字母、数字组成，且首字符不能为数字，4至20个字符长度");
            RuleFor(model => model.NickName).NotEmpty().Must(item =>
             {
                 var length = Encoding.UTF8.GetByteCount(item);
                 return Regex.IsMatch(item, @"^\S{3,24}") && length >= 1 && length <= 24;

             }).WithMessage("昵称3至24个字符长度，最大8个中文，1个中文占3个长度");
            RuleFor(model => model.Password).NotEmpty().Must(item =>
            {
                return Regex.IsMatch(item, @"^\w{6,20}$");
            }).WithMessage("密码仅限下划线、字母、数字组成，6至20个字符长度");
            RuleFor(model => model.Password2).NotEmpty().Equal(model => model.Password).WithMessage("两次密码不一致");
        }
    }

    public class AccountUpdateIntroValidator : AbstractValidator<AccountUpdateIntroRequestDTO>
    {
        public AccountUpdateIntroValidator()
        {
            When(model => !string.IsNullOrWhiteSpace(model.Intro), () =>
            {
                RuleFor(model => model.Intro).Must(item =>
                {
                    var length = Encoding.UTF8.GetByteCount(item);
                    return Regex.IsMatch(item, @"^\S{1,150}") && length >= 1 && length <= 150;

                }).WithMessage("一句话介绍支持150个字符长度，最大50个中文，1个中文占3个长度");
            });
        }
    }

    public class AccountUpdatePasswordValidator : AbstractValidator<AccountUpdatePasswordRequestDTO>
    {
        public AccountUpdatePasswordValidator()
        {
            RuleFor(model => model.Password).NotEmpty().Must(item =>
            {
                return Regex.IsMatch(item, @"^\w{6,20}$");
            }).WithMessage("原始密码仅限下划线、字母、数字组成，6至20个字符长度"); 
            RuleFor(model => model.Password1).NotEmpty().Must(item =>
            {
                return Regex.IsMatch(item, @"^\w{6,20}$");
            }).WithMessage("新的密码仅限下划线、字母、数字组成，6至20个字符长度");
            RuleFor(model => model.Password2).NotEmpty().Equal(model => model.Password1).WithMessage("两次密码不一致");
        }
    }
}
