using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LibraryCMS.Models;
using System.Diagnostics;

namespace LibraryCMS.Controllers
{
    public class LocationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/LocationsData/GetLocation
        [HttpGet]
        public IEnumerable<LocationDto> ListLocations()
        {
            List<Location> Locations = db.Locations.ToList();
            List<LocationDto> LocationDtos = new List<LocationDto>();

            Locations.ForEach(l => LocationDtos.Add(new LocationDto(){
                LocationId = l.LocationId,
                LocationName = l.LocationName,
                Address = l.Address,
                PostalCode = l.PostalCode,
            }));

            return LocationDtos;
        }

        // GET: api/LocationsData/FindLocation/5
        [ResponseType(typeof(Location))]
        [HttpGet]
        public IHttpActionResult FindLocation(int id)
        {
            Location location = db.Locations.Find(id);
            LocationDto LocationDto = new LocationDto()
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                Address = location.Address,
                PostalCode = location.PostalCode,
            };
            if (location == null)
            {
                return NotFound();
            }

            return Ok(LocationDto);
        }

        // PUT: api/LocationsData/UpdateLocation/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateLocation(int id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.LocationId)
            {
                Debug.WriteLine("The location id is " + id);
                Debug.WriteLine("The other location id is " + location.LocationId);
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
;
            Debug.WriteLine("There are no errors");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/LocationsData/AddLocation
        [ResponseType(typeof(Location))]
        [HttpPost]
        public IHttpActionResult AddLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = location.LocationId }, location);
        }

        // DELETE: api/LocationsData/DeleteLocation/5
        [ResponseType(typeof(Location))]
        [HttpPost]
        public IHttpActionResult DeleteLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(int id)
        {
            return db.Locations.Count(e => e.LocationId == id) > 0;
        }
    }
}