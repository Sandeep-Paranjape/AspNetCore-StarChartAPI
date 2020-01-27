using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpPost]
        public IActionResult Create ([FromBody]CelestialObject CO)
        {
            _context.CelestialObjects.Add(CO);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = CO.Id }, CO);
        }

        [HttpPut ("{id}")]
        public IActionResult Update(int Id, CelestialObject CO)
        {
            var celestialObject = _context.CelestialObjects.Find(Id);

            if (celestialObject == null)
                return NotFound();

            celestialObject.Name = CO.Name;
            celestialObject.OrbitalPeriod = CO.OrbitalPeriod;
            celestialObject.OrbitedObjectId = CO.OrbitedObjectId;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch ("{id}/{name}")]
        public IActionResult RenameObject (int Id, string name)
        {
            var celestialObject = _context.CelestialObjects.Find(Id);

            if (celestialObject == null)
                return NotFound();

            celestialObject.Name = name;

            _context.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
            
        }

        [HttpDelete ("{id}")]
        public IActionResult Delete (int Id)
        {
            var celestialObject = _context.CelestialObjects.Where(x => x.OrbitedObjectId == Id || x.Id == Id);

            if (!celestialObject.Any())
                return NotFound();

            _context.CelestialObjects.RemoveRange(celestialObject);
            _context.SaveChanges();

            return NoContent();
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
        
        [HttpGet ("{name}")]
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
