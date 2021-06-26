using System;
using System.IO;
using System.Web;
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
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult ListLocations()
        {
            List<Location> Locations = db.Locations.ToList();
            List<LocationDto> LocationDtos = new List<LocationDto>();

            Locations.ForEach(l => LocationDtos.Add(new LocationDto(){
                LocationId = l.LocationId,
                LocationName = l.LocationName,
                Address = l.Address,
                PostalCode = l.PostalCode,
            }));

            return Ok(LocationDtos);
        }

        //get a list of locations that are have a selected book
        // GET: api/LocationData/ListLocationsForBooks/1
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult ListLocationsForBooks(int id)
        {
            //books that are in the same location with selected id
            List<Location> Locations = db.Locations.Where(
                l=>l.Book.Any(
                b=>b.BookId==id)
            ).ToList();
            List<LocationDto> LocationDtos = new List<LocationDto>();

            Locations.ForEach(l => LocationDtos.Add(new LocationDto()
            {
                LocationId = l.LocationId,
                LocationName = l.LocationName,
                Address = l.Address,
                PostalCode = l.PostalCode
            }));

            return Ok(LocationDtos);
        }

        //get list of locations not associated with the selected book id
        // GET: api/LocationData/ListLocationsWithNoBooks/1
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult ListLocationsWithNoBooks(int id)
        {
            //books that are in the same location with selected id
            List<Location> Locations = db.Locations.Where(
                l => !l.Book.Any(
                b => b.BookId == id)
            ).ToList();
            List<LocationDto> LocationDtos = new List<LocationDto>();

            Locations.ForEach(l => LocationDtos.Add(new LocationDto()
            {
                LocationId = l.LocationId,
                LocationName = l.LocationName,
                Address = l.Address,
                PostalCode = l.PostalCode
            }));

            return Ok(LocationDtos);
        }

        // GET: api/LocationsData/FindLocation/5

        [HttpGet]
         [ResponseType(typeof(LocationDto))]
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
        [Authorize]
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

        [HttpPost]
        public IHttpActionResult UploadLocationPic(int id)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);


                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var locationPic = HttpContext.Current.Request.Files[0];

                    if (locationPic.ContentLength > 0)
                    {

                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(locationPic.FileName).Substring(1);

                        if (valtypes.Contains(extension))
                        {
                            try
                            {

                                string fn = id + "." + extension;
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Book/"), fn);

                                locationPic.SaveAs(path);


                                haspic = true;
                                picextension = extension;


                                Location selectedlocation = db.Locations.Find(id);
                                selectedlocation.LocationHasPic = haspic;
                                selectedlocation.PicExtension = extension;
                                db.Entry(selectedlocation).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                return BadRequest();

            }

        }

        // POST: api/LocationsData/AddLocation
        [ResponseType(typeof(Location))]
        [HttpPost]
        [Authorize]
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
        [Authorize]
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