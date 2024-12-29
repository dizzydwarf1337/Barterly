using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FavouriteCategory
    {
        [Column(Order = 0)]
        public Guid UserId { get; set; }

        [Column(Order = 1)]
        public Guid CategoryId { get; set; }
        
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
    }
}
