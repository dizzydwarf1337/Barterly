using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } =  Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        
        [MaxLength(50)]
        public string? Description { get; set; }
        
        public virtual ICollection<SubCategory>? SubCategories { get; set; }
}
}
