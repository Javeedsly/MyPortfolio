using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        //[Authorize] 
        public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductDto createDto)
        {
            try
            {
                var newProduct = await _productService.CreateProductAsync(createDto);
                return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
        }

        [HttpPut("{id}")]
        //[Authorize] 
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto updateDto)
        {
            try
            {
                await _productService.UpdateProductAsync(id, updateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
        }

        [HttpDelete("{id}")]
        //[Authorize] 
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}