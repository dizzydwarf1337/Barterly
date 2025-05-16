using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Posts
{
    public class ImagesDto
    {
        [Required]
        public string PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        public IFormFile? MainImage { get; set; }
        public IFormFile[]? SecondaryImages { get; set; }
    }
}
