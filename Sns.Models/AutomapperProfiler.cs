using Sns.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

namespace Sns.Models
{
    public class AutomapperProfiler:AutoMapper.Profile
    {
        public AutomapperProfiler()
        {
            
            CreateMap<CommentCreateRequestDTO, Comment>().ForMember(dest=> dest.Body, mapper=> mapper.MapFrom(source=> HttpUtility.HtmlEncode(source.Body)));
            CreateMap<FeedbackCreateRequestDTO, Feedback>();
        }
    }
}
