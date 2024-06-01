using CatalogService.Controllers;
using CatalogService.Data;
using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogServiceTests
{
    public class ItemControllerTests
    {
        private readonly ItemController _controller;
        private readonly CatalogContext _context;

        public ItemControllerTests()
        {
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CatalogContext(options);
            _controller = new ItemController(_context);
        }

        [Fact]
        public async Task GetItems_ReturnsEmptyList()
        {
            var result = await _controller.GetItems(null, 1, 10);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task AddItem_ReturnsCreatedItem()
        {
            var category = new Category { Name = "Test Category" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var item = new Item { Name = "Test Item", Description = "Description", CategoryId = category.CategoryId };

            var actionResult = await _controller.AddItem(item);
            var result = actionResult.Result as CreatedAtActionResult;
            var returnValue = result?.Value as Item;

            Assert.NotNull(result);
            Assert.Equal("Test Item", returnValue?.Name);
        }

        [Fact]
        public async Task UpdateItem_ReturnsNoContent()
        {
            var category = new Category { Name = "Test Category" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var item = new Item { Name = "Original Item", Description = "Description", CategoryId = category.CategoryId };
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            item.Name = "Updated Item";
            var result = await _controller.UpdateItem(item.ItemId, item);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteItem_ReturnsNoContent()
        {
            var category = new Category { Name = "Test Category" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var item = new Item { Name = "Test Item", Description = "Description", CategoryId = category.CategoryId };
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteItem(item.ItemId);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
