using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SynergySolution.Controllers;
using SynergySolution.Models;
using System.Web.Security;

namespace SynergySolution.Controllers
{
    public class HomeController : Controller
    {
        private SynergySolutionEntities1 db = new SynergySolutionEntities1();
        #region Login and Logout

        public ActionResult Index()
        { 
            return View();
        }
        [HttpPost]  
        public ActionResult Index(User user)
        {
            if (ModelState.IsValid) //i.e. if email and password are entered
            {
                if (IsValid(user.Email, user.Password)) //If details are correct.
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    User tempUser = db.Users.Where(m => m.Email == user.Email).First(); //Find the user who has been logged in using linq

                    //Set variables that exists for all the different types of users
                    Session["UserID"] = tempUser.UserId;
                    Session["Role"] = tempUser.Role;

                    //If statements to redirect to different page depending on type of user and set appropriate session variables.

                    if (tempUser.Role == 1)          //If agile team member logs in:
                    {
                        AgileTeamMember tempAgileTeamMember = db.AgileTeamMembers.Where(m => m.UserId == tempUser.UserId).First();//Find specific agile team member in db
                        //set session variables that dependent on agile team member attributes
                        Session["AgileTeamMemberID"] = tempAgileTeamMember.TeamMemberId;
                        return RedirectToAction("Details", "AgileTeamMembers", new { id = tempAgileTeamMember.TeamMemberId });//display thier profile page 
                    }

                    else if (tempUser.Role == 2) // If Line Manager logs in
                    {
                        LineManager tempLineManager = db.LineManagers.Where(m => m.UserId == tempUser.UserId).First();
                        Session["LineManagerID"] = tempLineManager.LineManagerId;
                        return RedirectToAction("Details", "LineManagers", new { id = tempLineManager.LineManagerId });
                    }

                    else if (tempUser.Role == 3) // If Shareholder logs in
                    {
                        Shareholder tempShareholder= db.Shareholders.Where(m => m.UserId == tempUser.UserId).First();
                        Session["ShareholderID"] = tempShareholder.ShareholderId;
                        return RedirectToAction("Details", "Shareholders", new { id = tempShareholder.ShareholderId });
                    }

                    else if (tempUser.Role == 4) // If Admin logs in
                    {
                        Admin tempAdmin = db.Admins.Where(m => m.UserId == tempUser.UserId).First();
                        Session["AdminID"] = tempAdmin.AdminId;
                        return RedirectToAction("Details", "Admins", new { id = tempAdmin.AdminId });
                    }

                    return RedirectToAction("Index", "Home"); //If they do not enter both a email address and password, refresh the page
                }
                else
                {
                    ModelState.AddModelError("", "Login Data is incorrect."); //If login data is incorrect, display error message
                }
            }
            return View(user);
        }

        //This is used for both change password and login. It checks that the password that has been entered matches the encrypted password that is stored in the database for that user
        public bool IsValid(string email, string password)
        {
            //Create instance of class related to the plugin used
            var crypto = new SimpleCrypto.PBKDF2();

            bool isValid = false; //method level true/false variable that will be returned depending on if the password is correct

            var user = db.Users.FirstOrDefault(u => u.Email == email); //Can use this to find the user as it is unique.
            if (user != null) //If the user exists, then:
            {
                if (user.Password == crypto.Compute(password, user.PasswordSalt)) //Check if the password saved in the database (which is encrytped) matches the newly encrypted password that has been passed to the method
                {
                    isValid = true; //then set return variable to true
                }
            }
            return isValid;
        }
        #endregion
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}