using Microsoft.AspNetCore.Http;

namespace MyPortfolio.Core.DTOs
{
    // ====== Category DTOs ======

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; }
    }

    public class UpdateCategoryDto
    {
        public string Name { get; set; }
    }

    // ====== Product DTOs ======

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImageFile { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}