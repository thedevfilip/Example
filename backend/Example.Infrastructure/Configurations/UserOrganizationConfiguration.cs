using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Infrastructure.Configurations;

public class UserOrganizationConfiguration : IEntityTypeConfiguration<UserOrganization>
{
    public void Configure(EntityTypeBuilder<UserOrganization> builder)
    {
        builder.HasKey(p => new { p.UserId, p.OrganizationId });

        builder.HasOne(p => p.User)
            .WithMany(p => p.UserOrganizations)
            .HasForeignKey(p => p.UserId);

        builder.HasOne(p => p.Organization)
            .WithMany(p => p.UserOrganizations)
            .HasForeignKey(p => p.OrganizationId);

        builder.HasOne(p => p.Role)
            .WithMany()
            .HasForeignKey(p => p.RoleId);
    }
}
