using JWT_ALL.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace JWT_ALL.Services
{
    public interface IAuth

	{
		Task<Auth> RegisterAsync(Register model);
		Task<Auth> LoginAsync(Login model);
		Task<string> AssignRoleAsync(AddRole model);
		Task<string> AddRoleAsync(string role);
		Task<Auth> RefreshTokenAsync(string token);
		Task<bool> RevokeTokenAsync(string token);
	}
}
