using Calamus.Result;
using Microsoft.AspNetCore.Mvc.Razor;
using Sns.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Sns.WebHosts.Startups
{
    public abstract class CommonRazorPageBase<TModel> : RazorPage<TModel>
    {
        protected LoginAuthModel AuthModel
        {
            get
            {
                if (!User.Identity.IsAuthenticated) return null;
                try
                {
                    string sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                    LoginAuthModel model = System.Text.Json.JsonSerializer.Deserialize<LoginAuthModel>(sub);
                    return model;
                }
                catch
                {
                    return null;
                }

            }
        }
    }
}
