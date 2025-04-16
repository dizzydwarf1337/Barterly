using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Posts
{
    public class PostImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PostId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }
}
