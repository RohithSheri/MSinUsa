using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MsInUsa.Model;
using PagedList;
using PagedList.Mvc;

namespace MsInUsa.Controllers
{
    [UserAuthorize][ValidateAdd]
    public class ProfileMatcherController : Controller
    {
        MsInUsEntities connection = new MsInUsEntities();
        public ActionResult SimilarProfiles(int? page)
        {
            int id = Convert.ToInt32(Session["UserId"]);
            FullProfile fs = connection.FullProfiles.First(m => m.Id == id);
            decimal? actualscore = fs.personalScore + Convert.ToDecimal(0.3);
            decimal? negativescore = fs.personalScore - Convert.ToDecimal(0.3);
            List<FullProfile> obj = connection.FullProfiles.Where(m =>(m.personalScore <= actualscore && m.personalScore >= negativescore  && m.Id != id)).ToList();
            return View(obj.ToPagedList(page??1,8));
        }
        
        public ActionResult AdmitsRejects(int ? page)
        {
            int id = Convert.ToInt32(Session["UserId"]);
            List<FullProfile> fs = new List<FullProfile>();
            List<AdmitsReject> lobj = connection.AdmitsRejects.Where(m=>m.StudentId == id).ToList();
            List<AdmitsReject> obj = new List<AdmitsReject>();
            foreach(var p in lobj){
                obj.AddRange(connection.AdmitsRejects.Where(m => (m.UnivId == p.UnivId && m.CourseId == p.CourseId && m.StudentId != p.StudentId)).ToList());
            }
            foreach (var c in obj)
            {
                FullProfile fobj = new FullProfile();
                  fobj = connection.FullProfiles.AsNoTracking().SingleOrDefault(m =>(m.Id == c.StudentId && m.Id != id));
                  if (fobj != null)
                  {
                      fobj.admitrejects = new List<AdmitsReject>();
                      fobj.admitrejects.Add(c);
                      fs.Add(fobj);
                  }
            }
           return View(fs.ToPagedList(page??1,8));
        }

        public ActionResult getstatus(int ? page, int university, string course, string status)
        {
            int id = Convert.ToInt32(Session["UserId"]);
            List<FullProfile> fs = new List<FullProfile>();
            List<AdmitsReject> lobj = connection.AdmitsRejects.Where(m =>(m.UnivId == university && m.CourseName == course && m.Status == status)).ToList();
            foreach(AdmitsReject c in lobj){
                FullProfile fsobj = connection.FullProfiles.FirstOrDefault(m => (m.Id == c.StudentId && m.Id != id));
                if (fsobj != null)
                {
                    fsobj.admitrejects = new List<AdmitsReject>();
                    fsobj.admitrejects.Add(c);
                    fs.Add(fsobj);
                }
            }
            return View("AdmitsRejects", fs.ToPagedList(page ?? 1, 8));
        }
    }
}
