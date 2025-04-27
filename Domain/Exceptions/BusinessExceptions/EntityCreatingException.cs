using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BusinessExceptions
{
    public class EntityCreatingException : Exception
    {
        public EntityCreatingException(string? entity, string? place) : base($"Error creating new {entity} in {place}")
        {
        }
    }
}
