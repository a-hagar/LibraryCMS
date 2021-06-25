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
    public class BookDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/BookData/ListBooks
        [HttpGet]
        public IEnumerable<BookDto> ListBooks()
        {
            List<Book> Books = db.Books.ToList();
            List<BookDto> BookDtos = new List<BookDto>();

            Books.ForEach(b => BookDtos.Add(new BookDto(){
                BookId = b.BookId,
                BookTitle = b.BookTitle,
                AuthorFname = b.AuthorFname,
                AuthorLname = b.AuthorLname,
                genre = b.genre,
                ISBN = b.ISBN,
                Publisher = b.Publisher,
                PublicationDate = b.PublicationDate
            }));

            return BookDtos;
        }


        // GET: api/BookData/ListBooksForLocations/1
        [HttpGet]
        [ResponseType(typeof(BookDto))]
        public IHttpActionResult ListBooksForLocations(int id)
        {
            //locations that have books that match the selected id
            List<Book> Books = db.Books.Where(
                b => b.Location.Any(
                l => l.LocationId == id)
            ).ToList();
            List<BookDto> BookDtos = new List<BookDto>();

            Books.ForEach(b => BookDtos.Add(new BookDto()
            {
                BookId = b.BookId,
                BookTitle = b.BookTitle,
                AuthorFname = b.AuthorFname,
                AuthorLname = b.AuthorLname,
                genre = b.genre,
                ISBN = b.ISBN,
                Publisher = b.Publisher,
                PublicationDate = b.PublicationDate
            }));

            return Ok(BookDtos);
        }

        // GET: api/BookData/FindBook/5
        [ResponseType(typeof(Book))]
        [HttpGet]
        public IHttpActionResult FindBook(int id)
        {
            Book book = db.Books.Find(id);
            BookDto BookDto = new BookDto()
            {
                BookId = book.BookId,
                BookTitle = book.BookTitle,
                AuthorFname = book.AuthorFname,
                AuthorLname = book.AuthorLname,
                genre = book.genre,
                Publisher = book.Publisher,
                PublicationDate = book.PublicationDate
            };
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/BookData/UpdateBook/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBook(int id, Book book)
        {
            Debug.WriteLine("Updating Book!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.BookId)
            {
                Debug.WriteLine("The book id is " + id);
                Debug.WriteLine("The other book id is " + book.BookId);
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /*
        [HttpPost]
        //public IHttpActionResult UpdateBookPic(int id)
        {
            
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                debug.WriteLine("Testing file upload...");
                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

            if(numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var bookPic = HttpContext.Current.Request.Files[0];
                    if (animalPic.ContentLength > 0)
                    {
                        var valtypes = new[] {"jpeg", "jpg", "png", "gif"};  
                        var extension = Path.GetExtension(bookPic.FileName).Substring(1);
                    }
                }


            }

                

             

           return id;
        }
        */

        // POST: api/BookData/AddBook
        [ResponseType(typeof(Book))]
        [HttpPost]
        public IHttpActionResult AddBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.BookId }, book);
        }

        // DELETE: api/BookData/DeleteBook/5
        [ResponseType(typeof(Book))]
        [HttpPost]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.BookId == id) > 0;
        }
    }
}