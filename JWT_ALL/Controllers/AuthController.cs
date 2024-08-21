using JWT_ALL.Data.Model;
using JWT_ALL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWT_ALL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
		private readonly IAuth _authService;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AuthController(IAuth authService, RoleManager<IdentityRole> roleManager)
		{
			_authService = authService;
			this._roleManager = roleManager;
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync([FromBody] Register model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.RegisterAsync(model);

			if (!result.IsAuthenticated)
				return BadRequest(result.Message);

			SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

			return Ok(result);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> LoginAsync([FromBody] Login model)
		{
			var result = await _authService.LoginAsync(model);

			if (!result.IsAuthenticated)
				return BadRequest(result.Message);

			if (!string.IsNullOrEmpty(result.RefreshToken))
				SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

			return Ok(result);
		}

		[HttpPost("add-role")]
		public async Task<IActionResult> AddRole([FromBody] string role)
		{
			if (await _roleManager.RoleExistsAsync(role))
				return BadRequest("Role Already exists");

		    var res= await _authService.AddRoleAsync(role);
            if (res!=null)
            {
				return Ok(res);
            }
            return BadRequest("something wrong");
		}

		[HttpPost("AssignRole")]
		public async Task<IActionResult> AssignRoleAsync([FromBody] AddRole model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.AssignRoleAsync(model);

			if (!string.IsNullOrEmpty(result))
				return BadRequest(result);

			return Ok(model);
		}

		[HttpGet("refreshToken")]
		public async Task<IActionResult> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];

			var result = await _authService.RefreshTokenAsync(refreshToken);

			if (!result.IsAuthenticated)
				return BadRequest(result);

			SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

			return Ok(result);
		}

		[HttpPost("revokeToken")]
		public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
		{
			var token = model.Token ?? Request.Cookies["refreshToken"];

			if (string.IsNullOrEmpty(token))
				return BadRequest("Token is required!");

			var result = await _authService.RevokeTokenAsync(token);

			if (!result)
				return BadRequest("Token is invalid!");

			return Ok();
		}

		private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = expires.ToLocalTime(),
				Secure = true,
				IsEssential = true,
				SameSite = SameSiteMode.None
			};

			Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
		}
	}
}
/*protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO [dbo].[AspNetRoles] VALUES ('{Guid.NewGuid()}', 'User', 'USER', '{Guid.NewGuid()}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM [dbo].[AspNetRoles] WHERE Name = 'User'");
        }*/