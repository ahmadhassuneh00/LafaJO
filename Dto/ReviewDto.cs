namespace FinalProjAPI.Dto
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public required string Name { get; set; }
        public required string Content { get; set; }
        public int? RegistrationId { get; set; }

        public int? UserId { get; set; }

    }
}