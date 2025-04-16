namespace Application.DTOs
{
    public class OpinionDto
    {
        public Guid id { get; set; }
        public Guid authorId { get; set; }
        public string content { get; set; }
        public bool isHidden { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? lastUpdatedAt { get; set; }
        public int? rate { get; set; }
        public Guid SubjectId { get; set; }
    }
}
