using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SessionFavouritePost : FavouritePostBase
    {
        [Column(Order=1)]
        public Guid SessionId { get; set; }
        public virtual Session Session { get; set; }
    }
}
