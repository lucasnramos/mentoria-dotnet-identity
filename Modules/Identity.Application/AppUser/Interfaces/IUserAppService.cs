using System;
using Identity.Application.AppUser.Input;
using Identity.Domain.Entities;

namespace Identity.Application.AppUser.Interfaces;

public interface IUserAppService
{
    Task<Users> InsertAsync(UserInput userInput);
    Task<IEnumerable<Users>> GetAllAsync();
    Task<Users> GetByEmailAsync(string email);
    Task<Users> GetByIdAsync(Guid id);
    Task<Users> UpdateAsync(Guid id, UserInput userInput);
    Task DeleteAsync(Guid id);
    Task DeleteAllAsync();
    Task<Users> LoginAsync(string login, string password);
}
