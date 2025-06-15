using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Posts
{
    public class RejectPostDto
    {
        public required string postId { get; set; }
        public required string reason { get; set; }
    }
}
