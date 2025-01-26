using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjAPI.Models
{
    public class RentCar
    {
        [Key]
        public int RentalID { get; set; }

        public int CarID { get; set; }

        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime RentalStartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime RentalEndDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCost { get; set; }

        // Navigation properties
        [ForeignKey("CarID")]
        public Car? Car { get; set; }

        [ForeignKey("UserId")]
        public Users? User { get; set; }
    }
}
