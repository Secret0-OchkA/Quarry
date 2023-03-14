

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Warehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {

            return Ok();
        }
    }
}
