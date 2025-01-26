namespace FinalProjAPI.Dto
{
    public class CreateItemDto
    {

        public required string Name { get; set; }
        public decimal Price { get; set; }
         public int? RegistrationId { get; set; }
        public IFormFile? ImageURL { get; set; }

        public int NumOfItems { get; set; }

    }
}
