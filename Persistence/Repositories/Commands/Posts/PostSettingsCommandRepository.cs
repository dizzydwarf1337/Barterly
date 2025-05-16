using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Posts
{
    public class PostSettingsCommandRepository :  BaseCommandRepository<BarterlyDbContext>, IPostSettingsCommandRepository
    {
        public PostSettingsCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task UpdatePostSettings(PostSettings postSettings)
        {
            _ = await _context.PostSettings.FindAsync(postSettings.Id) ?? throw new EntityNotFoundException("PostSettings");
            _context.PostSettings.Update(postSettings);
            await _context.SaveChangesAsync();
        }
    }
}
