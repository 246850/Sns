using Sns.Domains.Entities;
using System.Web;

namespace Sns.Models
{
    public class AutomapperProfiler:AutoMapper.Profile
    {
        public AutomapperProfiler()
        {
            CreateMap<CommentCreateRequestDTO, Comment>().ForMember(dest=> dest.Body, mapper=> mapper.MapFrom(source=> HttpUtility.HtmlEncode(source.Body)));
            CreateMap<FeedbackCreateRequestDTO, Feedback>().ForMember(dest => dest.Content, mapper => mapper.MapFrom(source => HttpUtility.HtmlEncode(source.Content)));
        }
    }
}
