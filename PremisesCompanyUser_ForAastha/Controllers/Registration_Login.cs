using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ChiragGupta_FullStackAssignment.Data;
using ChiragGupta_FullStackAssignment.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ChiragGupta_FullStackAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Registration_Login : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ApplicationDBContext _context;


        public Registration_Login(IConfiguration configuration, ApplicationDBContext context)
        {
            _configuration = configuration;
            _context = context;

        }

        public static UserModel user = new UserModel();
        public static Login login = new Login();

        [HttpPost("User_Register")]
        //[Authorize]
        public async Task<ActionResult<UserModel>> User_Register(UserModelDTO request)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.UserName = request.UserName;
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.PhoneNumber = request.PhoneNumber;
                    user.Email = request.Email;
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    login.Email = request.Email;
                    login.PasswordHash = passwordHash;
                    login.PasswordSalt = passwordSalt;
                    login.UserName = request.UserName;



                    var fuser = _context.Users.Where(x => x.UserName == request.UserName || x.Email == request.Email).ToList();
                    if (fuser.Count > 0)
                    {
                        return BadRequest("This Username already exists");
                    }
                    else
                    {
                        _context.Users.Add(user);
                        _context.Login.Add(login);
                        _context.SaveChanges();


                        return Ok(user);


                    }


                }
                return BadRequest("An error has occured");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            

        }

        [HttpPost("Consumer_Login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Consumer_Login(LoginDTO request)
        {
            try
            {
                var fuser = _context.Login.Where(x => x.UserName == request.UserName || x.Email == request.Email).ToList();
                if (fuser.Count == 0)
                {
                    return StatusCode(404);
                }

                foreach (var consumer in fuser)
                {

                    if (!VerifyPasswordHash(request.password, consumer.PasswordHash, consumer.PasswordSalt))
                    {

                        return StatusCode(401);
                    }


                    string Username = consumer.UserName;
                    string Email = consumer.Email;
                    string token = CreateToken(consumer);

                    return Ok(new { Email = Email, Username = Username, Token = token });
                }
                // The loop has completed without finding a matching user
                return BadRequest("User not found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        
        private string CreateToken(Login consumer)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, consumer.UserName),
                new Claim(ClaimTypes.Email, consumer.Email)
            };


            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            return jwt;

        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }



    }


}
