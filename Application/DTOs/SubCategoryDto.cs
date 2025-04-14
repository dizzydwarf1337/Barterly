using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SubCategoryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CategoryId {  get; set; }
    }
}
