using Domain.Entities;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Post
{
    public class PostOpinionQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostOpinionQueryRepository
    {
        public PostOpinionQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<PostOpinion> GetPostOpinionByIdAsync(Guid id)
        {
            return await _context.PostOpinions.FindAsync(id) ?? throw new Exception("Post opinion not found");
        }

        public async Task<ICollection<PostOpinion>> GetPostOpinionsAsync()
        {
            return await _context.PostOpinions.ToListAsync();
        }

        public async Task<ICollection<PostOpinion>> GetPostOpinionsByAuthorIdAsync(Guid userId)
        {
            return await _context.PostOpinions.Where(x => x.AuthorId == userId).ToListAsync();
        }

        public Task<ICollection<PostOpinion>> GetPostOpinionsByPostIdAsync(Guid postId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<PostOpinion>> GetPostOpinionsPaginatedAsync(int page, int pageSize)
        {
            return await _context.PostOpinions.Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

    }
}
