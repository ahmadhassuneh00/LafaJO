using System.ComponentModel.DataAnnotations;

namespace FinalProjAPI.Models
{
    public class Payments
    {
        [Key]
        public int paymentID { get; set; }
        [RegularExpression(@"^\d{13,19}$", ErrorMessage = "Invalid card number.")]
        public string? CardNumber { get; set; }

        [Required]
        public String? cardHolder { get; set; }

        [Required]
        public string? expirationDate { get; set; }

        [Required]
        public int cvv { get; set; }

    }
}