using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BusinessExceptions
{
    public class AccessForbiddenException : Exception
    {
        public string? UserId { get; }
        public string? Reason { get; }

        public AccessForbiddenException(string? place, string? userId ,string? reason)
            : base($"Access forbidden for {userId} in {place}")
        {
            UserId = userId;
            Reason = reason;
        }
    }
}
