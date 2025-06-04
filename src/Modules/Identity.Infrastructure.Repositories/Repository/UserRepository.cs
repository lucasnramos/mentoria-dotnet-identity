using System;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Marraia.MongoDb.Repositories;
using Marraia.MongoDb.Repositories.Interfaces;
using MongoDB.Driver;

namespace Identity.Infrastructure.Repositories.Repository;

public class UserRepository : MongoDbRepositoryBase<Users, Guid>, IUserRepository
{
    public UserRepository(IMongoContext context) : base(context)
    {
    }

    public Task<Users> GetByEmailAsync(string email)
    {
        return Collection
            .Find(x => x.Email == email)
            .FirstOrDefaultAsync();
    }
}
