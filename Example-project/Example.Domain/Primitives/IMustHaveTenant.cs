namespace Example.Domain.Primitives;

public interface IMustHaveTenant
{
    public Guid OrganizationId { get; init; }
}
