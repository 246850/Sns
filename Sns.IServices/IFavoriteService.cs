using Calamus.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.IServices
{
    public interface IFavoriteService:IDependency
    {
        void CreateOrRemove(int articleId);
    }
}
