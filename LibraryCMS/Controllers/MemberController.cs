using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using LibraryCMS.Models;
using System.Web.Script.Serialization;
using LibraryCMS.Models.ViewModels;

namespace LibraryCMS.Controllers
{
    public class MemberController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static MemberController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44329/api/");
        }

        // GET: Member/List
        public ActionResult List()
        {
            //retrieve list of members from member api
            //curl https://localhost:44329/api/memberdata/listmembers

            string url = "memberdata/listmembers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is " + response.StatusCode);

            IEnumerable<MemberDto> members = response.Content.ReadAsAsync<IEnumerable<MemberDto>>().Result;
            Debug.WriteLine("The number of members is: "+ members.Count());


            return View(members);
        }

        // GET: Member/Details/5
        public ActionResult Details(int id)
        {
            //retrieve data from selected member 
            //curl https://localhost:44329/api/memberdata/findmember/{id}

            string url = "memberdata/findmember/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is " + response.StatusCode);

            MemberDto selectedmember = response.Content.ReadAsAsync<MemberDto>().Result;
            Debug.WriteLine("The member selected is " + selectedmember.FirstName +" "+ selectedmember.LastName);


            return View(selectedmember);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Member/Create
        public ActionResult New()
        {
            //get info from locations api to list all locations

            string url = "locationdata/listlocations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> LocationsOptions = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;

            return View(LocationsOptions);
        }

        // POST: Member/Create
        [HttpPost]
        public ActionResult Create(Member member)
        {
            Debug.WriteLine("the new member is: " + member.FirstName + " " + member.LastName);
            string url = "memberdata/addmember";

            string jsonpayload = jss.Serialize(member);

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

        // GET: Member/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateMember ViewModel = new UpdateMember();
            //existing member info
            string url = "memberdata/findmember/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MemberDto Selectedmember = response.Content.ReadAsAsync<MemberDto>().Result;
            ViewModel.SelectedMember = Selectedmember;

            //get location info when updating member
            url = "locationdata/listlocations/";
            response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> LocationsOptions = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            ViewModel.LocationsOptions = LocationsOptions;

            return View(ViewModel);
        }

        // POST: Member/Update/5
        [HttpPost]
        public ActionResult Update(int id, Member member)
        {
            string url = "memberdata/updatemember/" + id;

            string jsonpayload = jss.Serialize(member);
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

        // GET: Member/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "memberdata/findmember" + id ;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MemberDto selectedmember = response.Content.ReadAsAsync<MemberDto>().Result;
            return View(selectedmember);
        }

        // POST: Member/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "memberdata/deletemember/" + id;

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
