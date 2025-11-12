namespace Example.Domain.Primitives;

public interface IMustHaveTenant
{
    Guid OrganizationId { get; init; }
}
