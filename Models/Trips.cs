using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjAPI.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!; // Non-nullable, assuming a title is always required.

        [Required]
        public string Content { get; set; } = null!; // Non-nullable, assuming content is always required.

        public int? RegistrationId { get; set; } // Nullable if not every trip has a company.

        [ForeignKey("RegistrationId")]
        public Company? Company { get; set; } // Nullable relationship to Company.

        [Required]
        public DateTime DepartureDate { get; set; }

        public DateTime? ReturnDate { get; set; } // Nullable since return date may not always be applicable.

        // Assuming ImageURL is a URL to the image. If storing the image data as a byte array, keep it as byte[].
        public byte[] ImageURL { get; set; } = null!; // Assuming required, change to string if storing as a URL.

        [Required]
        public int Price { get; set; } // Changed to decimal for better price representation.

        [Required]
        public int NumOfTourist { get; set; }

    }
}
