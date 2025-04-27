using Application.DTOs.Posts;
using Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Factories.Interfaces
{
    public interface IPostFactory
    {
        public Post CreatePost(CreatePostDto createPostDto);
    }
}
