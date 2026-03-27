using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CPUsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CPUsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.CPUs.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)// 
        {
            var cpu = await _context.CPUs.FindAsync(id);

            if (cpu == null)
                return NotFound();

            return Ok(cpu);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CPU cpu)
        {
            _context.CPUs.Add(cpu);
            await _context.SaveChangesAsync();

            return Ok(cpu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CPU cpu)
        {
            if (id != cpu.Id)
                return BadRequest();

            _context.Entry(cpu).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(cpu);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cpu = await _context.CPUs.FindAsync(id);

            if (cpu == null)
                return NotFound();

            _context.CPUs.Remove(cpu);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}