using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GPUsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GPUsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.GPUs.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var gpu = await _context.GPUs.FindAsync(id);

            if (gpu == null)
                return NotFound();

            return Ok(gpu);
        }

        [HttpPost]
        public async Task<IActionResult> Post(GPU gpu)
        {
            _context.GPUs.Add(gpu);
            await _context.SaveChangesAsync();

            return Ok(gpu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, GPU gpu)
        {
            if (id != gpu.Id)
                return BadRequest();

            _context.Entry(gpu).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(gpu);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var gpu = await _context.GPUs.FindAsync(id);

            if (gpu == null)
                return NotFound();

            _context.GPUs.Remove(gpu);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}