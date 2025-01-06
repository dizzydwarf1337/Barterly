using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class LogDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PostId { get; set; }
        public LogType LogType { get; set; }
    }
}
