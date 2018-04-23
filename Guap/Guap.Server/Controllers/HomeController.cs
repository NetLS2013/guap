using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Guap.Server.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/invite")]
        public async Task<IActionResult> Invite()
        {
            return View();
        }
    }
}