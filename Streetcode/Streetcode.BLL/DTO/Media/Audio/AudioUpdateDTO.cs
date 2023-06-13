using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Media.Audio
{
	public class AudioUpdateDTO : IModelState
	{
		public int AudioId { get; set; }
		public ModelState ModelState { get; set; }
	}
}
