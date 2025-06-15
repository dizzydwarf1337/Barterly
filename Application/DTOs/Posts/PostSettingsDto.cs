using Domain.Enums.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Posts
{
    public class PostSettingsDto
    {
        public required string Id {  get; set; }
        public bool IsHidden {  get; set; }
        public bool IsDeleted { get; set; }
        public PostStatusType PostStatusType { get; set; }
        public string? RejectionMessage {  get; set; }
    }
}
