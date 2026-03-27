using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PSUsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PSUsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.PSUs.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var psu = await _context.PSUs.FindAsync(id);

            if (psu == null)
                return NotFound();

            return Ok(psu);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PSU psu)
        {
            _context.PSUs.Add(psu);
            await _context.SaveChangesAsync();

            return Ok(psu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PSU psu)
        {
            if (id != psu.Id)
                return BadRequest();

            _context.Entry(psu).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(psu);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var psu = await _context.PSUs.FindAsync(id);

            if (psu == null)
                return NotFound();

            _context.PSUs.Remove(psu);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}