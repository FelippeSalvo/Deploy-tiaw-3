using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RAMsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RAMsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.RAMs.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _context.RAMs.FindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(RAM ram)
        {
            _context.RAMs.Add(ram);
            await _context.SaveChangesAsync();

            return Ok(ram);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, RAM ram)
        {
            if (id != ram.Id)
                return BadRequest();

            _context.Entry(ram).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(ram);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ram = await _context.RAMs.FindAsync(id);

            if (ram == null)
                return NotFound();

            _context.RAMs.Remove(ram);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
