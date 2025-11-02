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
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<BlogDto> GetBlogByIdAsync(int id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
            return _mapper.Map<BlogDto>(blog);
        }

        public async Task<BlogDto> GetBlogBySlugAsync(string slug)
        {
            var blog = await _unitOfWork.Blogs.GetBySlugAsync(slug);
            return _mapper.Map<BlogDto>(blog);
        }

        public async Task<IEnumerable<BlogDto>> GetAllBlogsAsync()
        {
            var blogs = await _unitOfWork.Blogs.GetAllAsync();
            return _mapper.Map<IEnumerable<BlogDto>>(blogs);
        }

        public async Task<BlogDto> CreateBlogAsync(CreateBlogDto createBlogDto)
        {
            string imageUrl = await SaveImageAsync(createBlogDto.ImageFile);
            var blog = _mapper.Map<Blog>(createBlogDto);

            blog.ImageUrl = imageUrl;
            blog.PublishedDate = DateTime.UtcNow; 

            await _unitOfWork.Blogs.AddAsync(blog);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<BlogDto>(blog);
        }

        public async Task UpdateBlogAsync(int id, UpdateBlogDto updateBlogDto)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
            if (blog == null)
            {
                throw new KeyNotFoundException("Blog Not Found!");
            }

            if (updateBlogDto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(blog.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, blog.ImageUrl.TrimStart('/'));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                blog.ImageUrl = await SaveImageAsync(updateBlogDto.ImageFile);
            }

            _mapper.Map(updateBlogDto, blog);
            _unitOfWork.Blogs.Update(blog);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteBlogAsync(int id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
            if (blog == null)
            {
                throw new KeyNotFoundException("Blog Not Found!");
            }

            // Delete the associated image file
            if (!string.IsNullOrEmpty(blog.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, blog.ImageUrl.TrimStart('/'));
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            _unitOfWork.Blogs.Delete(blog);
            await _unitOfWork.CompleteAsync();
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image was not provided.");
            }

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

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/uploads/{uniqueFileName}";
        }
    }
}