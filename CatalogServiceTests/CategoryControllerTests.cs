using CatalogService.Controllers;
using CatalogService.Data;
using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceTests
{
    public class CategoryControllerTests
    {
        private readonly CategoryController _controller;
        private readonly CatalogContext _context;

        public CategoryControllerTests()
        {
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CatalogContext(options);
            _controller = new CategoryController(_context);
        }

        [Fact]
        public async Task GetCategories_ReturnsList()
        {
            var result = await _controller.GetCategories();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task AddCategory_ReturnsCreatedCategory()
        {
            var category = new Category { Name = "Test Category" };

            var actionResult = await _controller.AddCategory(category);
            var result = actionResult.Result as CreatedAtActionResult;
            var returnValue = result.Value as Category;

            Assert.NotNull(result);
            Assert.Equal("Test Category", returnValue?.Name);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsNoContent()
        {
            var category = new Category { Name = "Some Category" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            category.Name = "Updated Category";
            var result = await _controller.UpdateCategory(category.CategoryId, category);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNoContent()
        {
            var category = new Category { Name = "Test Category" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteCategory(category.CategoryId);

            Assert.IsType<NoContentResult>(result);
        }
    }
}