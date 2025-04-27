using Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IPostSettingsQueryRepository
    {
        public Task<PostSettings> GetPostSettingsById(Guid settingsId);
        public Task<PostSettings> GetPostSettingsByPostId(Guid postId);
    }
}
