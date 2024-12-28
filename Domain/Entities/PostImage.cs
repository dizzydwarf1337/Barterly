using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PostImage
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string ImageUrl { get; set; }
    }
}
