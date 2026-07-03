using custom_chat_backend.Core.Domain.Entities.Common;

namespace custom_chat_backend.Core.Domain.Entities.User;

public class UserEntity:AuditableEntity
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }=string.Empty;
    public string Password { get; private set; }=string.Empty;

    protected UserEntity() { }
    
    public UserEntity(string username, string password)
    {
        Id = Guid.NewGuid();
        Username = username;
        Password = password; 
    }
}