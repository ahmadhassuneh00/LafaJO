using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjAPI.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly interfaceRepastory _userRepository;

        public UsersController(interfaceRepastory userRepository) // Removed IMapper
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetUsers")]
        public ActionResult<IEnumerable<Users>> GetUsers()  // Updated ActionResult return type
        {
            var users = _userRepository.GetUsers(); // Call the method
            if (users == null || !users.Any()) // Check if users are null or empty
            {
                throw new Exception("Failed to get users"); // Change exception message
            }
            return Ok(users); // Return the users as Ok response
        }

        [HttpGet("GetSingleUser/{userId}")]
        public async Task<ActionResult<UsersDto>> GetSingleUser(int userId) // Made async
        {
            var user = await _userRepository.GetSingleUserAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Manually create UsersDto from Users model
            var userDto = new UsersDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            return Ok(userDto);
        }

        [HttpGet("GetUserName/{userId}")]
        public async Task<ActionResult<string>> GetUserNameByUserId(int userId) // Made async
        {
            var userName = await _userRepository.GetUserNameByUserIdAsync(userId);
            if (userName == null)
            {
                return NotFound();
            }
            return Ok(userName);
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser(UsersDto userDto) // Use DTO for input
        {
            var userDb = await _userRepository.GetSingleUserAsync(userDto.UserId);
            if (userDb == null)
            {
                return NotFound();
            }

            // Update userDb properties manually
            userDb.FirstName = userDto.FirstName;
            userDb.LastName = userDto.LastName;
            userDb.Email = userDto.Email;

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            return StatusCode(500, "Failed to edit user");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId) // Made async
        {
            var userDb = await _userRepository.GetSingleUserAsync(userId); // Get user from repo
            if (userDb == null)
            {
                return NotFound();
            }

            _userRepository.RemoveEntitys(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            return StatusCode(500, "Failed to delete user");
        }
    }
}
