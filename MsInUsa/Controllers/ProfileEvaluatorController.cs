using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MsInUsa.Model;
    
namespace MsInUsa.Controllers
{
    /*gets the profile of the user by checking whether there exists a profile using the validateAdd attribute and whether
    user is logged in. */
    [UserAuthorize][ValidateAdd]
    public class ProfileEvaluatorController : Controller
    {
        MsInUsEntities connection = new MsInUsEntities();
        public ActionResult Index()
        {   
            int id = Convert.ToInt32(Session["UserId"]);
            FullProfile fs = connection.FullProfiles.SingleOrDefault(m => m.Id == id);
            if (fs.Toefl == true) fs.score = (fs.TIListening + fs.TIReading + fs.TISpeaking + fs.TIWriting).ToString();
            else if (fs.Ielts == true) fs.score = ((fs.TIListening + fs.TIReading + fs.TISpeaking + fs.TIWriting) / 4).ToString();
            if (fs.Toefl == null && fs.Ielts == null) fs.Toefl = true;
            MsInUsa.Controllers.ProfilesController cobj = new ProfilesController();
            cobj.initilization(fs);
            return View(fs);
        }
        
        //Post the profile to get the list of universities based on the generated user score.
        [HttpPost]
        public ActionResult Index(FullProfile fs)
        {
            decimal? score = getscore(fs);
            if (fs.Toefl == true) fs.score = (fs.TIListening + fs.TIReading + fs.TISpeaking + fs.TIWriting).ToString();
            else if (fs.Toefl == false) fs.score = ((fs.TIListening + fs.TIReading + fs.TISpeaking + fs.TIWriting) / 4).ToString();
            if (fs.Toefl == null && fs.Ielts == null) fs.Toefl = true;
            fs.ambUniv = connection.Universities_list.Where(m =>(m.OverallScore >= score + 1 && m.University_Deadlines.Any(c=>c.Course == fs.MSCourse))).OrderBy(m=>m.OverallScore).Take(5).ToList();
            fs.modUniv = connection.Universities_list.Where(m => (m.OverallScore > score && m.OverallScore < score + 1 && m.University_Deadlines.Any(c => c.Course == fs.MSCourse))).OrderBy(m => m.OverallScore).Take(5).ToList();
            fs.safeUniv = connection.Universities_list.Where(m => ((m.OverallScore <= score || m.OverallScore==null)&& m.University_Deadlines.Any(c=>c.Course == fs.MSCourse))).OrderByDescending(m => m.OverallScore).Take(5).ToList();
            MsInUsa.Controllers.ProfilesController cobj = new ProfilesController();
            cobj.initilization(fs);
            return View("recommend",fs);
        }
        
        //genertates score for the user
        private decimal? getscore(FullProfile fs)
        {
            decimal paper = 3;
            decimal? score = Convert.ToDecimal(fs.score);
            if (fs.Toefl == true) score = score / 120 * 10;
            if(fs.Papers == "Local") paper = 6; else if(fs.Papers =="National") paper = 8; else if(fs.Papers =="International") paper = 10;
            decimal? total =  ((((((fs.GQuant + fs.GVerbal) / 340) * 10)+ score + fs.UGScore + ((fs.WorkExperience / 42) * 10) + paper)/5*6) + ((fs.Sop + fs.Lor)/2*3) + ((fs.ExtraCurricular + fs.CommunityService)/2));
            return total/10;
        }
    }
}
