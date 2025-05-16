using Application.DTOs.Posts;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPostSettingsService
    {
        public Task ApprovePost(ApprovePostDto approvePostDto);
        public Task DeletePost(Guid postId);
        public Task RejectPost(RejectPostDto rejectPostDto);
        public Task SetPostHidden(Guid postId, bool value);
        public Task ResubmitPost(Guid postId);
    }
}
