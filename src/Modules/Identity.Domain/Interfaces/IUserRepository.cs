using System;
using Identity.Domain.Entities;
using Marraia.MongoDb.Repositories.Interfaces;

namespace Identity.Domain.Interfaces;

public interface IUserRepository : IRepositoryBase<Users, Guid>
{
    Task<Users> GetByEmailAsync(string email);
}
