using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalProjAPI.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _config;

        public AuthHelper(IConfiguration config)
        {
            _config = config;
        }

        public byte[] GetPasswordHash(string password, byte[] PasswordSalt)
        {
            // Correcting the section name and adding null check
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string passwordKey = _config.GetSection("AppSettings:PasswordKey").Value;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (string.IsNullOrEmpty(passwordKey))
            {
                throw new ArgumentNullException("PasswordKey is missing from configuration.");
            }

            string passwordSaltPlusString = passwordKey + Convert.ToBase64String(PasswordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8);
        }

        public string CreateToken(int userId)
        {
            // Ensure TokenKey is not null
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string tokenKey = _config.GetSection("AppSettings:TokenKey").Value;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new ArgumentNullException("TokenKey is missing from configuration.");
            }

            Claim[] claims = new Claim[]
            {
                new Claim("UserId", userId.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string CreateTokenWithClaims(List<Claim> claims)
        {
            // Ensure TokenKey is not null
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string tokenKey = _config.GetSection("AppSettings:TokenKey").Value;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new ArgumentNullException("TokenKey is missing from configuration.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
