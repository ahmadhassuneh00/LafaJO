namespace FinalProjAPI.Dto
{
    public partial class UserForLoginConformationDto
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        UserForLoginConformationDto()
        {

            if (PasswordHash == null)
            {
                PasswordHash = new byte[0];
            }
            if (PasswordSalt == null)
            {
                PasswordSalt = new byte[0];
            }
        }


    }
}