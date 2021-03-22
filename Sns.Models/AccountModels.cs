using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Sns.Models
{
    #region Requests
    public class AccountLoginRequestDTO
    {
        private string _account;
        /// <summary>
        /// 账号
        /// </summary>
        public string Account
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_account))
                    _account = string.Empty;
                return _account;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _account = value.Trim().ToLower();
                }
            }
        }
        private string _password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_password))
                    _password = string.Empty;
                return _password;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _password = value.Trim();
                }
            }
        }
    }

    public class AccountRegisterRequestDTO
    {
        private string _account;
        /// <summary>
        /// 账号
        /// </summary>
        public string Account
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_account))
                    _account = string.Empty;
                return _account;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _account = value.Trim().ToLower();
                }
            }
        }
        public string NickName { get; set; }
        private string _password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_password))
                    _password = string.Empty;
                return _password;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _password = value.Trim();
                }
            }
        }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string Password2 { get; set; }
    }

    public class AccountUpdateIntroRequestDTO
    {
        private string _intro;
        /// <summary>
        /// 一句话介绍
        /// </summary>
        public string Intro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_intro)) return string.Empty;
                return _intro;
            }
            set
            {
                _intro = value;
            }
        }
    }
    public class AccountUpdatePasswordRequestDTO
    {
        /// <summary>
        /// 原始密码
        /// </summary>
        public string Password { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
    }
    #endregion

    #region Responses
    public class AccountOfArticleResultDTO
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        private string _intro;
        /// <summary>
        /// 一句话介绍
        /// </summary>
        public string Intro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_intro)) return string.Empty;
                return _intro;
            }
            set
            {
                _intro = value;
            }
        }

        public int ArticleCount { get; set; }
        /// <summary>
        /// 粉丝数
        /// </summary>
        public int FansCount { get; set; }
        /// <summary>
        /// 登录人是否关注 发布人
        /// </summary>
        public bool IsFollow { get; set; }
    }

    public class AccountListResultDTO
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class AccountDetailResultDTO
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        private string _intro;
        /// <summary>
        /// 一句话介绍
        /// </summary>
        public string Intro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_intro)) return "暂无介绍";
                return _intro;
            }
            set
            {
                _intro = value;
            }
        }
        /// <summary>
        /// 是否关注
        /// </summary>
        public bool IsFollowed { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class AccountOfFollowResultDTO
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
    }

    #endregion

    public class AvatarComparer : IComparer<string>
    {
        public int Compare([NotNull] string x, [NotNull] string y)
        {
            var arr1 = x.Split("_", StringSplitOptions.RemoveEmptyEntries);
            var arr2 = y.Split("_", StringSplitOptions.RemoveEmptyEntries);
            if (arr1.Length != 2 || arr2.Length != 2) return 0;

            int a = Convert.ToInt32(arr1[0]);
            int b = Convert.ToInt32(arr2[0]);
            if (a > b) return 1;
            if (a < b) return -1;
            return 0;
        }
    }
}

