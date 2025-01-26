using System.ComponentModel.DataAnnotations;

namespace FinalProjAPI.Models
{
    public class CompanyType
    {
        public int TypeID { get; set; } // No change needed here

        [Required]
        public string? TypeName { get; set; }

        public ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}
