using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EcommerceContext context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration configuration;

        public UserController(EcommerceContext context, IConfiguration configuration)
        {
            this.context = context;
            _passwordHasher = new PasswordHasher<User>();
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUser()  //ActionResult what a controller action returns in response
        {
            var data = await context.Users.ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<User>>> GetUserById(int id)
        {
            var data = await context.Users.FindAsync(id);
            if (data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }

        [Authorize]
        [HttpGet("Login")]
        public async Task<ActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Username and password is required:");
            }
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return NotFound("User notFound");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid password.");
            }
                return Ok(new {Message ="Login Successfull", User = user});
        }

      
    [AllowAnonymous] // Allow login without authentication
[HttpPost("Login")]
public async Task<IActionResult> Login(string username, string password)
{
    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        return BadRequest("Username and password are required.");
    }

    var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
    if (user == null)
    {
        return NotFound("User not found.");
    }

    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
    if (result == PasswordVerificationResult.Failed)
    {
        return Unauthorized("Invalid password.");
    }

    // Generate JWT token with RoleId
    var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("userName", user.UserName),
        new Claim("FirstName", user.FirstName ?? ""),
        new Claim("RoleId", user.RoleId.ToString()) // Add RoleId as a claim
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        configuration["Jwt:Issuer"],
        configuration["Jwt:Audience"],
        claims,
        expires: DateTime.UtcNow.AddDays(1),
        signingCredentials: signIn
    );

    var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
    return Ok(new { Token = tokenValue, Message = "Login Successful", User = user });
}

   

        [HttpPut("{user}")]
        public async Task<ActionResult<User>> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var delete = await context.Users.FindAsync(id);
            if (delete == null)
            {
                return NotFound();
            }
            context.Users.RemoveRange(delete);
            await context.SaveChangesAsync();
            return Ok(delete);
        }
    }
}
