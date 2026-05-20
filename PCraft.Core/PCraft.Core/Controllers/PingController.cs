using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PingController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint super rápido e sem custo de banco para manter o Render acordado
        // Frequência ideal: A cada 14 minutos (apenas no horário ativo)
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok(new { status = "Healthy", message = "pong", timestamp = DateTime.UtcNow });
        }

        // Endpoint com consulta leve ao banco de dados para manter o Supabase acordado
        // Frequência ideal: 1 vez ao dia (ou a cada 12 horas)
        [HttpGet("db")]
        public async Task<IActionResult> PingDatabase()
        {
            try
            {
                // CanConnectAsync executa uma query muito leve no banco (geralmente SELECT 1 ou equivalente)
                bool canConnect = await _context.Database.CanConnectAsync();
                
                if (canConnect)
                {
                    return Ok(new { 
                        status = "Healthy", 
                        database = "Connected", 
                        message = "Supabase is active!", 
                        timestamp = DateTime.UtcNow 
                    });
                }
                
                return StatusCode(500, new { status = "Unhealthy", database = "Failed to connect" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }
    }
}
