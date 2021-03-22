namespace Calamus.AspNetCore.Users
{
    /// <summary>
    /// 当前授权用户 接口
    /// </summary>
    /// <typeparam name="TKey">主键Id类型</typeparam>
    /// <typeparam name="TUser">用户信息主体</typeparam>
    public interface IIdentityUser<TKey, TUser>
        where TKey:struct
        where TUser:class, new()
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        TKey Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        string Account { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        string Avatar { get; set; }
        /// <summary>
        /// 联系号码
        /// </summary>
        string Mobile { get; set; }
        /// <summary>
        /// 用户信息对象
        /// </summary>
        TUser UserInfo { get; set; }
    }
}
