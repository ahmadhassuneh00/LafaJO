using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProjAPI.Models;

namespace FinalProjAPI.Dto
{
    public class RentCarBrief
    {
        [ForeignKey("CarID")]
        public int CarID { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime RentalStartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime RentalEndDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCost { get; set; }
    }
}