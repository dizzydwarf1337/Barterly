using Domain.Entities.Categories;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users
{
    public class FavouriteCategory
    {
        [Column(Order = 0)]
        public Guid UserId { get; set; }

        [Column(Order = 1)]
        public Guid CategoryId { get; set; }

        public virtual User User { get; set; } = default!;
        public virtual Category Category { get; set; } = default!;
    }
}
