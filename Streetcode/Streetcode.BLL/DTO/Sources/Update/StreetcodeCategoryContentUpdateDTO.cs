using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Sources.Update
{
    public class StreetcodeCategoryContentUpdateDTO : StreetcodeCategoryContentDTO, IModelState
    {
        public ModelState ModelState { get; set; }
    }
}