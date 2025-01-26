using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjAPI.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }  // Primary key

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }  // Name of the item

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }  // Price of the item

        
         public int? RegistrationId { get; set; } // Nullable if not every trip has a company.

        [ForeignKey("RegistrationId")]
        public Company? Company { get; set; } // Nullable relationship to Company.

        public byte[]? ImageURL { get; set; }  // Optional image


        [ForeignKey("User")]
        public int? userId { get; set; }  // Optional reference to User

        public virtual Users? User { get; set; }  // Navigation to User
        public int NumOfItems { get; set; }
    }
}
