using IdentityServer4.Admin.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.Admin.Controllers.UI
{ 
    [Authorize(Roles = AdminConsts.AdminName)]
    [Route("role")]
    public class RoleController : BaseController
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

        [HttpGet("{roleId}/edit")]
        public IActionResult Edit()
        {
            return View();
        }
        
        [HttpGet("{roleId}/permission")]
        public IActionResult Permission()
        {
            return View();
        }
    }
}