using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Media.Images
{
    public class StreetcodeImage
    {
        [Required]
        public int StreetcodeId { get; set; }

        [Required]
        public int ImageId { get; set; }

        public Image Image { get; set; } = new();

        public StreetcodeContent Streetcode { get; set; } = new();
    }
}
