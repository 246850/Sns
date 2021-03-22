using AutoMapper;
using Sns.Domains.Entities;
using Sns.IServices;
using Sns.Models;
using System;

namespace Sns.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly SnsdbContext _snsdbContext;
        private readonly IMapper _mapper;
        public FeedbackService(SnsdbContext snsdbContext,
            IMapper mapper)
        {
            _snsdbContext = snsdbContext;
            _mapper = mapper;
        }

        public void Create(FeedbackCreateRequestDTO request)
        {
            var entity = _mapper.Map<Feedback>(request);
            _snsdbContext.Feedbacks.Add(entity);
            _snsdbContext.SaveChanges();
        }
    }
}
