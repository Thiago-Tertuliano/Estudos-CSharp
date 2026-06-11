using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Interfaces;

namespace School.Application.Courses.Commands.AssignGrade;

public class AssignGradeCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AssignGradeCommand>
{
    public async Task Handle(AssignGradeCommand request, CancellationToken ct)
    {
        var enrollment = await context.Enrollments
            .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, ct)
            ?? throw new NotFoundException("Enrollment not found.");

        if (enrollment.Grade.HasValue)
            throw new InvalidOperationException("Grade already assigned.");

        enrollment.Grade = request.Grade;
        await context.SaveChangesAsync(ct);
    }
}