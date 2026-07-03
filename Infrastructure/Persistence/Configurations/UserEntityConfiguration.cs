using custom_chat_backend.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class UserEntityConfiguration:IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(u => u.Username)
            .IsUnique();
        
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);
    }
}