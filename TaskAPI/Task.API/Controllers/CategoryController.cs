using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.API.Data;
using Task.API.Models;
using Task.API.Models.DTO;
using Task.API.Repositories.IRepository;

namespace Task.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly TaskApiDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(TaskApiDbContext dbContext, IMapper mapper, ICategoryRepository categoryRepository)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await categoryRepository.GetAllAsync();
            return Ok(mapper.Map<List<CategoryDTO>>(categories));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var category = await categoryRepository.GetAsync(x => x.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CategoryDTO>(category));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCategoryRequestDTO categoryRequestDTO)
        {
            if (categoryRequestDTO == null)
            {
                return BadRequest();
            }
            var category = mapper.Map<Category>(categoryRequestDTO);
            await categoryRepository.AddAsync(category);
            return Ok(mapper.Map<CategoryDTO>(category));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequestDTO updateCategoryRequestDTO)
        {
            if (updateCategoryRequestDTO == null)
            {
                return BadRequest();
            }
            var category = await categoryRepository.GetAsync(x => x.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            mapper.Map(updateCategoryRequestDTO, category);

            await categoryRepository.UpdateAsync(category);
            return Ok(mapper.Map<CategoryDTO>(category));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var category = await categoryRepository.GetAsync(x => x.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            dbContext.Categories.Remove(category);

            await categoryRepository.RemoveAsync(category);
            return Ok(mapper.Map<CategoryDTO>(category));
        }

    }
}
