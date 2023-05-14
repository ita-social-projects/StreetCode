using AutoMapper;
using Streetcode.BLL.DTO.News;
using Streetcode.DAL.Entities.News;

namespace Streetcode.BLL.Mapping.Newss
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<NewsDTO, News>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.URL, opt => opt.MapFrom(src => src.URL))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));

            CreateMap<News, NewsDTO>().ReverseMap();
        }
    }
}
