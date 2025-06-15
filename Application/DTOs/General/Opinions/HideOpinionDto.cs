using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.General.Opinions
{
    public class HideOpinionDto
    {
        public required string OpinionId { get; set; }
        public bool isHidden { get; set; }
    }
}
