using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.DTO.Streetcode
{
    public class StreetcodeMainPageDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Alias { get; set; }
        public string? Teaser { get; set; }
        public string? Text { get; set; }
        public int ImageId { get; set; }

        public string TransliterationUrl { get; set; }
    }
}
