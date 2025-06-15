using Domain.Entities.Posts;
using Domain.Enums.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPostSettingsCommandRepository
    {
        public Task UpdatePostSettings(Guid postId, bool? isHidden, bool? isDeleted, PostStatusType? postStatusType, string? rejectionMessage);
    }
}
