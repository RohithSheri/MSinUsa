using MsInUsa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Cryptography;
using System.IO;

namespace MsInUsa.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserProfile userprofile)
        {
                using (MsInUsEntities msInUsConnection = new MsInUsEntities())
                {
                    if (!msInUsConnection.UserProfiles.Any(m => m.Email_Id == userprofile.Email_Id))
                    {
                        TempData["ErrorLogin"] = "Invalid Credentials";
                        return RedirectToAction("LogIn", "Account");
                    }
                    string userpassword = msInUsConnection.UserProfiles.First(m => m.Email_Id == userprofile.Email_Id).Password;
                    if (userpassword != null && userprofile.Password == MyEncryption.DecodeFrom64(userpassword))
                    {
                        removeUserSession();
                        Session.Add("UserName", msInUsConnection.UserProfiles.First(m => m.Email_Id == userprofile.Email_Id).FirstName);
                        Session.Add("UserId", msInUsConnection.UserProfiles.First(m => m.Email_Id == userprofile.Email_Id).Id);
                        return RedirectToAction("Index", "Home"); 
                    }
                }
                TempData["ErrorLogin"] = "Invalid Credentials";
                return RedirectToAction("LogIn", "Account");
        }

        public ActionResult LogOut()
        {
            removeUserSession();
            return RedirectToAction("LogIn", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserProfile userprofile)
        {
            if (userprofile!=null)
                {
                    using (MsInUsEntities msInUsConnection = new MsInUsEntities())
                    {
                        int rowcount = msInUsConnection.UserProfiles.Count();
                        userprofile.Password = MyEncryption.EncodePasswordToBase64(userprofile.Password);
                        userprofile.ConfirmPassword = MyEncryption.EncodePasswordToBase64(userprofile.ConfirmPassword);
                        userprofile.Email_Id = userprofile.Email_IdRegister;
                        msInUsConnection.UserProfiles.Add(userprofile);
                        msInUsConnection.SaveChanges();
                        TempData["ConfirmationMessage"] = "Registered Successfully";
                        //Session.Add("Registration Message", "Registered Successfully");
                        if (rowcount < msInUsConnection.UserProfiles.Count()) { return RedirectToAction("LogIn", "Account"); }
                    }
                }
            return RedirectToAction("NotFound", "Error");
            }

        public JsonResult isUserAvailable(string Email_IdRegister)
        {
            MsInUsEntities connection = new MsInUsEntities();
            return Json(!connection.UserProfiles.Any(m => m.Email_Id == Email_IdRegister), JsonRequestBehavior.AllowGet);
        }

        void removeUserSession()
        {
            if (Session["UserName"] != null) Session.RemoveAll();
        }

        [UserAuthorize]
        public ActionResult settings()
        {
             MsInUsEntities connection = new MsInUsEntities();
             List<SelectListItem> year = new List<SelectListItem>();
                for (int p = DateTime.Today.Year - 40; p < DateTime.Today.Year - 16; p++)
                { year.Add(new SelectListItem() { Text = Convert.ToString(p), Value = Convert.ToString(p), Selected = (DateTime.Today.Year - 14 == p) }); }
                UserProfile userprofile = new UserProfile();
                FullProfile fullprofile = new FullProfile();
                ProfilesController pc = new ProfilesController();
                int id = Convert.ToInt32(Session["UserId"]);
                userprofile = connection.UserProfiles.Find(keyValues: id);
                if (userprofile.DOB != null)
                {
                    userprofile.DOBMonth = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(userprofile.DOB.Value.Month);
                    userprofile.DOBYear = userprofile.DOB.Value.Year;
                    userprofile.DOBday = userprofile.DOB.Value.Day;
                }
                pc.initilization(fullprofile);
                userprofile.FullProfile = fullprofile;
                userprofile.doBirthYear = year;
                return View(userprofile);
        }

        [HttpPost]
        public ActionResult settings(UserProfile userprofile)
        {
            try
            {
                if (userprofile != null)
                {
                    string dateobj = userprofile.DOBMonth + "/" + userprofile.DOBday + "/" + userprofile.DOBYear;
                    userprofile.DOB = DateTime.Parse(dateobj);
                    using (MsInUsEntities connection = new MsInUsEntities())
                    {
                        var userprofileobj = connection.UserProfiles.Find(userprofile.Id);
                        userprofileobj.Email_IdRegister = userprofile.Email_Id;
                        userprofileobj.ConfirmPassword = userprofile.Password;
                        connection.Entry(userprofileobj).CurrentValues.SetValues(userprofile);
                        connection.SaveChanges();
                        return RedirectToAction("Myprofile", "Profiles");
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                StreamWriter sw = new StreamWriter(@"D:\MasterStudy\MsInUsa\MsInUsa\Log\logs.txt",true);
                foreach (var c in e.EntityValidationErrors)
                    foreach (var ex in c.ValidationErrors)
                        sw.WriteLine(ex.ErrorMessage);
                sw.Close();
                return RedirectToAction("NotFound", "Error");
            }
            return View();
        }

        [UserAuthorize]
        public ActionResult changepassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult changepassword(UserProfile userprofile)
        {
            try
            {
                MsInUsEntities connection = new MsInUsEntities();
                int id = Convert.ToInt32(Session["UserId"]);
                UserProfile usobj = connection.UserProfiles.SingleOrDefault(m => m.Id == id);
                usobj.Password = MyEncryption.EncodePasswordToBase64(userprofile.Password);
                usobj.ConfirmPassword = MyEncryption.EncodePasswordToBase64(userprofile.ConfirmPassword);
                usobj.Email_IdRegister = usobj.Email_Id;
                connection.SaveChanges();
                TempData["ConfirmationMessage"] = "Password Changed Successfully!";
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                StreamWriter sw = new StreamWriter(@"D:\MasterStudy\MsInUsa\MsInUsa\Log\logs.txt", true);
                foreach (var c in e.EntityValidationErrors)
                    foreach (var ex in c.ValidationErrors)
                        sw.WriteLine(ex.ErrorMessage);
                sw.Close();
                return RedirectToAction("NotFound", "Error");
            }
            return RedirectToAction("changepassword", "Account");
        }

       public class MyEncryption
        {
            public static string DecodeFrom64(string password)
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(password);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }

            public static string EncodePasswordToBase64(string password)
            {
                try
                {
                    byte[] encData_byte = new byte[password.Length];
                    encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                    string encodedData = Convert.ToBase64String(encData_byte);
                    return encodedData;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in base64Encode" + ex.Message);
                }
            }

        }
    }
}

