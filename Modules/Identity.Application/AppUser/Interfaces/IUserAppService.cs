using System;
using Identity.Application.AppUser.Input;
using Identity.Domain.Entities;

namespace Identity.Application.AppUser.Interfaces;

public interface IUserAppService
{
    Task<Users> InsertAsync(UserInput userInput);
    Task<IEnumerable<Users>> GetAllAsync();
    Task<Users> GetUserByEmailAsync(string email);
}
