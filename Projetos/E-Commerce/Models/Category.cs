namespace Category.Models;

public class Category
{
    public Category(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}