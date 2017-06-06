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
    public class UserController : Controller
    {
        private SynergySolutionEntities1 db = new SynergySolutionEntities1();

        // GET: User
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        #region Change Password
        public ActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword, int userID)
        {
            try
            {
                //Find user in DB
                User user = db.Users.Find(userID);

                string email = user.Email;
                string OldPassword = user.Password;

                HomeController HC = new HomeController();


                bool oldPasswordvalid = HC.IsValid(email, OldPassword);


                //check that old password is the same as existing password in DB
                if (oldPasswordvalid)
                {

                    //check if new and confirm password are the same
                    if (newPassword == confirmPassword)
                    {
                        user.Password = newPassword;

                        var crypto = new SimpleCrypto.PBKDF2();

                        var encrypPass = crypto.Compute(user.Password);

                        user.Password = encrypPass;
                        user.PasswordSalt = crypto.Salt;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        return Json(new { ok = true });

                    }
                    else
                    {
                        //messaga that new and confirm password do not match
                        return Json(new { ok = false, message = "New passwords do not match" });

                    }
                }
                else
                {
                    return Json(new { ok = false, message = "Old password is incorrect" });
                }

            }
            catch (Exception ex)
            {
                //unexpected error exception
                return Json(new { ok = false, message = ex.Message });
            }

        }


        #endregion
        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public int Create(User user)
        {
            if (ModelState.IsValid)
            {
                var crypto = new SimpleCrypto.PBKDF2();

                var encrypPass = crypto.Compute(user.Password);

                user.Password = encrypPass;
                user.PasswordSalt = crypto.Salt;

                db.Users.Add(user);
                db.SaveChanges();
                return user.UserId;
            }
            else return 0;
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Edit( User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChangesAsync(); 
            }
            
        }

        // GET: User/Delete/5
        public ActionResult Delete (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        private bool PasswordIsValid(string password, int userID)
        {
            var crypto = new SimpleCrypto.PBKDF2();

            bool isValid = false;

            var user = db.Users.FirstOrDefault(u => u.UserId == userID);
            if (user != null)
            {
                string temp = crypto.Compute(password, user.PasswordSalt);
                if (user.Password == crypto.Compute(password, user.PasswordSalt))
                {
                    isValid = true;
                }

            }
            return isValid;
        }
    }
}
