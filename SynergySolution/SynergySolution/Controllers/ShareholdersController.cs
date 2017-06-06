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
    public class ShareholdersController : Controller
    {
        private SynergySolutionEntities1 db = new SynergySolutionEntities1();

        // GET: Shareholders
        public async Task<ActionResult> Index()
        {
            var shareholders = db.Shareholders.Include(s => s.User);
            return View(await shareholders.ToListAsync());
        }

        // GET: Shareholders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shareholder shareholder = await db.Shareholders.FindAsync(id);
            if (shareholder == null)
            {
                return HttpNotFound();
            }
            return View(shareholder);
        }

        // GET: Shareholders/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        }

        // POST: Shareholders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ShareholderId,UserId,Company,FirstName,LastName")] Shareholder shareholder)
        {
            if (ModelState.IsValid)
            {
                db.Shareholders.Add(shareholder);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", shareholder.UserId);
            return View(shareholder);
        }

        // GET: Shareholders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shareholder shareholder = await db.Shareholders.FindAsync(id);
            if (shareholder == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", shareholder.UserId);
            return View(shareholder);
        }

        // POST: Shareholders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ShareholderId,UserId,Company,FirstName,LastName")] Shareholder shareholder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shareholder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", shareholder.UserId);
            return View(shareholder);
        }

        // GET: Shareholders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shareholder shareholder = await db.Shareholders.FindAsync(id);
            if (shareholder == null)
            {
                return HttpNotFound();
            }
            return View(shareholder);
        }

        // POST: Shareholders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Shareholder shareholder = await db.Shareholders.FindAsync(id);
            db.Shareholders.Remove(shareholder);
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
