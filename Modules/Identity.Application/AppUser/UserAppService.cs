using System;
using Identity.Application.AppUser.Input;
using Identity.Application.AppUser.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Application.AppUser;

public class UserAppService : IUserAppService
{
    private readonly IUserRepository _userRepository;
    public UserAppService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task InsertAsync(UserInput userInput)
    {
        var user = new Users(userInput.Name, userInput.Email, userInput.Password, userInput.Type);
        await _userRepository.InsertAsync(user);
    }
}
