    using Application.Interfaces;
    using Domain.Entities;
    using Domain.Interfaces.Commands.Post;
    using Domain.Interfaces.Queries.Post;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Application.Services
    {
        public class VisitingPostService : IVisitingPostService
        {
            private readonly IVisitedPostCommandRepository _visitedPostCommandRepository;
            private readonly IVisitedPostQueryRepository _visitedPostQueryRepository;

            public VisitingPostService(IVisitedPostCommandRepository visitedPostCommandRepository, IVisitedPostQueryRepository visitedPostQueryRepository)
            {
                _visitedPostCommandRepository = visitedPostCommandRepository;
                _visitedPostQueryRepository = visitedPostQueryRepository;
            }

            public async Task<VisitedPost> GetVisitedPostByIdAsync(Guid id)
            {
                return await _visitedPostQueryRepository.GetVisitedPostByIdAsync(id);
            }

            public async Task<ICollection<VisitedPost>> GetVisitedPostsByPostIdAsync(Guid postId)
            {
                return await _visitedPostQueryRepository.GetVisitedPostsByPostIdAsync(postId);
            }

            public async Task<ICollection<VisitedPost>> GetVisitedPostsByUserIdAsync(Guid userId)
            {
                return await _visitedPostQueryRepository.GetVisitedPostsByUserIdAsync(userId);
            }

            public async Task VisitPost(Guid postId, Guid userId)
            {
                var post = await _visitedPostQueryRepository.GetUserVisitedPost(postId,userId);
                if (post != null)
                {
                    post.VisitedCount++;
                    post.LastVisitedAt = DateTime.Now;
                    await _visitedPostCommandRepository.UpdateVisitedPost(post);
                }
                else
                {
                    post = new VisitedPost
                    {
                        PostId = postId,
                        UserId = userId,
                        VisitedCount = 1,
                        LastVisitedAt = DateTime.Now
                    };
                    await _visitedPostCommandRepository.CreateVisitedPostAsync(post);
                }
            }
        }
    }
