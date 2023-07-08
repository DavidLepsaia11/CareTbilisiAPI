using CareTbilisiAPI.Domain.Models;
using CareTbilisiAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CareTbilisiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration , UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequestModel requestModel )
        {
            var appRole = new ApplicationRole() { Name = requestModel.Role };
            var createdRole = await _roleManager.CreateAsync(appRole);

            return Ok(new { Message = "role created successfully" });
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponseModel))]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            var result = await LoginAsync(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel request) 
        {
            var result =  await RegisterAsync(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        #region Private Methods

        private async Task<LoginResponseModel> LoginAsync(LoginRequestModel loginRequestModel)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestModel.Email);
            if (user == null) return new LoginResponseModel() { Message = "Invalid Email/Password", Success = false };

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString() ),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ),
            };

            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("ApiKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(30);

            var token = new JwtSecurityToken
                (
                   issuer: "https://localhost:7030",
                   audience: "https://localhost:7030",
                   claims: claims,
                   expires: expires,
                   signingCredentials: creds
                );

            return new LoginResponseModel()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "Login Successful!",
                Email = user?.Email,
                Success = true,
                UserId = user?.Id.ToString()
            };
        }

        private async Task<RegisterResponseModel> RegisterAsync(RegisterRequestModel request)
        {
             var userExist = await _userManager.FindByEmailAsync(request.Email);
            if (userExist != null)  return new RegisterResponseModel() { Success = false, Message = "Such already exist" };

            userExist = new ApplicationUser()
            {
                 UserName = request.UserName,
                 Email = request.Email, 
                 FullName = request.FullName
            };

            var createdUserResult = await _userManager.CreateAsync(userExist, request.Password);
            if (!createdUserResult.Succeeded) return new RegisterResponseModel() { Success = false, Message = $"Create user failed {createdUserResult?.Errors?.FirstOrDefault()?.Description}" };

            var addUserToRoleResult = await _userManager.AddToRoleAsync(userExist, "USER");
            if (!addUserToRoleResult.Succeeded) return new RegisterResponseModel() { Success = false, Message = $"Create user Succeeded but could not add user to role {addUserToRoleResult?.Errors?.FirstOrDefault()?.Description}" };

            return new RegisterResponseModel()
            {
                Message = "User registered successfully",
                Success = true
            };
        }

        #endregion
    }
}
