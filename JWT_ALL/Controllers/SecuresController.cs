using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_ALL.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SecuresController : ControllerBase
	{
		//[Authorize(Roles = "Admin,User")]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Admin")]
		//[Authorize(Policy = "AdminPolicy")]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok("values from secure controller.");
		}
	}
}
