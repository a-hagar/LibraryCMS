using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using LibraryCMS.Models;
using System.Web.Script.Serialization;

namespace LibraryCMS.Controllers
{
    public class LocationController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static LocationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44329/api/locationdata/");
        }

        // GET: Location/List
        public ActionResult List()
        {
            //retrieve list of members from member api
            //curl https://localhost:44329/api/locationdata/listlocations

            string url = "listlocations";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is " + response.StatusCode);

            IEnumerable<LocationDto> locations = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            Debug.WriteLine("The number of locations is: " + locations.Count());

            return View(locations);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Location/Details/5
        public ActionResult Details(int id)
        {

            //retrieve data from selected member 
            //curl https://localhost:44329/api/memberdata/findmember/{id}

            string url = "findlocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is " + response.StatusCode);

            LocationDto selectedlocation = response.Content.ReadAsAsync<LocationDto>().Result;
            Debug.WriteLine("The location selected is " + selectedlocation.LocationName);

            return View();
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(Location location)
        {
            Debug.WriteLine("the new location is: " + location.LocationName);
            string url = "addlocation";

            string jsonpayload = jss.Serialize(location);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Location/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
