using Domain.Entities;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.User
{
    public class UserOpinionRepository : BaseRepository, IUserOpinionCommandRepository, IUserOpinionQueryRepository
    {
        public UserOpinionRepository(BarterlyDbContext context) : base(context){}

        public async Task CreateUserOpinionAsync(UserOpinion userOpinion)
        {
            await _context.UserOpinions.AddAsync(userOpinion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserOpinionAsync(Guid id)
        {
            var UserOpinion = await GetUserOpinionByIdAsync(id);
            _context.UserOpinions.Remove(UserOpinion);
            await _context.SaveChangesAsync();
        }

        public async Task<UserOpinion> GetUserOpinionByIdAsync(Guid id)
        {
            return await _context.UserOpinions.FindAsync(id) ?? throw new Exception("User opinion not found");
        }

        public async Task<ICollection<UserOpinion>> GetUserOpinionsAsync()
        {
            return await _context.UserOpinions.ToListAsync();
        }

        public async Task<ICollection<UserOpinion>> GetUserOpinionsByAuthorIdAsync(Guid authorId)
        {
            return await _context.UserOpinions.Where(x => x.AuthorId == authorId).ToListAsync();
        }

        public async Task<ICollection<UserOpinion>> GetUserOpinionsByUserIdAsync(Guid userId)
        {
            return await _context.UserOpinions.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<UserOpinion>> GetUserOpinionsPaginated(int page, int pageSize)
        {
           return await _context.UserOpinions.Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task SetHiddenUserOpinionAsync(Guid id, bool IsHidden)
        {
            var UserOpinion = await GetUserOpinionByIdAsync(id);
            UserOpinion.IsHidden = IsHidden;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserOpinionAsync(UserOpinion userOpinion)
        {
            _context.UserOpinions.Update(userOpinion);
            await _context.SaveChangesAsync();
        }
    }
}
