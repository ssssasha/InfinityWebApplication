using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AutoShowWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AutoShowContext _context;

        public ValuesController(AutoShowContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var orders = _context.Drives.Include(b => b.Cars).ToList();
            List<object> catBook = new List<object>();
            catBook.Add(new[] { "Тип приводу", "Кількість автомобілів" });
            foreach (var c in orders)
            {
                catBook.Add(new object[] { c.DriveType, c.Cars.Count() });

            }
            return new JsonResult(catBook);
        }

        [HttpGet("JsonData2")]
        public JsonResult JsonData2()
        {
            var orders = _context.BodyTypes.Include(b => b.Cars).ToList();
            List<object> catBook = new List<object>();
            catBook.Add(new[] { "Тип кузова", "Кількість автомобілів" });
            foreach (var c in orders)
            {
                catBook.Add(new object[] { c.BodyTypeNames, c.Cars.Count() });

            }
            return new JsonResult(catBook);
        }
    }

}