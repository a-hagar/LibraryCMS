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
    public class MemberDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/MemberData/ListMembers
        [HttpGet]
        public IEnumerable<MemberDto> ListMembers()
        {
            List<Member> Members = db.Members.ToList();
            List<MemberDto> MemberDtos = new List<MemberDto>();

            Members.ForEach(m => MemberDtos.Add(new MemberDto(){
                MemberId = m.MemberId,
                FirstName = m.FirstName,
                LastName = m.LastName,
                PhoneNum = m.PhoneNum,
                eMail = m.eMail,
                RegistrationDate = m.RegistrationDate,
            }));

            return MemberDtos;
        }

        // GET: api/MemberData/FindMember/5
        [ResponseType(typeof(Member))]
        [HttpGet]
        public IHttpActionResult FindMember(int id)
        {
            Member member = db.Members.Find(id);
            MemberDto MemberDto = new MemberDto()
            {
                MemberId = member.MemberId,
                FirstName = member.FirstName,
                LastName = member.LastName,
                PhoneNum = member.PhoneNum,
                eMail = member.eMail,
                RegistrationDate = member.RegistrationDate,
            };
            if (member == null)
            {
                return NotFound();
            }

            return Ok(MemberDto);
        }

        // POST: api/MemberData/UpdateMember/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMember(int id, Member member)
        {
            Debug.WriteLine("Updating Animal!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != member.MemberId)
            {
                Debug.WriteLine("The member id is " + id);
                Debug.WriteLine("The other member id is " + member.MemberId);
                return BadRequest();
            }

            db.Entry(member).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("There are no errors");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/MemberData/AddMember
        [ResponseType(typeof(Member))]
        [HttpPost]
        public IHttpActionResult AddMember(Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Members.Add(member);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = member.MemberId }, member);
        }

        // POST: api/MemberData/DeleteMember/5
        [ResponseType(typeof(Member))]
        [HttpPost]
        public IHttpActionResult DeleteMember(int id)
        {
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }

            db.Members.Remove(member);
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

        private bool MemberExists(int id)
        {
            return db.Members.Count(e => e.MemberId == id) > 0;
        }
    }
}