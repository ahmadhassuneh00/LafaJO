using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class TokenHelper
{
    public static string? GetUserIdFromToken(string token, string secret)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(secret);

        try
        {
            // Token validation parameters (if you want to validate token)
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // You can adjust other parameters here based on your needs (e.g., token lifetime validation)
            };

            // Validate the token and extract the claims principal
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // Extract the user ID from the claims (assuming it's stored in the "nameid" claim)
            var userIdClaim = principal.FindFirst("UserId");

            return userIdClaim?.Value;
        }
        catch (Exception ex)
        {
            // Handle token validation failure
            Console.WriteLine("Token validation failed: " + ex.Message);
            return null; // Return null if token is invalid
        }
    }
     public static string? GetRegistrationIDFromToken(string token, string secret)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(secret);

        try
        {
            // Token validation parameters (if you want to validate token)
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // You can adjust other parameters here based on your needs (e.g., token lifetime validation)
            };

            // Validate the token and extract the claims principal
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // Extract the user ID from the claims (assuming it's stored in the "nameid" claim)
            var userIdClaim = principal.FindFirst("RegistrationID");

            return userIdClaim?.Value;
        }
        catch (Exception ex)
        {
            // Handle token validation failure
            Console.WriteLine("Token validation failed: " + ex.Message);
            return null; // Return null if token is invalid
        }
    }
}