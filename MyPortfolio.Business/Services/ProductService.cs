using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MyPortfolio.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductWithCategoryByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found.");
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllProductsWithCategoryAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            // Check if category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(createProductDto.CategoryId);
            if (category == null) throw new KeyNotFoundException("Category not found.");

            string imageUrl = await SaveImageAsync(createProductDto.ImageFile);
            var product = _mapper.Map<Product>(createProductDto);
            product.ImageUrl = imageUrl;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            var newProduct = await _unitOfWork.Products.GetProductWithCategoryByIdAsync(product.Id);
            return _mapper.Map<ProductDto>(newProduct);
        }

        public async Task UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found.");

            // Check if category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(updateProductDto.CategoryId);
            if (category == null) throw new KeyNotFoundException("Category not found.");

            if (updateProductDto.ImageFile != null)
            {
                DeleteImage(product.ImageUrl);
                product.ImageUrl = await SaveImageAsync(updateProductDto.ImageFile);
            }

            _mapper.Map(updateProductDto, product);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found.");

            DeleteImage(product.ImageUrl);
            _unitOfWork.Products.Delete(product);
            await _unitOfWork.CompleteAsync();
        }

        // --- Helper Methods for Image Handling ---

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image was not provided.");
            }

            var uploadPath = GetUploadPath();
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/uploads/{uniqueFileName}";
        }

        private void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        private string GetUploadPath()
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploadPath = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            return uploadPath;
        }
    }
}