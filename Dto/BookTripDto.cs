using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjAPI.Models
{
    public class BookTripDto
    {
        public int TripId { get; set; }

        public int UserId { get; set; }

        [Required]
        public int numberOfPersons { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCost { get; set; }

    }
}