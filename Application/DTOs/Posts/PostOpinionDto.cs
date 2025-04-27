using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Posts
{
    public class PostOpinionDto
    {
        public string Id { get; set; }

        public string AuthorId { get; set; }

        public string Content { get; set; }

        public bool IsHidden { get; set; } 

        public DateTime CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public int? Rate { get; set; } = null;
    }
}
