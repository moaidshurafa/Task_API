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
    public class TagController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITagRepository tagRepository;

        public TagController(IMapper mapper, ITagRepository tagRepository)
        {
            this.mapper = mapper;
            this.tagRepository = tagRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tags = await tagRepository.GetAllAsync();
            return Ok(mapper.Map<List<TagDTO>>(tags));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var tag = await tagRepository.GetAsync(x => x.TagId == id);
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
            await tagRepository.AddAsync(tag);
            return Ok(mapper.Map<TagDTO>(tag));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTagRequestDTO updateTagRequestDTO)
        {
            if (updateTagRequestDTO == null)
            {
                return BadRequest();
            }
            var tag = await tagRepository.GetAsync(x => x.TagId == id);
            if (tag == null)
            {
                return NotFound();
            }
            mapper.Map(updateTagRequestDTO, tag);
            await tagRepository.UpdateAsync(tag);
            return Ok(mapper.Map<TagDTO>(tag));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var tag = await tagRepository.GetAsync(x => x.TagId == id);
            if (tag == null)
            {
                return NotFound();
            }
            await tagRepository.RemoveAsync(tag);
            return Ok(mapper.Map<TagDTO>(tag));
        }
    }
}
