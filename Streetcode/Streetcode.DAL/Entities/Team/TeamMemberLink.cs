using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Enums;

namespace Streetcode.DAL.Entities.Team
{
    [Table("team_member_links", Schema = "team")]
    public class TeamMemberLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public LogoType LogoType { get; set; }

        [Required]
        [MaxLength(255)]
        public string? TargetUrl { get; set; }

        [Required]
        public int TeamMemberId { get; set; }

        public TeamMember? TeamMember { get; set; }
    }
}