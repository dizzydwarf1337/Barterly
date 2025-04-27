using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BusinessExceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entity) : base($"{entity} not found")
        {
        }
    }
}
