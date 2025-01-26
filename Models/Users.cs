using System.ComponentModel.DataAnnotations;

namespace FinalProjAPI.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }

        public byte[] passwordSalt { get; set; }
        public byte[] passwordHash { get; set; }

        public Users()
        {
            passwordSalt = new byte[0];
            passwordHash = new byte[0];
        }

        public static implicit operator Users(Task<Users?> v)
        {
            throw new NotImplementedException();
        }
    }
}
