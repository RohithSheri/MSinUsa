using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MsInUsa.Model;
using System.Data.Entity.Validation;

namespace MsInUsa.Controllers
{
    [UserAuthorize]
    public class ProfilesController : Controller
    {
        string[] papers = new string[] { "None", "Local", "National", "International" };
        string[] term = new string[] { "Fall", "Spring", "Summmer"};
        MsInUsEntities genConnection = new MsInUsEntities();
       
       //this gets profile of the user
        public ActionResult add()
        {
           int userid = Convert.ToInt32(Session["UserId"]);
           if (!genConnection.FullProfiles.Any(m => m.Id == userid))
           {

               if (Session["AddProfile"] != null && Convert.ToBoolean(Session["AddProfile"]) == false)
               {
                   FullProfile fullprofiles = new FullProfile();
                   initilization(fullprofiles);
                   return View(fullprofiles);
               }
           }
            return RedirectToAction("MyProfile", "Profiles");

        }
        
        
        //this adds profile of the user
        [HttpPost]
        public ActionResult add(FullProfile fullprofile)
        {
            using (MsInUsEntities connection = new MsInUsEntities())
            {
                try
                {
                    if (Session["UserName"] != null)
                    {
                        fullprofile.Id = Convert.ToInt32(Session["UserId"]);
                        if (fullprofile.Toefl == false) fullprofile.Ielts = true;
                        else if (fullprofile.Toefl == null) fullprofile.Toefl = false; fullprofile.Ielts = false;
                        fullprofile.personalScore = getscore(fullprofile);
                        connection.FullProfiles.Add(fullprofile);
                        connection.SaveChanges();
                        Session.Remove("AddProfile");
                    }
                }
                catch(System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    string msg = e.InnerException.Message;
                }
            }
            return RedirectToAction("Index", "Home");
        }
        
        //this updates user profile
        [HttpPost]
        public ActionResult edit(FullProfile fullprofile) {
            using (MsInUsEntities connection = new MsInUsEntities())
            {
                if (Session["UserName"] != null)
                {
                    int id  = Convert.ToInt32(Session["UserId"]);
                    FullProfile fp = connection.FullProfiles.Where(m => m.Id == id).FirstOrDefault();
                    connection.FullProfiles.Remove(fp);
                    connection.SaveChanges();
                    if (fullprofile.Toefl == false) fullprofile.Ielts = true;
                    if (fullprofile.Toefl == null) { fullprofile.Toefl = false; fullprofile.Ielts = false; }
                    fullprofile.personalScore = getscore(fullprofile);
                    connection.FullProfiles.Add(fullprofile);
                    connection.SaveChanges();
                }
            }
            return RedirectToAction("MyProfile", "Profiles");
        }
        
        //this gets user profile
        [HttpGet]
        public ActionResult edit(int? id)
        {
            id = (id == null) ? Convert.ToInt32(Session["UserId"]) : id;
            FullProfile fullprofiles = new FullProfile();
            fullprofiles = genConnection.FullProfiles.Where(m => m.Id == id).FirstOrDefault();
            initilization(fullprofiles);
            return View(fullprofiles);
        }

        
        [ValidateAdd]
        public ActionResult Myprofile()
        {
            MsInUsEntities dtbase = new MsInUsEntities();
            FullProfile fullprofiles = new FullProfile();
            ///initilization(fullprofiles);
            int id = Convert.ToInt32(Session["UserId"]);
            fullprofiles = dtbase.FullProfiles.FirstOrDefault(m => m.Id == id);
           if(dtbase.AdmitsRejects.Any(m=>m.StudentId == id)) fullprofiles.admitrejects = dtbase.AdmitsRejects.Where(m => m.StudentId == id).ToList();
            return View(fullprofiles);
        }
        
        //this adds the universities that the users is admitted, interested, rejected, applied
        [ValidateAdd]
        public ActionResult addUniversities(string university,string course, string status)
        {
            if (!string.IsNullOrEmpty(university))
            {
                int id =  Convert.ToInt32(university);
                int userid = Convert.ToInt32(Session["UserId"]);
                if ((!genConnection.AdmitsRejects.Any(m => (m.UnivId == id && m.CourseName == course && m.StudentId == userid)) && (genConnection.CourseWorks.Any(m => m.Course == course))))
                {
                    AdmitsReject adr = new AdmitsReject();
                    adr.CourseName = course;
                    adr.UnivId = id;
                    adr.Status = status;
                    adr.StudentId = userid;
                    adr.StudentName = Session["UserName"].ToString();
                    adr.UnivName = genConnection.Universities_list.FirstOrDefault(m => m.UnivId ==id).UnivName;
                    if (genConnection.CourseWorks.Any(m => m.Course == course)) adr.CourseId = genConnection.CourseWorks.FirstOrDefault(m => m.Course == course).CourseId;
                    else adr.CourseId = 0;
                    genConnection.AdmitsRejects.Add(adr);
                    genConnection.SaveChanges();
                }
            }
            return RedirectToAction("Myprofile");
        }
        
        //updates the univerisity status
        [ValidateAdd]
        public ActionResult editUniversities(string university, string course, string status)
        {
            if (!string.IsNullOrEmpty(university) || !string.IsNullOrEmpty(course) || !string.IsNullOrEmpty(status))
            {
                int id = Convert.ToInt32(university);
                if (genConnection.AdmitsRejects.Any(m => (m.UnivId == id && m.CourseName == course)))
                {
                    AdmitsReject adr = genConnection.AdmitsRejects.Where(m => (m.CourseName == course && m.UnivId == id)).First();
                    adr.Status = status;
                    genConnection.SaveChanges();
                }
            }
            return RedirectToAction("Myprofile");
        }
        
        
        //removes universities from the list
        [ValidateAdd]
        public ActionResult deleteUniversities(string university, string course)
        {
            if (!string.IsNullOrEmpty(university) && !string.IsNullOrEmpty(course))
            {
                int id = Convert.ToInt32(university);
                if (genConnection.AdmitsRejects.Any(m => (m.UnivId == id && m.CourseName == course)))
                {
                   AdmitsReject adr = genConnection.AdmitsRejects.FirstOrDefault(m => (m.UnivId == id && m.CourseName == course));
                   genConnection.AdmitsRejects.Remove(adr);
                   genConnection.SaveChanges();
                }
            }
            return RedirectToAction("Myprofile");
        }
        
        //gets the score for the user
        [NonAction]
        private decimal? getscore(FullProfile fs)
        {
            decimal paper = 3;
            decimal? score  = 0, quant = 130, verbal = 130,ugscore = 4, workexp = 0;
            if (fs.Toefl == true) fs.score = (fs.TIListening + fs.TIReading + fs.TISpeaking + fs.TIWriting).ToString();
            else if (fs.Toefl == false) fs.score = ((fs.TIListening + fs.TIReading + fs.TISpeaking + fs.TIWriting) / 4).ToString();
            else fs.score = "0";
            if (fs.GQuant != null) quant = fs.GQuant; if (fs.GVerbal != null) verbal = fs.GVerbal;
            if (fs.score != "") {score = Convert.ToDecimal(fs.score); }
            if (fs.Toefl == true) score = score / 120 * 10;
            if (fs.UGScore != null) ugscore = fs.UGScore; if (fs.WorkExperience != null) ugscore = fs.UGScore; if (fs.WorkExperience != null) workexp = fs.WorkExperience; 
            if (fs.Papers == "Local") paper = 6; else if (fs.Papers == "National") paper = 8; else if (fs.Papers == "International") paper = 10;
            decimal? total = ((((((quant+verbal) / 340) * 10) + score + ugscore + ((workexp / 42) * 10) + paper) / 5));
            return total;
        }
        
        public ActionResult GetProfile(int id)
        {
            FullProfile fs = genConnection.FullProfiles.First(m => m.Id == id);
            fs.admitrejects = genConnection.AdmitsRejects.Where(m => m.StudentId == id).ToList();
            return View(fs);
        }

       [NonAction]
       public void initilization(FullProfile fullprofile)
        {
           using(MsInUsEntities dtbase = new MsInUsEntities())
            {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var c in dtbase.CourseWorks) { items.Add(new SelectListItem() { Value = c.Course, Text = c.Course }); }
            fullprofile.itemslist = items;
            List<SelectListItem> day = new List<SelectListItem>();
            for (int i = 1; i <= 31; i++)
            { day.Add(new SelectListItem() { Text = Convert.ToString(i), Value = Convert.ToString(i), Selected = (DateTime.Today.Day == i) }); }
            fullprofile.listday = day;
            List<SelectListItem> year = new List<SelectListItem>();
            for (int p = DateTime.Today.Year - 2; p < DateTime.Today.Year + 6; p++)
            { year.Add(new SelectListItem() { Text = Convert.ToString(p), Value = Convert.ToString(p), Selected = (DateTime.Today.Year == p) }); }
            fullprofile.listyear = year;
            List<SelectListItem> month = new List<SelectListItem>();
            foreach (var c in System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames)
            { month.Add(new SelectListItem() { Text = c, Value = c, Selected = (System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month) == c) }); }
            fullprofile.listMonth = month;
            List<SelectListItem> papersList = new List<SelectListItem>();
            for (int i = 0; i < papers.Length; i++) { papersList.Add(new SelectListItem() { Text = papers[i], Value = papers[i] }); }
            fullprofile.listpapers = papersList;
            List<SelectListItem> termlist = new List<SelectListItem>();
            for (int i = 0; i < term.Length; i++) { termlist.Add(new SelectListItem() { Text = term[i], Value = term[i] }); }
            fullprofile.listterm = termlist;
            }
        }   
    }
}
