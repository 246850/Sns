using Calamus.Ioc;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sns.IServices
{
    public interface ICommentService:IDependency
    {
        void Create(CommentCreateRequestDTO request);
        void Delete(int id);
        void ThumbupCreateOrRemove(int id);
    }
}
