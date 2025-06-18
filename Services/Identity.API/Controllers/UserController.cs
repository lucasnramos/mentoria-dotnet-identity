using Identity.Application.AppUser.Input;
using Identity.Application.AppUser.Interfaces;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userAppService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetUserByEmailAsync([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required.");
            }

            var userByEmail = await _userAppService.GetByEmailAsync(email);
            return Ok(userByEmail);
        }


        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserInput userInput)
        {
            var existingUser = await _userAppService.GetByEmailAsync(userInput.Email);
            if (existingUser != null)
            {
                return Conflict("User with this email already exists.");
            }
            var newUser = await _userAppService.InsertAsync(userInput);
            return CreatedAtAction(nameof(AddUserAsync), newUser);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromQuery] Guid Id, [FromBody] UserInput userInput)
        {
            if (userInput == null || Id == Guid.Empty)
            {
                return BadRequest("Invalid user data."); // needs better error handling and messages
            }
            var user = await _userAppService.GetByIdAsync(Id);

            return Ok();
        }
    }
}
