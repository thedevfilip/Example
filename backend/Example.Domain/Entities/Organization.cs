namespace Example.Domain.Entities;

public class Organization
{
    public Guid Id { get; init; }
    public string Name { get; private set; } = default!;

    public ICollection<UserOrganization> UserOrganizations { get; } = [];

    public static Organization Create(string name) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = name
        };
}
