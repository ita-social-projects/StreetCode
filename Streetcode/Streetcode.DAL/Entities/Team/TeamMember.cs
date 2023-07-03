using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.Team
{
    [Table("team_members", Schema = "team")]
    public class TeamMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        [MaxLength(150)]
        public string? Description { get; set; }

        [Required]
        public bool IsMain { get; set; }

        public List<TeamMemberLink>? TeamMemberLinks { get; set; }

        public List<Positions>? Positions { get; set; }

        [Required]
        public int ImageId { get; set; }

        public Image? Image { get; set; }
    }
}
