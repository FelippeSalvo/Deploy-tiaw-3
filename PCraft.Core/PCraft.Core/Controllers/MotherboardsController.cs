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
            var motherboard = await _context.Motherboards.FindAsync(id);

            if (motherboard == null)
                return NotFound();

            return Ok(motherboard);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Motherboard motherboard)
        {
            _context.Motherboards.Add(motherboard);
            await _context.SaveChangesAsync();

            return Ok(motherboard);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Motherboard motherboard)
        {
            if (id != motherboard.Id)
                return BadRequest();

            _context.Entry(motherboard).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(motherboard);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var motherboard = await _context.Motherboards.FindAsync(id);

            if (motherboard == null)
                return NotFound();

            _context.Motherboards.Remove(motherboard);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}