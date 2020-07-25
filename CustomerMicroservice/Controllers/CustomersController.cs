using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerMicroservice.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        DatabaseContext db;
        public CustomersController()
        {
            db = new DatabaseContext();
        }
        [HttpGet("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            return Ok(db.Customers.ToList()); 
        }
    }
}