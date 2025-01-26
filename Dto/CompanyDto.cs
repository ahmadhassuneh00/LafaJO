namespace FinalProjAPI.Dto;

public class CompanyDto
{

    public int RegistrationID { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public byte[]? passwordHash { get; set; }
    public byte[]? passwordSalt { get; set; }
    public string? CompanyName { get; set; }
    public string? ContactPersonName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public int TypeID { get; set; }

}