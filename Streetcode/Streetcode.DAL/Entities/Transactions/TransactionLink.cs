using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Transactions;

[Table("transaction_links", Schema = "transactions")]
public class TransactionLink
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Url { get; set; }

    [Required]
    public string QrCodeUrl { get; set; }

    [Required]
    public int StreetcodeId { get; set; }
    
    public Streetcode.Streetcode? Streetcode { get; set; }
}