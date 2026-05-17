using Microsoft.AspNetCore.Mvc;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotherboardsController : GenericComponentController<Motherboard>
    {
        public MotherboardsController(AppDbContext context) : base(context)
        {
        }
    }
}
