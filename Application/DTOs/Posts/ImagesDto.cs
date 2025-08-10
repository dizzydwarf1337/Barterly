using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Posts;

public class ImagesDto
{
    [Required] public required Guid PostId { get; set; }
    public IFormFile? MainImage { get; set; }
    public IFormFile[]? SecondaryImages { get; set; }
}