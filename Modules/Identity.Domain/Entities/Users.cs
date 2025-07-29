using System;
using Marraia.MongoDb.Core;

namespace Identity.Domain.Entities;

public class Users : Entity<Guid>
{

    public Users(string name,
                string email,
                string password,
                string role)
    {
        Name = name;
        Email = email;
        Password = PasswordHasher.Hash(password);
        Role = role;
        Id = Guid.NewGuid();
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Role { get; private set; }

    public bool VerifyPassword(string hashedPassword)
    {
        return PasswordHasher.Verify(Password, hashedPassword);
    }

    public void Update(string name, string email, string role)
    {
        Name = name;
        Email = email;
        Role = role;
    }

    public bool IsValidUser(out string errorMessage)
    {
        if (string.IsNullOrEmpty(Name))
        {
            errorMessage = "Name cannot be null or empty.";
            return false;
        }
        if (string.IsNullOrEmpty(Email) || !IsValidEmail(Email))
        {
            errorMessage = $"{Email} is not a valid email address.";
            return false;
        }
        if (string.IsNullOrEmpty(Password))
        {
            errorMessage = "Password cannot be null or empty.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    public static bool IsValidEmail(string email)
    {
        return !string.IsNullOrEmpty(email) && email.Contains('@') && email.Contains('.');
    }
}
