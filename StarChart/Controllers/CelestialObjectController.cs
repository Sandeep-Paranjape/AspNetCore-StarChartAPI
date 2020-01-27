using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet("{id:int}", Name = "GetById")]        
        public IActionResult GetById(int Id)
        {
            var celestialObject = _context.CelestialObjects.Find(Id);
            if (celestialObject == null)
                return NotFound();

            celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == Id).ToList();

            return Ok(celestialObject);
        }
        
        [HttpGet ("{Name}")]
        public IActionResult GetByName (string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (!celestialObjects.Any())
                return NotFound();

            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == item.Id).ToList();
            }            

            return Ok(celestialObjects);
        }

    }
}
