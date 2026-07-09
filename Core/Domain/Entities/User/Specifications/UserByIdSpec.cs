using Ardalis.Specification;

namespace custom_chat_backend.Core.Domain.Entities.User.Specifications;

public class UserByIdSpec:Specification<UserEntity>
{
    public UserByIdSpec(Guid userId, bool asNoTracking = true)
    {
        var query= Query.Where(x => x.Id == userId);
        if (asNoTracking)
        {
            query.AsNoTracking();
        }
    }
}