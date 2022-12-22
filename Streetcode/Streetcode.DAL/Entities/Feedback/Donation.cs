using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Feedback;

[Table("donations", Schema = "feedback")]
public class Donation
{
    public int Id { get; set; }
}
