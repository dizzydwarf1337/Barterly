using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Session
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public string? IP { get; set; }
        
        public DateTime Started { get; set; } = DateTime.UtcNow;
        
        public DateTime Expired { get; set; } = DateTime.UtcNow + TimeSpan.FromDays(3);
        
        public bool IsExpired { get; set; } = false;
        
        public virtual ICollection<SessionFavouritePost>? Favourites { get; set; }
    }
}
