using Domain.Enums.Common;

namespace Application.DTOs.General
{
    public class ReportDto
    {
        public Guid Id;
        public string Message = "";
        public DateTime CreatedAt = DateTime.UtcNow;
        public ReportStatusType Status = ReportStatusType.Submitted;
        public Guid AuthorId;
        public Guid RepotedSubjectId;
    }
}
