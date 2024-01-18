using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

namespace Signup.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _db;

        public AuthenticationController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _db = context;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpData signUpData)
        {
            try
            {
                bool emailExists = await _db.signUpDatas.AnyAsync(s => s.Email == signUpData.Email);
                if (emailExists)
                {
                    return BadRequest("Email already registered");
                }

                _db.signUpDatas.Add(signUpData);
                await _db.SaveChangesAsync();

                return Ok(signUpData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginData loginData)
        {
            try
            {
                var user = await _db.signUpDatas.SingleOrDefaultAsync(s => s.Email == loginData.Email && s.Password == loginData.Password && s.Role == loginData.Role);
                if (user == null)
                {
                    return Unauthorized("Invalid email or password");
                }

                var token = GenerateJwtToken(user);

                return Ok(new { token,user.Id,user.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(SignUpData user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;
// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using System.Threading.Tasks;
// using WebAPI.Data;
// using WebAPI.Models;

// namespace Signup.Controllers
// {
//     [ApiController]
//     [Route("[controller]")]
//     public class AuthenticationController : ControllerBase
//     {
//         private readonly IConfiguration _configuration;
//         private readonly DataContext _dbContext;

//         public AuthenticationController(IConfiguration configuration, DataContext dbContext)
//         {
//             _configuration = configuration;
//             _dbContext = dbContext;
//         }

//         [HttpPost]
//         public async Task<IActionResult> SignUp(SignUpData signUpData)
//         {
//             try
//             {
//                 bool emailExists = await _dbContext.signUpDatas.AnyAsync(s => s.Email == signUpData.Email);
//                 if (emailExists)
//                 {
//                     return BadRequest("Email already registered");
//                 }

//                 _dbContext.signUpDatas.Add(signUpData);
//                 await _dbContext.SaveChangesAsync();

//                 return CreatedAtAction(nameof(SignUp), signUpData);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex.Message}");
//             }
//         }

//         [HttpPost]
//         [Route("login")]
//         public async Task<IActionResult> Login(LoginData loginData)
//         {
//             try
//             {
//                 var user = await _dbContext.signUpDatas.SingleOrDefaultAsync(s => s.Email == loginData.Email && s.Password == loginData.Password);
//                 if (user == null)
//                 {
//                     return Unauthorized("Invalid email or password");
//                 }

//                 var token = GenerateJwtToken(user);

//                 return Ok(new { token });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex.Message}");
//             }
//         }

//         private string GenerateJwtToken(SignUpData user)
//         {
//             var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
//             var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//             var claims = new[]
//             {
//                 new Claim(ClaimTypes.Email, user.Email),
//                 new Claim(ClaimTypes.Role, user.Role)
//             };

//             var token = new JwtSecurityToken(
//                 issuer: _configuration["Jwt:Issuer"],
//                 audience: _configuration["Jwt:Audience"],
//                 claims: claims,
//                 expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
//                 signingCredentials: credentials
//             );

//             return new JwtSecurityTokenHandler().WriteToken(token);
//         }
//     }
// }







// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;
// using Signup.Models;
// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using System.Threading.Tasks;
// using WebAPI.Data;
// using WebAPI.Models;

// namespace Signup.Controllers
// {
//     [ApiController]
//     [Route("[controller]")]
//     public class AuthenticationController : ControllerBase
//     {
//         private readonly IConfiguration _configuration;
//         private readonly DataContext db;

//         public AuthenticationController(IConfiguration configuration, DataContext context)
//         {
//             _configuration = configuration;
//             db = context;
//         }

//         [HttpPost]
//         public async Task<IActionResult> SignUp(SignUpData signUpData)
//         {
//             try
//             {
//                 bool emailExists = await db.signUpDatas.AnyAsync(s => s.Email == signUpData.Email);
//                 if (emailExists)
//                 {
//                     return BadRequest("Email already registered");
//                 }

//                 db.signUpDatas.Add(signUpData);
//                 await db.SaveChangesAsync();

//                 //return Ok(signUpData);
//                 return CreatedAtAction(nameof(SignUp), signUpData);

//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex.Message}");
//             }
//         }

//         [HttpPost]
//         [Route("login")]
//         public async Task<IActionResult> Login(LoginData loginData)
//         {
//             try
//             {
//                 var user = await db.signUpDatas.SingleOrDefaultAsync(s => s.Email == loginData.Email && s.Password == loginData.Password);
//                 if (user == null)
//                 {
//                     return Unauthorized("Invalid email or password");
//                 }

//                 var token = GenerateJwtToken(user);

//                 return Ok(new { token });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex.Message}");
//             }
//         }

//         private string GenerateJwtToken(SignUpData user)
//         {
//             var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
//             var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//             var claims = new[]
//             {
//                 new Claim(ClaimTypes.Email, user.Email),
//                 new Claim(ClaimTypes.Role, user.Role)
//             };

//             var token = new JwtSecurityToken(
//                 issuer: _configuration["Jwt:Issuer"],
//                 audience: _configuration["Jwt:Audience"],
//                 claims: claims,
//                 expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
//                 signingCredentials: credentials
//             );

//             return new JwtSecurityTokenHandler().WriteToken(token);
//         }
//     }
// }



// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using Microsoft.EntityFrameworkCore;
// using Signup.Models;
// using MimeKit;
// using MailKit.Net.Smtp;
// using WebAPI.Data;
// using WebAPI.Models;

// namespace Signup.Controllers;

// [ApiController]
// [Route("[controller]")]

// public class AuthenticationController : ControllerBase{
//     private readonly IConfiguration _configuration;

//     private readonly DataContext db;

//     public AuthenticationController(IConfiguration configuration,DataContext context){
//         db=context;
//         _configuration = configuration;
//     }
//     [HttpPost]

//     public async Task<ActionResult<SignUpData>>SignUp(SignUpData sign)
//     {
//     bool emailExists = await db.signUpDatas.AnyAsync(s => s.Email == sign.Email);
//     if (emailExists)
//     {
//         return BadRequest("Email already registered");
//     }
   

//         db.signUpDatas.Add(sign);

//         await db.SaveChangesAsync();
//         return (sign);
//     }
// }

















































 // if (sign.Password != sign.Confirmpassword)
    // {
    //     return BadRequest("Password and confirm password do not match");
    // }

    //  [HttpPost]
    //     [Route("forgotpassword")]
    //     public async Task<ActionResult<string>> ForgotPassword(string email)
    //     {
    //         var user = await _context.Signup_Detail.FirstOrDefaultAsync(u => u.Email == email);
    //         if (user == null)
    //         {
    //             return BadRequest("User not found");
    //         }

    //         var message = new MimeMessage();
    //         message.From.Add(new MailboxAddress("Swedha", "murugaiyanlatha6@gmail.com"));
    //         message.To.Add(new MailboxAddress(user.Name, user.Email));
    //         message.Subject = "Your previous password";

    //         var bodyBuilder = new BodyBuilder();
    //         bodyBuilder.TextBody = "Your previous password is: " + user.Password;

    //         message.Body = bodyBuilder.ToMessageBody();

    //         using var client = new SmtpClient();
    //         await client.ConnectAsync(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]), false);
    //         await client.AuthenticateAsync(_configuration["SMTP:Username"], _configuration["SMTP:Password"]);
    //         await client.SendAsync(message);
    //         await client.DisconnectAsync(true);

    //         return Ok();
    //     }
// [HttpPost]
// [Route("forgotpassword")]
// public async Task<ActionResult<string>> ForgotPassword(ForgotPasswordRequest request)
// {
//     var user = await _context.Signup_Detail.FirstOrDefaultAsync(u => u.Email == request.Email);
//     if (user == null)
//     {
//         return BadRequest("User not found");
//     }

//     var message = new MimeMessage();
//     message.From.Add(new MailboxAddress("Sw...", "murugaiyanlatha6@gmail.com"));
//     message.To.Add(new MailboxAddress("", user.Email));
//     message.Subject = "Password reset request";
//     message.Body = new TextPart("plain")
//     {
//         Text = "Please click on the following link to reset your password: http://localhost:5285/resetpassword?email=" + user.Password
//     };

//     using (var client = new SmtpClient())
//     {
//         client.Connect(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]), false);
//         client.Authenticate(_configuration["SMTP:Username"], _configuration["SMTP:Password"]);
//         await client.SendAsync(message);
//         client.Disconnect(true);
//     }

//     return Ok("Password reset email has been sent");
// }

// public class ForgotPasswordRequest
// {
//     public string Email { get; set; }
// }

