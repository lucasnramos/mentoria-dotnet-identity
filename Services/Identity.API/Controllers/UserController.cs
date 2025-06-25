using System.Linq.Expressions;
using HttpHandler.Base;
using HttpHandler.Handlers;
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
    public class UserController(MessageHandler messageHandler, IUserAppService userAppService) : HttpHandlerControllerBase(messageHandler)
    {
        private readonly IUserAppService _userAppService = userAppService;

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userAppService.GetAllAsync();
            return HandleOkOrNotFound(users);
            // return Ok(users);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            try
            {
                var userByEmail = await _userAppService.GetByEmailAsync(email);
                return Ok(userByEmail);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _userAppService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserInput userInput)
        {
            try
            {
                var newUser = await _userAppService.InsertAsync(userInput);
                var id = newUser.Id;
                // CreatedAtAction is returning System.InvalidOperationException: No route matches the supplied values.
                // return CreatedAtAction(nameof(GetUserByIdAsync), new { id }, newUser);
                return Created($"api/user/{id}", newUser);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromQuery] Guid Id, [FromBody] UserInput userInput)
        {
            try
            {
                var user = await _userAppService.UpdateAsync(Id, userInput);
                return Accepted(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync([FromQuery] Guid Id)
        {
            try
            {
                await _userAppService.DeleteAsync(Id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllUsersAsync()
        {
            await _userAppService.DeleteAllAsync();
            return NoContent();
        }
    }
}
