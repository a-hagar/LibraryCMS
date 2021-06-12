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
    public class MemberController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static MemberController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44329/api/memberdata/");
        }

        // GET: Member/List
        public ActionResult List()
        {
            //retrieve list of members from member api
            //curl https://localhost:44329/api/memberdata/listmembers

            string url = "listmembers";
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

            string url = "findmember/" + id;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        public ActionResult Create(Member member)
        {
            Debug.WriteLine("the new member is: " + member.FirstName + " " + member.LastName);
            string url = "addmember";

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
            return View();
        }

        // POST: Member/Edit/5
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

        // GET: Member/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Member/Delete/5
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
