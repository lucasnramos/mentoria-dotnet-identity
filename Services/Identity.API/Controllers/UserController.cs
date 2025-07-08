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
            var user = await _userAppService.GetByIdAsync(id);
            return OkOrNotFound(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserInput userInput)
        {

            var newUser = await _userAppService.InsertAsync(userInput);
            return CreatedContent("", newUser);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromQuery] Guid Id, [FromBody] UserInput userInput)
        {

            var user = await _userAppService.UpdateAsync(Id, userInput);
            return AcceptedOrContent(user);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync([FromQuery] Guid Id)
        {

            await _userAppService.DeleteAsync(Id);
            return NoContent();
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
