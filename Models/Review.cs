using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjAPI.Models
{
    public class Review
    {
        [Key]
        [Column("ReviewID")]
        public int ReviewId { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; }

        [Required]
        public required string Content { get; set; }

        [ForeignKey("RegistrationId")]
        public int? RegistrationId { get; set; }

        // Navigation property for Company
        public virtual Company Company { get; set; } = null!; // Use null-forgiving operator

        // Navigation property for User
        public virtual Users User { get; set; } = null!; // Use null-forgiving operator
    }
}
