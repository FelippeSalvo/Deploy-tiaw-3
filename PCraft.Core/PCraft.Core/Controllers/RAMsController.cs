using Microsoft.AspNetCore.Mvc;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RAMsController : GenericComponentController<RAM>
    {
        public RAMsController(AppDbContext context) : base(context)
        {
        }
    }
}
