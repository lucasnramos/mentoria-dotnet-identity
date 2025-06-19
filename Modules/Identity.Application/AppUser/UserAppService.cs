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

    public async Task<Users> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user;
    }

    public async Task<Users> InsertAsync(UserInput userInput)
    {
        var user = new Users(userInput.Name, userInput.Email, userInput.Password, userInput.Type);
        await _userRepository.InsertAsync(user);
        return user;
    }

    public async Task<Users> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user;
    }

    public async Task<Users> UpdateAsync(Guid id, UserInput userInput)
    {
        var user = await _userRepository.GetByIdAsync(id);
        user.Update(userInput.Name, userInput.Email, userInput.Password, userInput.Type);
        await _userRepository.UpdateAsync(user);
        return user;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public async Task DeleteAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        if (users.Any())
        {
            foreach (var user in users)
            {
                await _userRepository.DeleteAsync(user.Id);
            }
        }
    }

}
