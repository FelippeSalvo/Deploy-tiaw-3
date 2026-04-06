using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotherboardsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotherboardsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Motherboards.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _context.Motherboards.FindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Motherboard mb)
        {
            _context.Motherboards.Add(mb);
            await _context.SaveChangesAsync();

            return Ok(mb);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Motherboard mb)
        {
            if (id != mb.Id)
                return BadRequest();

            _context.Entry(mb).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(mb);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mb = await _context.Motherboards.FindAsync(id);

            if (mb == null)
                return NotFound();

            _context.Motherboards.Remove(mb);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
