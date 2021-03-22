using Calamus.Ioc;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.IServices
{
    public interface IFeedbackService:IDependency
    {
        void Create(FeedbackCreateRequestDTO request);
    }
}
