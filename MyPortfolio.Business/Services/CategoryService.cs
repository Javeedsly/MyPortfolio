using AutoMapper;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("Category not found.");
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("Category not found.");

            _mapper.Map(updateCategoryDto, category);
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("Category not found.");

            // Optional: Check if category has products before deleting
            // var products = await _unitOfWork.Products.Find(p => p.CategoryId == id);
            // if (products.Any()) throw new InvalidOperationException("Cannot delete category with associated products.");

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.CompleteAsync();
        }
    }
}