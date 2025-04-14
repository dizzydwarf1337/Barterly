using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string NamePL { get; set; }
        public string NameEN { get; set; }
        public string? Description { get; set; }
        public SubCategoryDto?[] SubCateogries { get; set; }

    }
}
