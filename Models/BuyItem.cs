using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjAPI.Models
{
    public class BuyItem
    {
        [Key]
        public int BuyId { get; set; }

        public int ItemId { get; set; }

        public int UserId { get; set; }

        [Required]
        public int numberOfItems { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCost { get; set; }

        // Navigation properties
        [ForeignKey("ItemId")]
        public Item? Item { get; set; }

        [ForeignKey("UserId")]
        public Users? User { get; set; }
    }
}