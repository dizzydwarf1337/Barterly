using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums.Posts;

namespace Domain.Entities.Posts.PostTypes;

public class RentPost : Post
{
    public RentObjectType RentObjectType { get; set; }
    public int? NumberOfRooms { get; set; }

    [Column(TypeName = "decimal(8,2)")] public decimal? Area { get; set; }

    public int? Floor { get; set; }
}