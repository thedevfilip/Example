using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Example.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Example.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        base.OnConfiguring(optionsBuilder);
}