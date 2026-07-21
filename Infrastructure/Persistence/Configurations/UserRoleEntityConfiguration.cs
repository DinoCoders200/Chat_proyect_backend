using custom_chat_backend.Core.Domain.Entities.ServerRole;
using custom_chat_backend.Core.Domain.Entities.User;
using custom_chat_backend.Core.Domain.Entities.UserRole;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(x => x.Id);

        // The same role must not be assigned to the same user twice.
        builder.HasIndex(r => new { r.UserId, r.ServerRolesId })
            .IsUnique();

        builder.HasOne<ServerRoleEntity>()
            .WithMany()
            .HasForeignKey(r => r.ServerRolesId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict: ServerRoles already cascades from Servers, so cascading from Users
        // as well would give Postgres two delete paths into this table.
        builder.HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
