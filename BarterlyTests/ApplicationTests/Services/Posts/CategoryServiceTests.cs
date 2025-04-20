using Application.DTOs.Categories;
using Application.Services;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarterlyUnitTests.ApplicationTests.Services.Posts
{
    public class CategoryServiceTests
    {

        [Fact]
        public async Task AddCategory_ShouldFilterSubCategories_AndAddDefaultSubCategory()
        {
            // Arrange
            var mockCommandRepo = new Mock<ICategoryCommandRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockQueryRepo = new Mock<ICategoryQueryRepository>();

            var inputDto = new CategoryDto
            {
                Id = Guid.NewGuid(),
                SubCategories = new List<SubCategoryDto>
                {
                    new SubCategoryDto { TitleEN = "Valid", TitlePL = "Valid" },
                    new SubCategoryDto { TitleEN = "", TitlePL = "No EN" },
                    new SubCategoryDto { TitleEN = "No PL", TitlePL = "" }
                }
            };
            var mappedCategory = new Category
            {
                Id = inputDto.Id,
                SubCategories = inputDto.SubCategories
                    .Where(x => !string.IsNullOrWhiteSpace(x.TitlePL) && !string.IsNullOrWhiteSpace(x.TitleEN))
                    .Select(x => new SubCategory { TitleEN = x.TitleEN, TitlePL = x.TitlePL })
                    .ToList()
            };

            mockMapper.Setup(m => m.Map<Category>(inputDto)).Returns(mappedCategory);

            var service = new CategoryService(mockCommandRepo.Object, mockQueryRepo.Object, mockMapper.Object);

            // Act
            await service.AddCategory(inputDto);

            // Assert
            mockCommandRepo.Verify(r => r.CreateCategoryAsync(It.Is<Category>(c =>
                c.SubCategories.Count == 1 &&
                c.SubCategories.Any(s => s.TitleEN == "Valid" && s.TitlePL == "Valid")
            )), Times.Once);

            mockCommandRepo.Verify(r => r.AddSubCategoryAsync(It.Is<SubCategory>(s =>
                s.TitleEN == "All" &&
                s.TitlePL == "Wszystkie" &&
                s.CategoryId == inputDto.Id
            )), Times.Once);
        }
    }
}
