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
            if (!typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType)) continue;
            builder.Entity(entityType.ClrType)
                .Property(nameof(IAuditableEntity.CreatedAt))
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.Entity(entityType.ClrType)
                .Property(nameof(IAuditableEntity.CreatedBy))
                .HasMaxLength(256)
                .IsRequired(false);

            builder.Entity(entityType.ClrType)
                .Property(nameof(IAuditableEntity.UpdatedAt))
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            builder.Entity(entityType.ClrType)
                .Property(nameof(IAuditableEntity.UpdatedBy))
                .HasMaxLength(256)
                .IsRequired(false);
        }
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void OnBeforeSaving()
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Self-Registration";

        var entries = ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow;

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUserId;
                    break;
                case EntityState.Modified:
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUserId;
                    break;
            }
        }
    }
}