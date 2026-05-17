using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class GenericComponentController<T> : ControllerBase where T : class, IComponent
    {
        private readonly AppDbContext _context;

        public GenericComponentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Set<T>().ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Post(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, T entity)
        {
            if (id != entity.Id)
                return BadRequest();

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
                return NotFound();

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
