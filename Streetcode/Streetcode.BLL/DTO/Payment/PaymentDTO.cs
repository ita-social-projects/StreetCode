using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Payment
{
    public class PaymentDTO
    {
        [Required]
        public long Amount { get; set; }

        public string? RedirectUrl { get; set; }
    }
}
