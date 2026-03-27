using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CasesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CasesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Cases.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var caseItem = await _context.Cases.FindAsync(id);

            if (caseItem == null)
                return NotFound();

            return Ok(caseItem);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Case caseItem)
        {
            _context.Cases.Add(caseItem);
            await _context.SaveChangesAsync();

            return Ok(caseItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Case caseItem)
        {
            if (id != caseItem.Id)
                return BadRequest();

            _context.Entry(caseItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(caseItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var caseItem = await _context.Cases.FindAsync(id);

            if (caseItem == null)
                return NotFound();

            _context.Cases.Remove(caseItem);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}