namespace Application.DTOs
{
    public class OpinionDto
    {
        public string id { get; set; }
        public string authorId { get; set; }
        public string content { get; set; }
        public bool isHidden { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? lastUpdatedAt { get; set; }
        public int? rate { get; set; }
        public string SubjectId { get; set; }
    }
}
