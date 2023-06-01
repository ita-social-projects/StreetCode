using Streetcode.DAL.Enums;
using Streetcode.BLL.DTO.Streetcode.Update.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode.Update.Media;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.BLL.DTO.Streetcode.Update.Transactions;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Streetcode.Update.TextContent;

namespace Streetcode.BLL.DTO.Streetcode.Update
{
  public class StreetcodeUpdateDTO
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
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
    /*		public TextUpdateDTO Text { get; set; }
        public List<SubtitleUpdateDTO> Subtitles { get; set; }*/
		public IEnumerable<TimelineItemUpdateDTO> TimelineItems { get; set; }
		/*public TransactionLinkUpdateDTO TransactionLink { get; set; }*/

		public IEnumerable<FactUpdateDTO> Facts { get; set; }
		// public IEnumerable<VideoUpdateDTO> Videos { get; set; }

        // public IEnumerable<StreetcodeDTO> RelatedFigures { get; set; }
	}
}
