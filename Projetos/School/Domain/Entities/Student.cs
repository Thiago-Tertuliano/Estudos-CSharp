namespace School.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; } 
    public DateTime EnrolledAt { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}