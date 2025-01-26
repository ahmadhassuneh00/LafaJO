namespace FinalProjAPI.Dto
{
    public class CarDto
    {
        public int CarId { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? LicensePlate { get; set; }
        public string? FuelType { get; set; }
        public string? TransmissionType { get; set; }
        public string? Color { get; set; }
        public decimal DailyRate { get; set; }
        public IFormFile? ImageURL { get; set; }
        public int RegistrationId { get; set; }

    }
}
