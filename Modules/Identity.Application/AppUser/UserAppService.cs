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
    private readonly ISmartNotification _notification = notification;

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
            _notification.NewNotificationBadRequest("Id cannot be empty.");
            return null;
        }

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            _notification.NewNotificationBadRequest($"User with Id {id} not found.");
            return null;
        }
        return user;
    }

    public async Task<Users> InsertAsync(UserInput userInput)
    {
        var hashedPassword = PasswordHasher.Hash(userInput.Password);
        var user = new Users(userInput.Name, userInput.Email, hashedPassword, userInput.Type);
        var isValidInput = user.IsValidUser(out string errorMessage);
        var hasUser = await _userRepository.GetByEmailAsync(userInput.Email);
        if (!isValidInput)
        {
            _notification.NewNotificationBadRequest($"Invalid user input: {errorMessage}");
            return null;
        }

        if (hasUser != null)
        {
            _notification.NewNotificationConflict($"User with email {userInput.Email} already exists.");
            return null;
        }
        await _userRepository.InsertAsync(user);
        return user;
    }

    public async Task<Users> UpdateAsync(Guid id, UserInput userInput)
    {
        if (id == Guid.Empty)
        {
            _notification.NewNotificationBadRequest("Id cannot be empty.");
            return null;
        }
        if (userInput == null)
        {
            _notification.NewNotificationBadRequest("User information is required.");
            return null;
        }

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            var newUser = await InsertAsync(userInput);
            return newUser;
        }

        user.Update(userInput.Name, userInput.Email, userInput.Type);
        await _userRepository.UpdateAsync(user);

        return user;
    }

    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _notification.NewNotificationBadRequest("Id is required.");
            return;
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
