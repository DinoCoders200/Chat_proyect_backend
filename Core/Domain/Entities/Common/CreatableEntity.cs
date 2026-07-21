namespace custom_chat_backend.Core.Domain.Entities.Common;

public abstract class CreatableEntity : ICreatableEntity
{
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
}
