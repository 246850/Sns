using Calamus.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.IServices
{
    public interface IThumbService:IDependency
    {
        void CreateOrRemove(int articleId);
    }
}
