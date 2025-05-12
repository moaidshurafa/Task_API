using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Task.API.Data;
using Task.API.Models;
using Task.API.Models.DTO;
using Task.API.Repositories.IRepository;

namespace Task.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly TaskApiDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;

        public ProductsController(TaskApiDbContext dbContext, IMapper mapper, IProductRepository productRepository)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? category = null,
            [FromQuery] string? tag = null)
        {
            Expression<Func<Product, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(category))
            {
                filter = p => p.Category.CategoryName.Contains(category);
            }
            if (!string.IsNullOrWhiteSpace(tag))
            {
            }

            var includeProperties = "Category,ProductTags.Tag";
            var products = await productRepository.GetAllAsync(filter, includeProperties);

            var paged = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(mapper.Map<List<ProductDTO>>(paged));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await productRepository.GetAsync(
                p => p.ProductId == id,
                includeProperties: "Category,ProductTags.Tag"
            );

            if (product == null)
                return NotFound();

            return Ok(mapper.Map<ProductDTO>(product));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddProductRequestDTO request)
        {
            var product = mapper.Map<Product>(request);
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            if (request.TagIds != null && request.TagIds.Any())
            {
                foreach (var tagId in request.TagIds.Distinct())
                {
                    dbContext.ProductTags.Add(new ProductTag
                    {
                        ProductId = product.ProductId,
                        TagId = tagId
                    });
                }
                await productRepository.SaveAsync();
            }
            var createdProduct = await productRepository.GetAsync(
                p => p.ProductId == product.ProductId,
                includeProperties: "Category,ProductTags.Tag"
                );

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.ProductId }, mapper.Map<ProductDTO>(createdProduct));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequestDTO dto)
        {
            var product = await productRepository.GetAsync(
                p => p.ProductId == id,
                includeProperties: "ProductTags"
            );

            if (product == null)
                return NotFound();

            mapper.Map(dto, product);

            var existingTagIds = product.ProductTags.Select(pt => pt.TagId).ToList();
            var toRemove = product.ProductTags.Where(pt => !dto.TagIds.Contains(pt.TagId)).ToList();
            var toAdd = dto.TagIds.Except(existingTagIds).ToList();

            foreach (var pt in toRemove)
                product.ProductTags.Remove(pt);

            foreach (var tagId in toAdd)
                product.ProductTags.Add(new ProductTag { ProductId = id, TagId = tagId });

            await productRepository.SaveAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productRepository.GetAsync(u => u.ProductId == id);
            if (product == null)
                return NotFound();

            await productRepository.RemoveAsync(product);
            return Ok(mapper.Map<ProductDTO>(product));
        }
    }
}
