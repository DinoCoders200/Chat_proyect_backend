using System.Reflection;
using System.Security.Claims;
using custom_chat_backend.Core.Domain.Entities.Common;
using custom_chat_backend.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace custom_chat_backend.Infrastructure.Persistence.Context;

public class ApplicationDbContext: DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public DbSet<UserEntity> Users => Set<UserEntity>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ICreatableEntity).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(ICreatableEntity.CreatedAt))
                    .HasColumnType("timestamp with time zone")
                    .IsRequired();

                builder.Entity(entityType.ClrType)
                    .Property(nameof(ICreatableEntity.CreatedBy))
                    .IsRequired(false);
            }

            if (typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IAuditableEntity.UpdatedAt))
                    .HasColumnType("timestamp with time zone")
                    .IsRequired(false);

                builder.Entity(entityType.ClrType)
                    .Property(nameof(IAuditableEntity.UpdatedBy))
                    .IsRequired(false);
            }

            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(ISoftDeletable.DeletedAt))
                    .HasColumnType("timestamp with time zone")
                    .IsRequired(false);
            }
        }
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private Guid? GetCurrentUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(claim, out var userId) ? userId : null;
    }

    private void OnBeforeSaving()
    {
        var currentUserId = GetCurrentUserId();
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<ICreatableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUserId;
                    break;
                case EntityState.Modified:
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                    break;
            }
        }

        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State != EntityState.Modified) continue;

            entry.Entity.UpdatedAt = now;
            entry.Entity.UpdatedBy = currentUserId;
        }
    }
}