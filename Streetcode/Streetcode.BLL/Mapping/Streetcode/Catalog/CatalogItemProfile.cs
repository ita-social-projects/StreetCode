using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.CatalogItem;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Mapping.Streetcode.Catalog;

public class CatalogItemProfile : Profile
{
    public CatalogItemProfile()
    {
        CreateMap<EventStreetcode, CatalogItem>()
            .IncludeBase<StreetcodeContent, CatalogItem>();

        CreateMap<PersonStreetcode, CatalogItem>()
            .IncludeBase<StreetcodeContent, CatalogItem>();

        CreateMap<StreetcodeContent, CatalogItem>()
            .ForPath(dto => dto.Url, conf => conf
                .MapFrom(e => e.TransliterationUrl))
            .ForPath(dto => dto.ImageId, conf => conf
                .MapFrom(e => e.Images.Select(i => i.Id).FirstOrDefault()));
    }
}
