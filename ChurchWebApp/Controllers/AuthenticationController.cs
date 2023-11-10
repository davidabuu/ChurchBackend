using ChurchWebApp.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChurchWebApp.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
       private readonly UserManager<IdentityUser> _userManager;

    private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
       
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register ([FromBody] Registration registration)
        {
            if (ModelState.IsValid)
            {
                //Check If User Exists
                var userExists = await _userManager.FindByEmailAsync(registration.Email!);
                if (userExists == null)
                {
                    var newUser = new IdentityUser()
                    {
                        UserName = registration.Email,
                        Email = registration.Email,
                    };
                    var isCreated = await _userManager.CreateAsync(newUser, registration.Password!);
                    if (isCreated.Succeeded)
                    {
                        var token = GenerateJwtToken(newUser);
                        return Ok(new { status = "true", msg = "Regsitration Succesful", data = token });
                    }
                    return BadRequest(isCreated);
                }
                return BadRequest("User Already Exists");
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            if (ModelState.IsValid) {
                var userExists = await _userManager.FindByEmailAsync(login.Email!);
                if(userExists == null)
                {
                    return NotFound("User Not Found");
                }
                //Check for the Password
                var isCorrect = await _userManager.CheckPasswordAsync(userExists, login.Password!);
                if (!isCorrect)
                {
                    return BadRequest("Password Is Incorrect");
                }
                var jwtToken = GenerateJwtToken(userExists);
                return Ok(jwtToken);

            }
            return BadRequest();
        }
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value!);

            // Token Descriptorc
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email !),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),

            }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)

            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;



        }


    }
}
