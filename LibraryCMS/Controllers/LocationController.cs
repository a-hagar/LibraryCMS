using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using LibraryCMS.Models;
using LibraryCMS.Models.ViewModels;
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
            client.BaseAddress = new Uri("https://localhost:44329/api/");
        }

        // GET: Location/List
        public ActionResult List()
        {
            //retrieve list of members from member api
            //curl https://localhost:44329/api/locationdata/listlocations

            string url = "locationdata/listlocations";
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
            DetailsLocation ViewModel = new DetailsLocation();

            //retrieve data from selected member 
            //curl https://localhost:44329/api/memberdata/findmember/{id}

            string url = "locationdata/findlocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is " + response.StatusCode);

            LocationDto SelectedLocation = response.Content.ReadAsAsync<LocationDto>().Result;
            Debug.WriteLine("The location selected is " + SelectedLocation.LocationName);
            ViewModel.SelectedLocation = SelectedLocation;

            //shows all members with the location chosen as their preference
            url = "memberdata/listmembersforlocation/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MemberDto> RelatedMembers = response.Content.ReadAsAsync<IEnumerable<MemberDto>>().Result;

            ViewModel.RelatedMembers = RelatedMembers;

            //list of all books at the selected location
            url = "bookdata/listbooksforlocations/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<BookDto> BookSelection = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;
            ViewModel.BookSelection = BookSelection;

            return View(ViewModel);
        }

        // GET: Location/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(Location location)
        {
            Debug.WriteLine("the new location is: " + location.LocationName);
            string url = "locationdata/addlocation";

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
        public ActionResult Edit(int id, Location location)
        {
            string url = "locationdata/updatelocation/" + id;

            string jsonpayload = jss.Serialize(location);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Location/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "locationdata/deletelocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LocationDto selectedlocation = response.Content.ReadAsAsync<LocationDto>().Result;
            return View(selectedlocation);
        }

        // POST: Location/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "locationdata/deletelocation/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
