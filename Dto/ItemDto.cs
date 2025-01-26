namespace FinalProjAPI.Dto
{
    public class ItemDto
    {
         public int ItemId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
         public int? RegistrationId { get; set; }
        public IFormFile? ImageURL { get; set; }
        public int? userId { get; set; }
        public int NumOfItems { get; set; }

    }
}
