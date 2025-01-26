using Microsoft.AspNetCore.Http.Metadata;

namespace FinalProjAPI.Dto
{
    public class CreateReviewDto
    {
        public required string Name { get; set; }
        public required string Content { get; set; }
        public int RegistrationId { get; set; }
        public int CompanyId { get; set; } // Add Company ID
        public int UserId { get; set; } // Add User ID
    }

}
