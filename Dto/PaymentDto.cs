using System.ComponentModel.DataAnnotations;

namespace FinalProjAPI.Dto
{
    public class PaymentDto
    {
        [RegularExpression(@"^\d{13,19}$", ErrorMessage = "Invalid card number.")]
        public string? CardNumber { get; set; }

        [Required]

        public string? cardHolder { get; set; }

        [Required]

        public string? expirationDate { get; set; }

        [Required]

        public int cvv { get; set; }
    }
}