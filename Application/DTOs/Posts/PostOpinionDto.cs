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
        [Required]
        public string Id { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        [StringLength(300,MinimumLength =2,ErrorMessage ="Content length must be between 2 and 300")]
        public string Content { get; set; }

        public bool IsHidden { get; set; } 

        public DateTime CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public int? Rate { get; set; } = null;
    }
}
