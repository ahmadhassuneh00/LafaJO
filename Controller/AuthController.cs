using System.Data;
using System.Security.Cryptography;
using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Helpers;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
namespace DotnetAPI.Controllers

{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;
            _authHelper = new AuthHelper(config);
        }
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public IActionResult SignUp(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExists = "SELECT Email FROM FinalProjUser.Users WHERE Email ='"
                + userForRegistration.Email + "'";
                IEnumerable<string> existsUser = _dapper.LoadData<string>(sqlCheckUserExists);
                if (!existsUser.Any())
                {
                    byte[] PasswordSalt = new byte[120 / 8];
                    using (RandomNumberGenerator ran = RandomNumberGenerator.Create())
                    {
                        ran.GetNonZeroBytes(PasswordSalt);
                    }
                    string PasswordSaltPlussSrting = _config.GetSection("AppSittings:PasseordKey").Value
                    + Convert.ToBase64String(PasswordSalt);

                    byte[] PasswordHash = _authHelper.GetPasswordHash(userForRegistration.Password, PasswordSalt);

                    string sqlAddAuth = @"INSERT INTO FinalProjUser.Users ([FirstName],
                    [LastName],[Email],[Gender],
                    [PasswordHash],
                    [PasswordSalt]) VALUES('" + userForRegistration.FirstName + @"','" + userForRegistration.LastName +
                     @"','" + userForRegistration.Email + "','" + userForRegistration.Gender + "',@PasswordHash,@PasswordSalt)";//we add @ to add variable to sql
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter PasswordSaltParameters = new SqlParameter
                    ("@PasswordSalt", SqlDbType.VarBinary);
                    PasswordSaltParameters.Value = PasswordSalt;

                    SqlParameter PasswordHashParameters = new SqlParameter
                  ("@PasswordHash", SqlDbType.VarBinary);

                    PasswordHashParameters.Value = PasswordHash;

                    sqlParameters.Add(PasswordSaltParameters);

                    sqlParameters.Add(PasswordHashParameters);

                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        return Ok();

                    }
                }
                throw new Exception("falied to regester user");
            }
            throw new Exception("user with this Email is exists");
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {

            string sqlForHashAndSolt = @"SELECT [passwordHash],
             [passwordSalt] FROM FinalProjUser.Users WHERE Email = '"
             + userForLoginDto.Email + "'";
            UserForLoginConformationDto userForLoginConformationDto = _dapper.LoadDataSingle
            <UserForLoginConformationDto>(sqlForHashAndSolt);
            byte[] PasswordHash = _authHelper.GetPasswordHash(userForLoginDto.Password, userForLoginConformationDto.PasswordSalt);
            for (int index = 0; index < PasswordHash.Length; index++)
            {
                if (PasswordHash[index] != userForLoginConformationDto.PasswordHash[index])
                {
                    return StatusCode(401, "uncorrect password");
                }
            }
            string UserIdsql = @"SELECT [UserId] FROM FinalProjUser.Users
            where Email='" + userForLoginDto.Email + "'";

            int UserId = _dapper.LoadDataSingle<int>(UserIdsql);
            return Ok(new Dictionary<string, string>
            {
                {"token",_authHelper.CreateToken(UserId)}
            });
        }
        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("UserID")?.Value + "";
            string userIdSql = @"SELECT [UserId] FROM FinalProjUser.Users WHERE UserId ='" + userId + "'";
            int userFromDB = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>
        {
        {"userId", userFromDB.ToString()}
        });
        }

        [AllowAnonymous]
        [HttpPost("RegisterCompany")]
        public IActionResult Register(CompanyForRegistration companyForRegistration)
        {
            // Ensure passwords match
            if (companyForRegistration.password != companyForRegistration.passwordConfirm)
            {
                throw new Exception("Passwords do not match");
            }

            // Check if company email already exists
            string sqlCheckCompanyExists = $"SELECT Email FROM FinalProjCompanies.Registration WHERE Email='{companyForRegistration.Email}'";
            IEnumerable<string> existsCompany = _dapper.LoadData<string>(sqlCheckCompanyExists);

            if (existsCompany.Any())
            {
                throw new Exception("A company with this Email already exists");
            }

            // Generate password salt
            byte[] passwordSalt = new byte[120 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value
                + Convert.ToBase64String(passwordSalt);

#pragma warning disable CS8604 // Possible null reference argument.
            byte[] passwordHash = _authHelper.GetPasswordHash(companyForRegistration.password, passwordSalt);
#pragma warning restore CS8604 // Possible null reference argument.

            // Check if TypeID exists, if not, insert it
            string sqlCheckTypeExists = $"SELECT TypeID FROM FinalProjCompanies.CompanyTypes WHERE TypeID={companyForRegistration.TypeID}";
            IEnumerable<int> existsType = _dapper.LoadData<int>(sqlCheckTypeExists);

            if (!existsType.Any())
            {
                string sqlAddType = $@"INSERT INTO FinalProjCompanies.CompanyTypes ([TypeID], [TypeName]) 
                              VALUES ({companyForRegistration.TypeID}, '{companyForRegistration.TypeName}')";
                _dapper.ExecuteSql(sqlAddType);
            }

            // Add company registration
            string sqlAddAuth = $@"INSERT INTO FinalProjCompanies.Registration 
                         ([Username], [Email], [PasswordHash], [PasswordSalt], 
                          [CompanyName], [ContactPersonName], [ContactEmail], 
                          [ContactPhone], [TypeID]) 
                          VALUES ('{companyForRegistration.Username}', 
                                  '{companyForRegistration.Email}', 
                                  @PasswordHash, @PasswordSalt, 
                                  '{companyForRegistration.CompanyName}', 
                                  '{companyForRegistration.ContactPersonName}', 
                                  '{companyForRegistration.ContactEmail}', 
                                  '{companyForRegistration.ContactPhone}', 
                                  {companyForRegistration.TypeID})";

            var sqlParameters = new List<SqlParameter>
                                {
                                    new SqlParameter("@PasswordHash", SqlDbType.VarBinary) { Value = passwordHash },
                                    new SqlParameter("@PasswordSalt", SqlDbType.VarBinary) { Value = passwordSalt }
                                };

            if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to register company");
        }

        [AllowAnonymous]
        [HttpPost("CompanyLogin")]
        public IActionResult CompanyLogin(CompanyForLogin companyForLogin)
        {
            string sqlForHashAndSalt = $"SELECT [PasswordHash], [PasswordSalt] FROM FinalProjCompanies.Registration WHERE ContactEmail = '{companyForLogin.Email}'";

            // Fetch the hash and salt
            CompanyForConformation companyForConformation;
            try
            {
                companyForConformation = _dapper.LoadDataSingle<CompanyForConformation>(sqlForHashAndSalt);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Email not found: " + companyForLogin.Email);
                return StatusCode(401, "Email not found");
            }

            // Verify the password
            byte[] passwordHash = _authHelper.GetPasswordHash(companyForLogin.Password, companyForConformation.PasswordSalt);
            if (!passwordHash.SequenceEqual(companyForConformation.PasswordHash))
            {
                Console.WriteLine("Incorrect password for email: " + companyForLogin.Email);
                return StatusCode(401, "Incorrect password");
            }

            // Fetch the registration ID
            string registrationIDSql = $"SELECT [RegistrationID] FROM FinalProjCompanies.Registration WHERE ContactEmail = '{companyForLogin.Email}'";
            int registrationID = _dapper.LoadDataSingle<int>(registrationIDSql);

            // Define claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, companyForLogin.Email),
        new Claim("RegistrationID", registrationID.ToString())
    };

            // Optionally add more claims here based on other company data if needed
            // e.g., new Claim("CompanyName", companyForConformation.CompanyName)

            // Create token with claims
            string token = _authHelper.CreateTokenWithClaims(claims);

            // Return the token with claims
            return Ok(new Dictionary<string, string>
    {
        { "token", token }
    });
        }

        [HttpGet("CompanyRefreshToken")]
        public IActionResult CompanyRefreshToken()
        {
            // Retrieve the RegistrationID claim from the ClaimsPrincipal (User)
            string? registrationID = User.FindFirst("RegistrationID")?.Value;

            if (string.IsNullOrEmpty(registrationID))
            {
                return StatusCode(401, "RegistrationID claim not found");
            }

            // Query to validate if the company still exists in the database
            string registrationIDSql = $"SELECT [RegistrationID] FROM FinalProjCompanies.Registration WHERE RegistrationID  = {registrationID}";
            int companyFromDB;

            try
            {
                companyFromDB = _dapper.LoadDataSingle<int>(registrationIDSql);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(404, "Company not found");
            }

            return Ok(new Dictionary<string, string>
                {
                    { "RegistrationID", companyFromDB.ToString() }
                });
        }
        

    }

}