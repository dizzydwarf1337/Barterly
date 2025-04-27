using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.ExternalServicesExceptions
{
    public class ExternalServiceException : Exception
    {
        public ExternalServiceException(string? message) : base(message)
        {
        }
    }
}
