namespace FinalProjAPI.Dto
{
    public partial class CompanyForRegistration
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        
        public string? CompanyName { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public int TypeID { get; set; }
        public string? password { get; set; }
        public string? passwordConfirm { get; set; }
        public string? TypeName
        {
            get
            {
                return TypeID switch
                {
                    1 => "Tourist Office",
                    2 => "Car Company",
                    3 => "Market Vendor",
                    _ => " "
                };
            }
        }
    }

}