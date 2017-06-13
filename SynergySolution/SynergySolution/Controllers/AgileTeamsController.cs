using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SynergySolution.Models;

namespace SynergySolution.Controllers
{
    public class AgileTeamsController : Controller
    {
        private SDSavagesEntities db = new SDSavagesEntities();

        // GET: AgileTeams
        public ActionResult ShowTeam()
        {
            return View(db.AgileTeams.ToList());
        }

        // GET: AgileTeams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgileTeam agileTeam = db.AgileTeams.Find(id);
            if (agileTeam == null)
            {
                return HttpNotFound();
            }
            return View(agileTeam);
        }

        // GET: AgileTeams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AgileTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "teamId,teamName,teamDescription")] AgileTeam agileTeam)
        {
            if (ModelState.IsValid)
            {
                db.AgileTeams.Add(agileTeam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agileTeam);
        }

        // GET: AgileTeams/Edit/5
        public ActionResult EditTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgileTeam agileTeam = db.AgileTeams.Find(id);
            if (agileTeam == null)
            {
                return HttpNotFound();
            }
            return View(agileTeam);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTeam(AgileTeam agileTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agileTeam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agileTeam);
        }

       
        public ActionResult DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgileTeam agileTeam = db.AgileTeams.Find(id);
            if (agileTeam == null)
            {
                return HttpNotFound();
            }
            return View(agileTeam);
        }

        
        public ActionResult DeleteTeam(int id)
        {
            AgileTeam agileTeam = db.AgileTeams.Find(id);
            db.AgileTeams.Remove(agileTeam);
            db.SaveChanges();
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
