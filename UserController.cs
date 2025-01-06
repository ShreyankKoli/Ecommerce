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

      
        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {   
            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("UserName or Password cannot be null or empty.");
            }
            try
            {
                if (user.UserName != null)
                {
                    var claims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("userName", user.UserName.ToString()),
                    new Claim("FirstName", user.FirstName.ToString())

                };
                    var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                        configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: signIn
                        );
                    string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

                    user.Password = _passwordHasher.HashPassword(user, user.Password);
                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();

                    return Ok(new { Token = tokenValue, result = true, message = "User added successfully", user });
                    //return Ok(new { result = true, message = "User added successfully", user });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, message = "Internal Server Error", error = ex.Message });
            }
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
