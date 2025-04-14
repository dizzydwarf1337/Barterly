using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum TokenType
    {
        None = 0,
        EmailConfirmation = 1,
        PasswordReset = 2,
        Auth = 3,
    }
}
