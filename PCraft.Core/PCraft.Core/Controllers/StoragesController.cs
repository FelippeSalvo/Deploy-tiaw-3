using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoragesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StoragesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Storages.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var storage = await _context.Storages.FindAsync(id);

            if (storage == null)
                return NotFound();

            return Ok(storage);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Storage storage)
        {
            _context.Storages.Add(storage);
            await _context.SaveChangesAsync();

            return Ok(storage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Storage storage)
        {
            if (id != storage.Id)
                return BadRequest();

            _context.Entry(storage).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(storage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var storage = await _context.Storages.FindAsync(id);

            if (storage == null)
                return NotFound();

            _context.Storages.Remove(storage);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}