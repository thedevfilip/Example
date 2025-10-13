using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Infrastructure.Configurations;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Token)
            .HasMaxLength(200);

        builder.Property(p => p.IpAddress)
            .HasMaxLength(45);

        builder.Property(p => p.UserAgent)
            .HasMaxLength(500);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.ExpiresAt)
            .IsRequired();

        builder.Property(p => p.IsRevoked)
            .HasDefaultValue(false);

        builder.HasIndex(p => p.Token)
            .IsUnique();

        builder.HasIndex(p => new { p.UserId, p.IsRevoked });

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .HasPrincipalKey(p => p.Id);
    }
}
