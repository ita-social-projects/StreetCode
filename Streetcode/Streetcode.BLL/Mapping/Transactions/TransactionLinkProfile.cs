using AutoMapper;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.DAL.Entities.Transactions;

namespace Streetcode.BLL.Mapping.Transactions;

public class TransactionLinkProfile : Profile
{
    public TransactionLinkProfile()
    {
        CreateMap<TransactionLink, TransactLinkDTO>()
            .ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.UrlTitle))
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url));

        CreateMap<TransactionLink, TransactionLinkUpdateDTO>()
           .ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.UrlTitle))
           .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url))
           .ReverseMap();
	}
}