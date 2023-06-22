using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Transactions;

[Table("transaction_links", Schema = "transactions")]
public class TransactionLink
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [MaxLength(255)]
    public string? UrlTitle { get; set; }

    [Required]
    [MaxLength(255)]
    public string? Url { get; set; }

    [Required]
    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }
}