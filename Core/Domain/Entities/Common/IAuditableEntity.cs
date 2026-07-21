namespace custom_chat_backend.Core.Domain.Entities.Common;

/// <summary>
/// Rows that track both creation and modification.
/// </summary>
public interface IAuditableEntity : ICreatableEntity
{
    DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Id of the user that last updated the row. Null when there was no authenticated user.
    /// </summary>
    Guid? UpdatedBy { get; set; }
}
