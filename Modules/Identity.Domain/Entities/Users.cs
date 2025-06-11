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
        Id = Guid.NewGuid();
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public int Type { get; private set; }

    public bool VerifyPassword(string password)
    {
        return Password == password;
    }
}
