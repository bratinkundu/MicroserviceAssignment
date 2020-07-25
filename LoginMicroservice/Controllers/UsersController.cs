using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using LoginMicroservice.Database;
using LoginMicroservice.Database.Entities;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace LoginMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        DatabaseContext db;
        public UsersController()
        {
            db = new DatabaseContext();
        }

        [HttpGet("GetAllUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        [HttpPost("UserLogin")]
        public IActionResult Login([FromBody] User user)
        {
            if (CheckCredentials(user.Username, user.Password))
            {
                var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"));
                var now = DateTime.Now;

                var claim = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: "Admin",
                    audience: "user",
                    claims: claim,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(20)),
                    signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
                    );

                var responseToken = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(responseToken);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody]User userdetails)
        {
            try
            {
                db.Users.Add(userdetails);
                db.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                db.Users.Remove(db.Users.Where(x => x.Id == id).FirstOrDefault());
                db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser( [FromBody] User userdata)
        {
            try
            {
                if (db.Users.Find(userdata.Id)!= null)
                {
                    db.Entry(userdata).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [NonAction]
        public bool CheckCredentials(string username, string password)
        {
            var userdata = db.Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            if (userdata != null) { return true; }
            else { return false; }
        }
    }
}