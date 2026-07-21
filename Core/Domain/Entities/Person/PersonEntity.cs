using custom_chat_backend.Core.Domain.Entities.Common;

namespace custom_chat_backend.Core.Domain.Entities.Person;

public class PersonEntity : AuditableEntity, ISoftDeletable
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateOnly? BirthDate { get; private set; }
    public string? Phone { get; private set; }
    public string? AvatarUrl { get; private set; }
    public string? Bio { get; private set; }
    public DateTime? DeletedAt { get; set; }

    protected PersonEntity() { }

    public PersonEntity(Guid userId, string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }
}
