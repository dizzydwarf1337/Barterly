namespace Application.DTOs.Reports;

public class ReportDto
{
    public required string Id { get; set; }
    public required string AuthorId { get; set; }
    public required string SubjectId { get; set; }
    public required string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}