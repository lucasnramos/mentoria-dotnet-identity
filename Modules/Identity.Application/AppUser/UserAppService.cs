using System;
using System.IO.Pipes;
using Identity.Application.AppUser.Input;
using Identity.Application.AppUser.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Application.AppUser;

public class UserAppService(IUserRepository userRepository) : IUserAppService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<Users>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users;
    }

    public async Task<Users> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user;
    }

    public async Task InsertAsync(UserInput userInput)
    {
        var user = new Users(userInput.Name, userInput.Email, userInput.Password, userInput.Type);
        await _userRepository.InsertAsync(user);
    }

}
