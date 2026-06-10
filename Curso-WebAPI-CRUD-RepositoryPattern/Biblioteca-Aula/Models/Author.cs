namespace Author.Models;

public class Author
{
    public Author(string name, int age, string gender)
    {
        Id = Guid.NewGuid();
        Name = name;
        Age = age;
        Gender = gender;
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }
    public int Age { get; private set; }
    public string Gender { get; private set; }

    public void Update(string name, int age, string gender)
    {
        Name = name;
        Age = age;
        Gender = gender;
    }
}
