using Domain.Entities;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Post
{
    public class PostOpinionRepository : BaseRepository, IPostOpinionCommandRepository, IPostOpinionQueryRepository
    {
        public PostOpinionRepository(BarterlyDbContext context) : base(context) { }
        public async Task CreatePostOpinionAsync(PostOpinion opinion)
        {
            await _context.PostOpinions.AddAsync(opinion);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostOpinionAsync(Guid id)
        {
            var opinion = await GetPostOpinionByIdAsync(id);
            _context.Remove(opinion);
            await _context.SaveChangesAsync();
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

        public async Task SetHiddenPostOpinionAsync(Guid id, bool value)
        {
            var opinion = await GetPostOpinionByIdAsync(id);
            opinion.IsHidden = value;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostOpinionAsync(PostOpinion opinion)
        {
            _context.PostOpinions.Update(opinion);
            await _context.SaveChangesAsync();
        }

    }
}
