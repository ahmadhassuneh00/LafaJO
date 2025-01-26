namespace FinalProjAPI.Dto
{
    public class TripDto
    {
        public int TripId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public int? RegistrationId { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int Price { get; set; }
        public int NumOfTourist { get; set; }
       
    }
}
