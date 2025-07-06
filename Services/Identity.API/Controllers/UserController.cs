using Authentication.Adapter.Configurations;
using System.Linq.Expressions;
using Identity.Application.AppUser.Input;
using Identity.Application.AppUser.Interfaces;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Authentication.Adapter.Token;
using Marraia.Notifications.Base;
using MediatR;
using Marraia.Notifications.Models;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserAppService _userAppService;
        private readonly IConfiguration _configuration;
        public UserController(IUserAppService userAppService, IConfiguration configuration, INotificationHandler<DomainNotification> notification) : base(notification)
        {
            _userAppService = userAppService;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userAppService.GetAllAsync();
            return OkOrNotFound(users);
        }

        [Authorize]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var userByEmail = await _userAppService.GetByEmailAsync(email);
            return OkOrNotFound(userByEmail);
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllUsersAsync()
        {
            await _userAppService.DeleteAllAsync();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<object> Login([FromBody] UserInput input,
                                        [FromServices] SigningConfigurations signingConfigurations)
        {
            var user = await _userAppService.LoginAsync(input.Email, input.Password);

            if (user == null)
            {
                return BadRequest("Usuário não autenticado");
            }

            var tokenIssuer = _configuration.GetSection("TokenConfigurations:Issuer").Value;
            var tokenAudience = _configuration.GetSection("TokenConfigurations:Audience").Value;

            var token = GenerateToken.GetToken(user.Id, user.Email, user.Type, tokenIssuer, tokenAudience, signingConfigurations);

            return token;
        }
    }
}
