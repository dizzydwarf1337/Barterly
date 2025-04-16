using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Categories
{
    public class SubCategoryDto
    {
        public Guid Id { get; set; }
        public string TitlePL { get; set; }
        public string TitleEN { get; set; }
        public Guid CategoryId { get; set; }
    }
}
