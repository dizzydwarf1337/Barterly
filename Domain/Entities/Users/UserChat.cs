using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class UserChat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
