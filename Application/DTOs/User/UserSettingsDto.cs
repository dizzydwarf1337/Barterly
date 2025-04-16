using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User
{
    public class UserSettingsDto
    {
        public bool IsHidden { get; set; }

        public bool IsBanned { get; set; }

        public bool IsPostRestricted { get; set; }

        public bool IsOpinionRestricted { get; set; }

        public bool IsChatRestricted { get; set; }
    }
}
