using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.API.Data;
using Task.API.Models;
using Task.API.Models.DTO;

namespace Task.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly TaskApiDbContext dbContext;
        private readonly IMapper mapper;

        public ProductsController(TaskApiDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? category = null,
            [FromQuery] string? tag = null)
        {
            var query = dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.CategoryName.Contains(category));

            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(p => p.ProductTags.Any(pt => pt.Tag.TagName.Contains(tag)));

            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(mapper.Map<List<ProductDTO>>(items));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.ProductId == id);

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
                await dbContext.SaveChangesAsync();
            }
            var createdProduct = await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.ProductId }, mapper.Map<ProductDTO>(createdProduct));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequestDTO dto)
        {
            var product = await dbContext.Products
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();
            mapper.Map(dto, product);

            var existingTagIds = product.ProductTags.Select(pt => pt.TagId).ToList();
            var toRemove = product.ProductTags.Where(pt => !dto.TagIds.Contains(pt.TagId)).ToList();
            var toAdd = dto.TagIds.Except(existingTagIds).ToList();

            dbContext.ProductTags.RemoveRange(toRemove);
            foreach (var tagId in toAdd)
            {
                product.ProductTags.Add(new ProductTag { ProductId = id, TagId = tagId });
            }

            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
