using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface IDeletedPostCommandRepository
    {
        Task CreateDeletedPostAsync(DeletedPost deletedPost);
        Task DeleteDeletedPostAsync(Guid id);

    }
}
