using System;

namespace Identity.Application.AppUser.Input;

public class UserInput
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required int Type { get; set; }
}
