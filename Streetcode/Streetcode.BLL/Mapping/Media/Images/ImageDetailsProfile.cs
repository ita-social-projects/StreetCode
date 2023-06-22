using AutoMapper;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.BLL.Mapping.Media.Images
{
    public class ImageDetailsProfile : Profile
    {
        public ImageDetailsProfile()
        {
            CreateMap<ImageDetails, ImageDetailsDto>().ReverseMap();
        }
    }
}
