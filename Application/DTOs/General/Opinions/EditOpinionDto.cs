using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.General.Opinions
{
    public class EditOpinionDto
    {
        public required string Id { get; set; }
        public required string Content { get; set; }
        public required string AuthorId { get; set; }
        public required string OpinionType { get; set; }
        [Range(1,5)]
        public int Rate { get; set; }
    }
}
