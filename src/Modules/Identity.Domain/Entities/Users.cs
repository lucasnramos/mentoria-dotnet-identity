using System;
using Marraia.MongoDb.Core;

namespace Identity.Domain.Entities;

public class Users : Entity<Guid>
{

    public Users(string name,
                string email,
                string password,
                int type)
    {
        Name = name;
        Email = email;
        Password = password;
        Type = type;
        Id = Guid.NewGuid(); // Ensure Id is set to a new Guid
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public int Type { get; private set; }

    public bool VerifyPassword(string password)
    {
        // Implement password verification logic here
        // For example, compare the hashed password with the stored hash
        return Password == password; // Simplified for demonstration purposes
    }
}
