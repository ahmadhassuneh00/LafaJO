using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjAPI.Models
{
    public class BookTrip
    {
        [Key]
        public int BookId { get; set; }

        public int TripId { get; set; }

        public int UserId { get; set; }

        [Required]
        public int numberOfPersons { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCost { get; set; }

        // Navigation properties
        [ForeignKey("TripId")]
        public Trip? Trip { get; set; }

        [ForeignKey("UserId")]
        public Users? User { get; set; }
    }
}