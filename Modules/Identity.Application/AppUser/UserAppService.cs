using System;
using System.IO.Pipes;
using Authentication.Adapter;
using Identity.Application.AppUser.Input;
using Identity.Application.AppUser.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Marraia.Notifications.Interfaces;

namespace Identity.Application.AppUser;

public class UserAppService(IUserRepository userRepository, IHttpContextAccessor accessor, ISmartNotification notification) : IUserAppService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IHttpContextAccessor _accessor = accessor;
    private readonly ISmartNotification _notification;

    public async Task<IEnumerable<Users>> GetAllAsync()
    {
        var loggedUser = Logged.GetUserLogged(_accessor);
        var users = await _userRepository.GetAllAsync();
        return users;
    }

    public async Task<Users> GetByEmailAsync(string email)
    {
        var isValidEmail = Users.IsValidEmail(email);
        if (!isValidEmail)
        {
            _notification.NewNotificationBadRequest("Invalid email format.");
            return null;
        }
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            _notification.NewNotificationBadRequest($"User with email {email} not found.");
            return null;
        }

        return user;
    }

    public async Task<Users> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with Id {id} not found.");
        }
        return user;
    }

    public async Task<Users> InsertAsync(UserInput userInput)
    {
        var user = new Users(userInput.Name, userInput.Email, userInput.Password, userInput.Type);
        var isValidInput = user.IsValidUser(out string errorMessage);
        var hasUser = await _userRepository.GetByEmailAsync(userInput.Email);
        if (!isValidInput)
        {
            throw new ArgumentException($"Invalid user input: {errorMessage}");
        }

        if (hasUser != null)
        {
            throw new ArgumentException($"User with email {userInput.Email} already exists.");
        }
        await _userRepository.InsertAsync(user);
        return user;
    }

    public async Task<Users> UpdateAsync(Guid id, UserInput userInput)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id is required"); // needs better error handling and messages
        }
        if (userInput == null)
        {
            throw new ArgumentException("User information is required");
        }

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            var newUser = await InsertAsync(userInput);
            return newUser;
        }

        user.Update(userInput.Name, userInput.Email, userInput.Password, userInput.Type);
        await _userRepository.UpdateAsync(user);

        return user;
    }

    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id is required");
        }
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

    public async Task<Users> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user != null)
        {
            var isValidPassword = PasswordHasher.Verify(password, user.Password);

            if (isValidPassword)
            {
                return user;
            }
        }

        return null;
    }
}
