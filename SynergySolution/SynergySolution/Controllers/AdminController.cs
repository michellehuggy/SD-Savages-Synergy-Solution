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
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using SynergySolution.ModelHelper;

namespace SynergySolution.Controllers
{
    public class AdminController : Controller
    {
        SDSavagesEntities db = new SDSavagesEntities();
        public ActionResult AddEmploy(EmployeeInfodetail employeeInfodetail)
        {
            string newPassword = GetUniqueKey(9);

            if (employeeInfodetail != null)
            {
                EmployLogin employLogin = new EmployLogin();
                employLogin.email = employeeInfodetail.email;
                employLogin.pasword = newPassword;
                db.EmployLogins.Add(employLogin);
                db.SaveChanges();

                EmployInfo employInfo = new EmployInfo();
                employInfo.fName = employeeInfodetail.fName;
                employInfo.lName = employeeInfodetail.lName;
                employInfo.dob = employeeInfodetail.dob;
                employInfo.phone = employeeInfodetail.phone;
                employInfo.email = employeeInfodetail.email;
                employInfo.loginId = employLogin.loginId;
                db.EmployInfoes.Add(employInfo);
                db.SaveChanges();

                EmplyDesignation employDesignation = new EmplyDesignation();
                employDesignation.employId = employInfo.employId;
                employDesignation.designationId = employeeInfodetail.designationId;
                employDesignation.isPartOfTeam = false;
                db.EmplyDesignations.Add(employDesignation);
                db.SaveChanges();

                string designation = (from a in db.Designations where a.designationId == employDesignation.designationId select a.designationName).FirstOrDefault();

                bool check = SendMail(employeeInfodetail.email, newPassword, employInfo.fName, designation);
                if (!check)
                {
                    ViewBag.mailError = "Sending Failed due to some reason. Try again";
                    return View();
                }
                else
                {
                    return RedirectToAction("ShowEmplyee", "Admin");
                }

            }

            else
            {
                return View();
            }
        }

        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                //crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetBytes(data); //GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                //result.Append((UInt32)(BitConverter.ToUInt32(data, 4 * maxSize) % (chars.Length)));
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public bool SendMail(string email, string newPassword, string name, string role)
        {
            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            //bool enableSSL = false;
            bool result = false;

            string emailFrom = "synergysolution1@gmail.com";
            string password = "samplepassword";
            string emailTo = email.ToString();
            string subject = "Synergy Solution Account Creation";
            string body = "<b>Hello " + name + "</b>,<br>You have been added to the system as a " + role + ", your password is " + newPassword + " please log onto the system using the provided password.<br><br>After you have logged on for the first time, you will be prompted to change your password.<br><br>Thanks<br>Synergy Solution Team";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;


                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    // smtp.EnableSsl = enableSSL;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    result = true;
                }
            }
            return result;
        }

        public static AddTeamMemberList addTeamMemberList = new AddTeamMemberList();

        public ActionResult AddDesigination()
        {
            //AddTeamMemberList addTeamMemberList = new AddTeamMemberList();
            ViewBag.User = new SelectList(db.AgileTeams, "teamId", "teamName");
            var getTeamMemberList = (from a in db.EmplyDesignations
                                     where a.designationId == 2
                                     select a).ToList();
            addTeamMemberList.selectedAgileTeamMemberId = new List<int>();
            addTeamMemberList.selectedLineManagerId = new List<int>();
            addTeamMemberList.selectedAgileTeamMemberName = new List<string>();
            addTeamMemberList.selectedLineManagerName = new List<string>();
            if (getTeamMemberList.Count != 0)
            {
                addTeamMemberList.agileTeamMemberList = new List<AgileTeamMemberList>();
                foreach (var item in getTeamMemberList)
                {
                    AgileTeamMemberList agileTeamMemberList = new AgileTeamMemberList();
                    agileTeamMemberList.employId = item.employId;
                    agileTeamMemberList.name = item.EmployInfo.fName + " " + item.EmployInfo.lName;
                    addTeamMemberList.agileTeamMemberList.Add(agileTeamMemberList);
                }
            }
            ViewBag.selectedAgileTeamMember = new SelectList(addTeamMemberList.agileTeamMemberList, "employId", "name");

            var getLineManagerList = (from a in db.EmplyDesignations
                                      where a.designationId == 1
                                      select a).ToList();
            if (getLineManagerList.Count != 0)
            {
                addTeamMemberList.lineManagerList = new List<LineMemberList>();
                foreach (var item in getLineManagerList)
                {
                    LineMemberList agileLineMemberList = new LineMemberList();
                    agileLineMemberList.employId = item.employId;
                    agileLineMemberList.name = item.EmployInfo.fName + " " + item.EmployInfo.lName;
                    addTeamMemberList.lineManagerList.Add(agileLineMemberList);
                }
            }
            ViewBag.selectedLineManager = new SelectList(addTeamMemberList.lineManagerList, "employId", "name");

            return View();
        }

        public static List<AgileTeamMemberList> agileTeamMemberList = new List<AgileTeamMemberList>();

        public static List<LineMemberList> lineMemberList = new List<LineMemberList>();

        public ActionResult _AddTeamMember(int employId)
        {
            if (agileTeamMemberList.Count != 0)
            {
                int check = agileTeamMemberList.Where(x => x.employId == employId).Select(x => x.employId).FirstOrDefault(); //addTeamMemberList.selectedLineManagerId.Contains(employId);
                if (check == 0)
                {
                    AgileTeamMemberList add = new AgileTeamMemberList();
                    add.employId = employId;
                    add.name = (from a in db.EmployInfoes where a.employId == employId select a.fName).FirstOrDefault();
                    agileTeamMemberList.Add(add);
                    return View(agileTeamMemberList);
                }
                else
                {
                    return View(agileTeamMemberList);
                }
            }
            else
            {
                AgileTeamMemberList add = new AgileTeamMemberList();
                add.employId = employId;
                add.name = (from a in db.EmployInfoes where a.employId == employId select a.fName).FirstOrDefault();
                agileTeamMemberList.Add(add);
                return View(agileTeamMemberList);
            }
        }

        public ActionResult _AddLineManager(int employId)
        {
            if (lineMemberList.Count != 0)
            {
                int check = lineMemberList.Where(x => x.employId == employId).Select(x => x.employId).FirstOrDefault(); //addTeamMemberList.selectedLineManagerId.Contains(employId);
                if (check == 0)
                {
                    LineMemberList add = new LineMemberList();
                    add.employId = employId;
                    add.name = (from a in db.EmployInfoes where a.employId == employId select a.fName).FirstOrDefault();
                    lineMemberList.Add(add);
                    return View(lineMemberList);
                }
                else
                {
                    return View(lineMemberList);
                }
            }
            else
            {
                LineMemberList add = new LineMemberList();
                add.employId = employId;
                add.name = (from a in db.EmployInfoes where a.employId == employId select a.fName).FirstOrDefault();
                lineMemberList.Add(add);
                return View(lineMemberList);
            }
        }

        public ActionResult _DeleteTeamMember(int employId)
        {
            AgileTeamMemberList delete = agileTeamMemberList.Where(x => x.employId == employId).FirstOrDefault();
            agileTeamMemberList.Remove(delete);
            return View(agileTeamMemberList);
        }

        public ActionResult _DeleteLineMan(int employId)
        {
            LineMemberList delete = lineMemberList.Where(x => x.employId == employId).FirstOrDefault();
            lineMemberList.Remove(delete);
            return View(lineMemberList);
        }

        public ActionResult DeleteTeamMember(int employId)
        {
            var remove = addTeamMemberList.selectedAgileTeamMemberId.Remove(employId);
            remove = addTeamMemberList.selectedAgileTeamMemberName.Remove((from a in db.EmployInfoes where a.employId == employId select a.fName).FirstOrDefault());
            return View(addTeamMemberList);
        }

        public ActionResult DeleteLineManager(int employId)
        {
            var remove = addTeamMemberList.selectedLineManagerId.Remove(employId);
            remove = addTeamMemberList.selectedLineManagerName.Remove((from a in db.EmployInfoes where a.employId == employId select a.fName).FirstOrDefault());
            return View(addTeamMemberList);
        }

        public ActionResult AddTeam(AgileTeam model)
        {
            AgileTeam agileTeam = new AgileTeam();
            agileTeam.teamName = model.teamName;
            agileTeam.teamDescription = model.teamDescription;
            db.AgileTeams.Add(agileTeam);
            db.SaveChanges();
            return RedirectToAction("ShowTeam", "Admin");
        }

        public ActionResult CreateTeam(FormCollection fm)
        {
            AgileTeamMemberDetail agileTeamMemberDetail = new AgileTeamMemberDetail();
            foreach (var item in agileTeamMemberList)
            {
                agileTeamMemberDetail.teamId = Convert.ToInt16(fm["Team"]);
                agileTeamMemberDetail.employId = item.employId;
                db.AgileTeamMemberDetails.Add(agileTeamMemberDetail);
                db.SaveChanges();

                var addToTeam = (from a in db.EmplyDesignations
                                 where a.employId == item.employId
                                 select a).FirstOrDefault();
                addToTeam.isPartOfTeam = true;
                db.SaveChanges();
            }
            foreach (var item in lineMemberList)
            {
                agileTeamMemberDetail.teamId = Convert.ToInt16(fm["Team"]);
                agileTeamMemberDetail.employId = item.employId;
                db.AgileTeamMemberDetails.Add(agileTeamMemberDetail);
                db.SaveChanges();

                var addToTeam = (from b in db.EmplyDesignations
                                 where b.employId == item.employId
                                 select b).FirstOrDefault();
                addToTeam.isPartOfTeam = true;
                db.SaveChanges();
            }

            return RedirectToAction("ShowAllTeams");
        }
        public ActionResult ShowAllTeams()
        {
            var team = (from e in db.AgileTeamMemberDetails
                        join f in db.AgileTeams
                        on e.teamId equals f.teamId
                        select new EditTeamClass
                        {
                            teamId = f.teamId,
                            teamName = f.teamName,
                            employId = e.employId,
                            employName = e.EmployInfo.fName,
                            employDesignation = e.EmployInfo.EmplyDesignations.FirstOrDefault().Designation.designationName
                        }
                        ).ToList();

            return View(team);
        }
        public ActionResult EditEmploy(EmployeeInfodetail employ)
        {
            var updateEmploy = (from a in db.EmployInfoes
                                where a.employId == employ.employId
                                select a).FirstOrDefault();
            if (updateEmploy != null)
            {
                updateEmploy.fName = employ.fName;
                updateEmploy.lName = employ.lName;
                updateEmploy.dob = employ.dob;
                updateEmploy.phone = employ.phone;
                db.SaveChanges();

                var changeDesignation = (from b in db.EmplyDesignations
                                         where b.employId == employ.employId
                                         select b).FirstOrDefault();
                changeDesignation.designationId = employ.designationId;
                db.SaveChanges();
            }
            return RedirectToAction("ShowEmplyee");
        }

        public ActionResult DeleteEmploy(int employId)
        {

            var deleteDesignation = (from b in db.EmplyDesignations
                                     where b.employId == employId
                                     select b).FirstOrDefault();
            if (deleteDesignation.isPartOfTeam.Value)
            {
                var deleteFromTeam = (from c in db.AgileTeamMemberDetails
                                      where c.employId == employId
                                      select c).ToList();
                db.AgileTeamMemberDetails.RemoveRange(deleteFromTeam);
                db.SaveChanges();
            }

            db.EmplyDesignations.Remove(deleteDesignation);
            db.SaveChanges();

            var deleteEmploy = (from a in db.EmployInfoes
                                where a.employId == employId
                                select a).FirstOrDefault();
            db.EmployInfoes.Remove(deleteEmploy);
            db.SaveChanges();
            return RedirectToAction("ShowEmplyee");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(FormCollection au)
        {
            string user = au[0];
            string pass = au[1];
            using (db)
            {
                var usr = (from e in db.AdminLogins
                           where e.email == user && e.pasword == pass
                           select e).FirstOrDefault();

                if (usr != null)
                {
                    //Session["password"] = pass;
                    Session["username1"] = user;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {

                    ViewBag.abs = "Enter Correct Username and Password";
                    return View();
                }
            }
        }
        // GET: Admins
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddEmployee()
        {
            ViewBag.desi = new SelectList(db.Designations, "designationId", "designationName");
            return View();
        }
        public ActionResult AddCompany()
        {
            return View();
        }
        //public ActionResult AddDesigination()
        //{
        //    ViewBag.User = new SelectList(db.AgileTeams, "teamId", "teamName");
        //    ViewBag.mangr = new SelectList(db.AgileTeams, "teamId", "teamName");
        //    ViewBag.team = new SelectList(db.AgileTeams, "teamId", "teamName");
        //    return View();
        //}
        public ActionResult ShowEmplyee()
        {
            var emp = (from e in db.EmployInfoes
                       join f in db.EmplyDesignations
                       on e.employId equals f.employId
                       select new EmployeeInfodetail
                       {
                           employId = e.employId,
                           fName = e.fName,
                           lName = e.lName,
                           phone = e.phone,
                           dob = e.dob,
                           email = e.email,
                           designationId = f.designationId,
                           designation = f.Designation.designationName



                       }
                       ).ToList();
            return View(emp);
        }

        ////////////////////////////Employeeeee Edit Delete  ////////////////
        public ActionResult EditEmployee(int? id)
        {
            ViewBag.desi = new SelectList(db.Designations, "designationId", "designationName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EmployInfo employInfo = db.EmployInfoes.Find(id);
            //if (employInfo == null)
            //{
            //    return HttpNotFound();
            //}
            var emp = (from e in db.EmployInfoes
                       join f in db.EmplyDesignations
                       on e.employId equals f.employId
                       where e.employId == id.Value
                       select new EmployeeInfodetail
                       {
                           employId = e.employId,
                           fName = e.fName,
                           lName = e.lName,
                           phone = e.phone,
                           dob = e.dob,
                           email = e.email,
                           designationId = f.designationId,
                           designation = f.Designation.designationName,
                           loginId = e.loginId
                       }
                       ).FirstOrDefault();
            ViewBag.loginId = new SelectList(db.EmployLogins, "loginId", "email", emp.loginId);

            return View(emp);
            //return View(employInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(EmployInfo employInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.loginId = new SelectList(db.EmployLogins, "loginId", "email", employInfo.loginId);
            return View(employInfo);
        }

        public ActionResult DeleteEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployInfo employInfo = db.EmployInfoes.Find(id);
            if (employInfo == null)
            {
                return HttpNotFound();
            }
            return View(employInfo);
        }

        public ActionResult DeleteEmployee(int id)
        {
            EmployInfo employInfo = db.EmployInfoes.Find(id);
            db.EmployInfoes.Remove(employInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /////////////////////////////Team///////////////////////
        public ActionResult ShowTeam()
        {
            return View(db.AgileTeams.ToList());
        }
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
                return RedirectToAction("ShowTeam");
            }
            return View(agileTeam);
        }

       

        public ActionResult DeleteTeam(int id)
        {
            AgileTeam agileTeam = db.AgileTeams.Find(id);
            db.AgileTeams.Remove(agileTeam);
            db.SaveChanges();
            return RedirectToAction("ShowTeam");
        }
        
        public ActionResult DeleteMemberFromTeam(int teamId, int employId)
        {
            AgileTeamMemberDetail delete = (from a in db.AgileTeamMemberDetails
                                            where a.teamId == teamId && a.employId == employId
                                            select a).FirstOrDefault();
            db.AgileTeamMemberDetails.Remove(delete);
            db.SaveChanges();

            var count = (from b in db.AgileTeamMemberDetails
                         where b.employId == employId
                         select b).ToList();
            if(!(count.Count > 0))
            {
                var change = (from c in db.EmplyDesignations
                              where c.employId == employId
                              select c).FirstOrDefault();
                change.isPartOfTeam = false;
                db.SaveChanges();
            }

            return RedirectToAction("ShowAllTeams");
        }
    }
}
