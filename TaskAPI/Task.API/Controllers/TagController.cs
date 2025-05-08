using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.API.Data;
using Task.API.Models;
using Task.API.Models.DTO;

namespace Task.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TaskApiDbContext dbContext;
        private readonly IMapper mapper;

        public TagController(TaskApiDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tags = await dbContext.Tags.ToListAsync();
            return Ok(mapper.Map<List<TagDTO>>(tags));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var tag = await dbContext.Tags.FirstOrDefaultAsync(x => x.TagId == id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<TagDTO>(tag));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTagRequestDTO tagRequestDTO)
        {
            if (tagRequestDTO == null)
            {
                return BadRequest();
            }
            var tag = mapper.Map<Tag>(tagRequestDTO);
            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();
            return Ok(mapper.Map<TagDTO>(tag));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTagRequestDTO updateTagRequestDTO)
        {
            if (updateTagRequestDTO == null)
            {
                return BadRequest();
            }
            var tag = await dbContext.Tags.FirstOrDefaultAsync(x => x.TagId == id);
            if (tag == null)
            {
                return NotFound();
            }
            mapper.Map(updateTagRequestDTO, tag);
            await dbContext.SaveChangesAsync();
            return Ok(mapper.Map<TagDTO>(tag));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var tag = await dbContext.Tags.FirstOrDefaultAsync(x => x.TagId == id);
            if (tag == null)
            {
                return NotFound();
            }
            dbContext.Tags.Remove(tag);
            await dbContext.SaveChangesAsync();
            return Ok(mapper.Map<TagDTO>(tag));
        }
    }
}
