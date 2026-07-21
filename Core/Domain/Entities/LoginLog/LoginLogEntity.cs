using custom_chat_backend.Core.Domain.Entities.Common;

namespace custom_chat_backend.Core.Domain.Entities.LoginLog;

/// <summary>
/// Append-only record of authentication attempts. Extends <see cref="CreatableEntity"/>
/// rather than <see cref="AuditableEntity"/> because log rows are never updated or deleted.
/// </summary>
public class LoginLogEntity : CreatableEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Provider { get; private set; } = string.Empty;
    public string EventType { get; private set; } = string.Empty;
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public string? DeviceName { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public bool Success { get; private set; }

    /// <summary>
    /// Null when <see cref="Success"/> is true.
    /// </summary>
    public string? FailureReason { get; private set; }

    protected LoginLogEntity() { }

    public LoginLogEntity(Guid userId, string provider, string eventType, bool success)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Provider = provider;
        EventType = eventType;
        Success = success;
    }
}
