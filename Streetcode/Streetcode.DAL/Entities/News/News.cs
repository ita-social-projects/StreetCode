﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.News
{
    [Table("news", Schema = "news")]
    [Index(nameof(URL), IsUnique = true)]
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;
        [Required]
        [MaxLength(25000)]
        public string Text { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string URL { get; set; } = null!;
        public int ImageId { get; set; }
        public Image Image { get; set; } = null!;
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
