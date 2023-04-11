using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.Mapping.Streetcode
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<EventStreetcode, RelatedFigureDTO>()
                .ForPath(dto => dto.Title, conf => conf
                    .MapFrom(e => e.Title))
                .ForPath(dto => dto.Url, conf => conf
                    .MapFrom(e => e.TransliterationUrl))
                .ForPath(dto => dto.ImageId, conf => conf
                    .MapFrom(e => e.Images.Select(i => i.Id).FirstOrDefault()));

            CreateMap<PersonStreetcode, RelatedFigureDTO>()
                .ForPath(dto => dto.Title, conf => conf
                    .MapFrom(e => $"{e.Rank ?? ""} {e.FirstName} {e.LastName}"))
                .ForPath(dto => dto.Url, conf => conf
                    .MapFrom(e => e.TransliterationUrl))
                .ForPath(dto => dto.ImageId, conf => conf
                    .MapFrom(e => e.Images.Select(i => i.Id).FirstOrDefault()));
        }
    }
}
