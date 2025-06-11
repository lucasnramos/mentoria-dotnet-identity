using System;
using Identity.Application.AppUser.Input;

namespace Identity.Application.AppUser.Interfaces;

public interface IUserAppService
{
    Task InsertAsync(UserInput userInput);
}
