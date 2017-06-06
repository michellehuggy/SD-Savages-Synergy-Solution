using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SynergySolution.Models;

namespace SynergySolution.Controllers
{
    public class AgileTeamMembersController : Controller
    {
        private SynergySolutionEntities1 db = new SynergySolutionEntities1();

        // GET: AgileTeamMembers
        public async Task<ActionResult> Index()
        {
            var agileTeamMembers = db.AgileTeamMembers.Include(a => a.Team).Include(a => a.User);
            return View(await agileTeamMembers.ToListAsync());
        }

        // GET: AgileTeamMembers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgileTeamMember agileTeamMember = await db.AgileTeamMembers.FindAsync(id);
            if (agileTeamMember == null)
            {
                return HttpNotFound();
            }
            return View(agileTeamMember);
        }

        // GET: AgileTeamMembers/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        }

        // POST: AgileTeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TeamMemberId,UserId,FirstName,LastName,TeamId")] AgileTeamMember agileTeamMember)
        {
            if (ModelState.IsValid)
            {
                db.AgileTeamMembers.Add(agileTeamMember);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", agileTeamMember.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", agileTeamMember.UserId);
            return View(agileTeamMember);
        }

        // GET: AgileTeamMembers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgileTeamMember agileTeamMember = await db.AgileTeamMembers.FindAsync(id);
            if (agileTeamMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", agileTeamMember.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", agileTeamMember.UserId);
            return View(agileTeamMember);
        }

        // POST: AgileTeamMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TeamMemberId,UserId,FirstName,LastName,TeamId")] AgileTeamMember agileTeamMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agileTeamMember).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", agileTeamMember.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", agileTeamMember.UserId);
            return View(agileTeamMember);
        }

        // GET: AgileTeamMembers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgileTeamMember agileTeamMember = await db.AgileTeamMembers.FindAsync(id);
            if (agileTeamMember == null)
            {
                return HttpNotFound();
            }
            return View(agileTeamMember);
        }

        // POST: AgileTeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AgileTeamMember agileTeamMember = await db.AgileTeamMembers.FindAsync(id);
            db.AgileTeamMembers.Remove(agileTeamMember);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
