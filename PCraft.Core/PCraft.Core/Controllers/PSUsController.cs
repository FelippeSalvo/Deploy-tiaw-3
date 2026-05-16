using Microsoft.AspNetCore.Mvc;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PSUsController : GenericComponentController<PSU>
    {
        public PSUsController(AppDbContext context) : base(context)
        {
        }
    }
}
