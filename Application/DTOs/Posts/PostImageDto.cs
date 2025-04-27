using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Posts
{
    public class PostImageDto
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
    }
}
