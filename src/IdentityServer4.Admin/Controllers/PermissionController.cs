using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.Admin.Controllers
{ 
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    public class PermissionController : Controller
    { 
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet("{permissionId}/edit")]
        public IActionResult Edit()
        {
            return View();
        }
    }
}