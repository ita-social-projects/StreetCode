using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.Update
{
	public class StreetcodeUpdateDTO
	{
		public int Id { get; set; }
		public int Index { get; set; }
		public string? Teaser { get; set; }
		public string DateString { get; set; }
		public string? Alias { get; set; }
		public StreetcodeStatus Status { get; set; }
		public string Title { get; set; }
		public string TransliterationUrl { get; set; }
		public DateTime UpdatedAt { get; set; }
		public DateTime EventStartOrPersonBirthDate { get; set; }
		public DateTime? EventEndOrPersonDeathDate { get; set; }
	}
}
