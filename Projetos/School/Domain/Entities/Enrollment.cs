namespace School.Domain.Entities;

public class Enrollment
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime EnrolledAt { get; set; }
    public decimal? Grade { get; set; }
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;

}