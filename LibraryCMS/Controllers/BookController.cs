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
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44329/api/");
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

            return View(ViewModel);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult Create(Book book)
        {
            Debug.WriteLine("the new book is: " + book.BookTitle);
            string url = "bookdata/addbook";

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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Book/Edit/5
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

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Book/Delete/5
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
