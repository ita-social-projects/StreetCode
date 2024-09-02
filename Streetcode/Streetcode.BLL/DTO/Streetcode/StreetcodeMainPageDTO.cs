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
        public string Title { get; set; } = null!;
        public string Alias { get; set; } = null!;
        public string Teaser { get; set; } = null!;
        public string Text { get; set; } = null!;
        public int ImageId { get; set; }
        public string TransliterationUrl { get; set; } = null!;
    }
}
