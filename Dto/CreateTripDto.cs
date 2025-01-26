namespace FinalProjAPI.Dto
{
    public class CreateTripDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public int? RegistrationId { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public IFormFile? ImageURL { get; set; }
        public int NumOfTourist { get; set; }
        public int Price { get; set; }
    }
}
