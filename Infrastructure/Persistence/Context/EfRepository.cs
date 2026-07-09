using Ardalis.Specification.EntityFrameworkCore;

namespace custom_chat_backend.Infrastructure.Persistence.Context;

public class EfRepository<T> : RepositoryBase<T> where T : class
{
    public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
    { }
}