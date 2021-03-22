using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.Models
{
    public class LoginAuthModel
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
        public DateTime CreateTime { get; set; }
    }
}
