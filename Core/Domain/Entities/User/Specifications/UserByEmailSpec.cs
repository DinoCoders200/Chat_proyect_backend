using Ardalis.Specification;

namespace custom_chat_backend.Core.Domain.Entities.User.Specifications;

public class UserByEmailSpec : Specification<UserEntity>
{
    public UserByEmailSpec(
        string email,
        bool asNoTracking = true)
    {
        var query = Query.Where(x =>
            x.Email == email);

        if (asNoTracking)
        {
            query.AsNoTracking();
        }
    }
}