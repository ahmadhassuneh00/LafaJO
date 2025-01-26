namespace FinalProjAPI.Dto;
public partial class CompanyForLogin
{
    public string Email { get; set; }
    public string Password { get; set; }

    public CompanyForLogin()
    {
        if (Email == null)
        {
            Email = " ";
        }
        if (Password == null)
        {
            Password = " ";
        }
    }
}