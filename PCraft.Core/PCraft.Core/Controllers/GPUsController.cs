using Microsoft.AspNetCore.Mvc;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GPUsController : GenericComponentController<GPU>
    {
        public GPUsController(AppDbContext context) : base(context)
        {
        }
    }
}
