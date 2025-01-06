using Domain.Entities;
using Domain.Interfaces.Commands.Post;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Post
{
    public class PostOpinionCommandRepository : BaseCommandRepository<BarterlyDbContext>, IPostOpinionCommandRepository
    {
        public PostOpinionCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreatePostOpinionAsync(PostOpinion opinion)
        {
            await _context.PostOpinions.AddAsync(opinion);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostOpinionAsync(Guid id)
        {
            var opinion = await _context.Opinions.FindAsync(id) ?? throw new Exception("PostOpinion not found");
            _context.Remove(opinion);
            await _context.SaveChangesAsync();
        }

        public async Task SetHiddenPostOpinionAsync(Guid id, bool value)
        {
            var opinion = await _context.PostOpinions.FindAsync(id) ?? throw new Exception("PostOpinion not found");
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
