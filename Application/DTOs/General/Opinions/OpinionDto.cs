using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.General.Opinions
{
    public class OpinionDto
    {
        public required string id { get; set; }
        public required string authorId { get; set; }
        public required string content { get; set; }
        public bool isHidden { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? lastUpdatedAt { get; set; }
        public int? rate { get; set; }
        public required string SubjectId { get; set; }
    }
}
