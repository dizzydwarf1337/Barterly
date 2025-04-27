using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Posts
{
    public class PostSettingsQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostSettingsQueryRepository
    {
        public PostSettingsQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<PostSettings> GetPostSettingsById(Guid settingsId)
        {
            var settings = await _context.PostSettings.FindAsync(settingsId) ?? throw new EntityNotFoundException("Post settings");
            return settings;
        }

        public async Task<PostSettings> GetPostSettingsByPostId(Guid postId)
        {
            var settings = await _context.PostSettings.FirstOrDefaultAsync(x=>x.PostId == postId) ?? throw new EntityNotFoundException("Post settings");
            return settings;
        }
    }
}
