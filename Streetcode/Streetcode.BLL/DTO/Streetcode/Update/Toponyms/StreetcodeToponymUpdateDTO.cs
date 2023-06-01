using Streetcode.BLL.DTO.Streetcode.Update.Interface;

namespace Streetcode.BLL.DTO.Streetcode.Update.Toponyms
{
	public class StreetcodeToponymUpdateDTO : IChanged
	{
		public int StreetcodeId { get; set; }
		public int ToponymId { get; set; }
		public bool? IsChanged { get; set; }
	}
}
