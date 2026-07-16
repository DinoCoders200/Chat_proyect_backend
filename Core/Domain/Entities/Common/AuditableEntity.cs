namespace custom_chat_backend.Core.Domain.Entities.Common;

public abstract class AuditableEntity : CreatableEntity, IAuditableEntity
{
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}
