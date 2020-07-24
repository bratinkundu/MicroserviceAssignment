using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginMicroservice.Database;
using LoginMicroservice.Database.Entities;

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
        public IEnumerable<User> GetAllUsers(int id)
        {
            return db.Users.ToList();
        }

        [HttpGet]
        public User GetCredentials(int id)
        {
            return db.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        [HttpPost]
        public IActionResult AddUser(User userdetails)
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

        [HttpDelete]
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

        [HttpPost]
        public IActionResult EditUser(User userdata)
        {
            try
            {
                db.Entry(userdata).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}