using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.AdditionalContent.Tag
{
	public class StreetcodeTagUpdateDto : StreetcodeTagDto, IModelState
	{
		public int StreetcodeId { get; set; }
		public ModelState ModelState { get; set; }
	}
}
