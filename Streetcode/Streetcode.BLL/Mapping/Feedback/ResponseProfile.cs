using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.DAL.Entities.Feedback;


namespace Streetcode.BLL.Mapping.Feedback;


public class ResponseProfile: Profile
{
    public ResponseProfile()
    {
        CreateMap<Response,ResponseDTO>().ReverseMap();
    }
}