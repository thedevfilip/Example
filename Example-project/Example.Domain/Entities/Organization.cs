namespace Example.Domain.Entities;

public class Organization(Guid id, string name)
{
    public Guid Id { get; init; } = id;
    public string Name { get; set; } = name;
}