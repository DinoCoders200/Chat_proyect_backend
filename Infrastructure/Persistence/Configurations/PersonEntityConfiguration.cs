using custom_chat_backend.Core.Domain.Entities.Person;
using custom_chat_backend.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class PersonEntityConfiguration : IEntityTypeConfiguration<PersonEntity>
{
    public void Configure(EntityTypeBuilder<PersonEntity> builder)
    {
        builder.ToTable("People");
        builder.HasKey(x => x.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.BirthDate)
            .HasColumnType("date");

        builder.Property(p => p.Phone)
            .HasMaxLength(30);

        builder.Property(p => p.AvatarUrl)
            .HasMaxLength(2048);

        builder.Property(p => p.Bio)
            .HasMaxLength(500);

        builder.HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
