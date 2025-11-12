using System.Linq.Expressions;
using Example.Domain.Contexts;
using Example.Domain.Entities;
using Example.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;

namespace Example.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options, IOrganizationContext organizationContext)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ConfigureTenantFilter(builder);
    }

    private void ConfigureTenantFilter(ModelBuilder builder)
    {
        foreach (IMutableEntityType? entityType in builder.Model.GetEntityTypes()
                     .Where(p => typeof(IMustHaveTenant).IsAssignableFrom(p.ClrType)))
        {
            builder.Entity(entityType.ClrType)
                .AddQueryFilter<IMustHaveTenant>(p => p.OrganizationId == organizationContext.OrganizationId);
        }
    }
}

internal static class QueryFilterExtensions
{
    internal static void AddQueryFilter<TInterface>(
        this EntityTypeBuilder builder,
        Expression<Func<TInterface, bool>> filter)
    {
        ParameterExpression newParam = Expression.Parameter(builder.Metadata.ClrType);
        Expression newBody = ReplacingExpressionVisitor.Replace(filter.Parameters.Single(), newParam, filter.Body);
        builder.HasQueryFilter(Expression.Lambda(newBody, newParam));
    }
}
