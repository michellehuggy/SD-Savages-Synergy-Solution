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
    public class LineManagersController : Controller
    {
        private SynergySolutionEntities1 db = new SynergySolutionEntities1();

        // GET: LineManagers
        public async Task<ActionResult> Index()
        {
            var lineManagers = db.LineManagers.Include(l => l.Team).Include(l => l.User);
            return View(await lineManagers.ToListAsync());
        }

        // GET: LineManagers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineManager lineManager = await db.LineManagers.FindAsync(id);
            if (lineManager == null)
            {
                return HttpNotFound();
            }
            return View(lineManager);
        }

        // GET: LineManagers/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        }

        // POST: LineManagers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LineManagerId,UserId,FirstName,LastName,TeamId")] LineManager lineManager)
        {
            if (ModelState.IsValid)
            {
                db.LineManagers.Add(lineManager);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", lineManager.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", lineManager.UserId);
            return View(lineManager);
        }

        // GET: LineManagers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineManager lineManager = await db.LineManagers.FindAsync(id);
            if (lineManager == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", lineManager.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", lineManager.UserId);
            return View(lineManager);
        }

        // POST: LineManagers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LineManagerId,UserId,FirstName,LastName,TeamId")] LineManager lineManager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lineManager).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", lineManager.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", lineManager.UserId);
            return View(lineManager);
        }

        // GET: LineManagers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineManager lineManager = await db.LineManagers.FindAsync(id);
            if (lineManager == null)
            {
                return HttpNotFound();
            }
            return View(lineManager);
        }

        // POST: LineManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LineManager lineManager = await db.LineManagers.FindAsync(id);
            db.LineManagers.Remove(lineManager);
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
