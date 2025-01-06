using System.ComponentModel.DataAnnotations;

namespace Streetcode.DAL.Entities.Users.Expertise;

public class UserExpertise
{
    [Required]
    public int ExpertiseId { get; set; }

    [Required]
    public string UserId { get; set; }

    public Expertise? Expertise { get; set; }

    public User? User { get; set; }
}