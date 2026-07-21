namespace custom_chat_backend.Core.Domain.Entities.Common;

/// <summary>
/// Rows that record when and by whom they were created, but are never modified
/// afterwards (append-only tables such as login_logs).
/// </summary>
public interface ICreatableEntity
{
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// Id of the user that created the row. Null when there was no authenticated user
    /// (self-registration, seeding, or background jobs).
    /// </summary>
    Guid? CreatedBy { get; set; }
}
