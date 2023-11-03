using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domian.Entities.Identity;

namespace Pharmacy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SecuredController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SecuredController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        //just for test

        [HttpGet("secured")]
        public IActionResult GetData()
        {


            return Ok("Hello from secured controller");
        }
    }
}
