using Microsoft.AspNetCore.Mvc;
using PCraft.Core.Data;
using PCraft.Core.Models;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CPUsController : GenericComponentController<CPU>
    {
        public CPUsController(AppDbContext context) : base(context)
        {
        }
    }
}
