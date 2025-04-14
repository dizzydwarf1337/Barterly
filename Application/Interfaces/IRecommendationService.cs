using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRecommendationService
    {
        Task<ICollection<PostDto>> GetRecommendationPostsByUser(int count, Guid userId);
        Task<ICollection<PostDto>> GetFeed(int page, Guid? userId = null);
        Task<ICollection<PostDto>> GetPopularPosts(int pageCount, int Page);
        Task<ICollection<PostDto>> GetPromotedPosts(int count, string? categories = null, string? cities = null);
    }
}
