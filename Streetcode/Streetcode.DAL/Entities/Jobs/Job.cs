﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Jobs
{
    [Table("job", Schema = "jobs")]
    public class Job
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(65)]
        public string Title { get; set; } = null!;

        [Required]
        public bool Status { get; set; }

        [Required]
        [MaxLength(3000)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string Salary { get; set; } = null!;
    }
}
