namespace Calamus.AspNetCore.Users
{
    /// <summary>
    /// 当前授权用户
    /// </summary>
    internal class DefaultIdentityUser<TKey, TUser> : IIdentityUser<TKey, TUser>
        where TKey : struct
        where TUser:class, new()
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public TKey Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 联系号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 用户信息对象
        /// </summary>
        public TUser UserInfo { get; set; }
    }
}
