using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjAPI.Models
{
    public class Car
    {
        [Key]
        public int CarID { get; set; }

        [Required]
        [StringLength(50)]
        public string? Make { get; set; }

        [Required]
        [StringLength(50)]
        public string? Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [StringLength(20)]
        public string? LicensePlate { get; set; }

        [StringLength(20)]
        public string? FuelType { get; set; }

        [StringLength(20)]
        public string? TransmissionType { get; set; }

        [StringLength(30)]
        public string? Color { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DailyRate { get; set; }

        public byte[]? ImageURL { get; set; }

        public int RegistrationId { get; set; }

        [ForeignKey("RegistrationId")]
        public Company? Company { get; set; }
    }
}
