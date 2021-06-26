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
    public class BookController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44329/api/");
        }

        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;


            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);
            Debug.WriteLine("Token Submitted is : " + token);

            return;
        }

        // GET: Book/List
        public ActionResult List()
        {
            //retrieve list of members from member api
            //curl https://localhost:44329/api/bookdata/listbooks

            string url = "bookdata/listbooks";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is " + response.StatusCode);

            IEnumerable<BookDto> books = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;
            Debug.WriteLine("The number of members is: " + books.Count());

            return View(books);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            DetailsBook ViewModel = new DetailsBook();

            string url = "bookdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is " + response.StatusCode);

            BookDto SelectedBook = response.Content.ReadAsAsync<BookDto>().Result;
            Debug.WriteLine("The book selected is " + SelectedBook.BookTitle);

            ViewModel.SelectedBook = SelectedBook;

            //list of locations that have the selected book
            url = "locationdata/listlocationsforbooks/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> CurrentLocation = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            ViewModel.CurrentLocation = CurrentLocation;

            //list of locations that does not have the selected book
            url = "locationdata/listlocationswithnobooks/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> OtherLocation = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            ViewModel.OtherLocation = OtherLocation;

            return View(ViewModel);
        }

        //POST: Book/Associate/{bookid}
        [HttpPost]
        [Authorize]
        public ActionResult Associate(int id, int LocationId)
        {
            GetApplicationCookie();
            Debug.WriteLine("Associating Book #" + id + " with Location #" + LocationId);

            string url = "bookdata/associatebookwithlocation/" + id + "/" + LocationId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //Get: Book/UnAssociate/{id}?LocationId={LocationId}
        [HttpGet]
        [Authorize]
        public ActionResult UnAssociate(int id, int LocationId)
        {
            GetApplicationCookie();
            Debug.WriteLine("Unassociating Book #" + id + " with Location #" + LocationId);

            string url = "bookdata/unassociatebookwithlocation/" + id + "/" + LocationId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        // GET: Book/Create
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Book book)
        {
            GetApplicationCookie();
            
            string url = "bookdata/addbook";
            Debug.WriteLine("the new book is: " + book.BookTitle);

            string jsonpayload = jss.Serialize(book);

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

        // GET: Book/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "bookdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is " + response.StatusCode);

            BookDto selectedbook = response.Content.ReadAsAsync<BookDto>().Result;
            return View(selectedbook);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Book book)
        {
            GetApplicationCookie();

            string url = "bookdata/updatebook/" + id;

            string jsonpayload = jss.Serialize(book);
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

        // GET: Book/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "bookdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is " + response.StatusCode);

            BookDto selectedbook = response.Content.ReadAsAsync<BookDto>().Result;

            return View(selectedbook);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();

            string url = "bookdata/deletebook/"+id;

            string jsonpayload = jss.Serialize("");

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
    }
}
