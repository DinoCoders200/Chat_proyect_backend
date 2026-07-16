namespace custom_chat_backend.Core.Domain.Entities.Common;

/// <summary>
/// Marks an entity whose rows are never physically removed. Deleting stamps
/// <see cref="DeletedAt"/> instead, so history and foreign keys stay intact.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// When the row was deleted. Null means the row is active.
    /// </summary>
    DateTime? DeletedAt { get; set; }
}
