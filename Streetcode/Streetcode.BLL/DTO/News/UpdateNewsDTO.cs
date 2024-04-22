﻿using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.News
{
    public class UpdateNewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int? ImageId { get; set; }
        public string URL { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
