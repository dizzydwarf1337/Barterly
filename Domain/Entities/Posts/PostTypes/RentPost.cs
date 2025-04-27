using Domain.Enums.Posts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Posts.PostTypes
{
    public  class RentPost : Post
    {
        public RentObjectType RentObjectType { get; set; }
        public int? NumberOfRooms { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal? Area { get; set; }

        public int? Floor { get; set; }


    }
}
