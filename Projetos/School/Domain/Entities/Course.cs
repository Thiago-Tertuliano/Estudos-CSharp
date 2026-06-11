namespace School.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int MaxStudents { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}