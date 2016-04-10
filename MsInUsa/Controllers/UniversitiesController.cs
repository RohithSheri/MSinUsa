using MsInUsa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;


namespace MsInUsa.Controllers
{
    public class UniversitiesController : Controller
    {
        public ActionResult Index(int? page, string sortby)
        {
            using(MsInUsEntities connection = new MsInUsEntities()){
                ViewBag.sortUniv = String.IsNullOrEmpty(sortby) ? "Univdesc" : "";
                ViewBag.sortRank = sortby == "Rank" ? "Rankdesc" : "Rank";
                ViewBag.sortType = sortby == "Type" ? "Typedesc" : "Type";
                List<Universities_list> universities_list = connection.Universities_list.OrderBy(m => m.UnivName).ToList();
                ViewBag.count = universities_list.Count;
                switch(sortby){
                    case "Univdesc" :
                        universities_list = universities_list.OrderByDescending(m => m.UnivName).ToList();
                        break;
                    case "Rank":
                        universities_list = universities_list.OrderBy(m => m.Rank).ToList();
                        break;
                    case "Rankdesc" :
                        universities_list = universities_list.OrderByDescending(m => m.Rank).ToList();
                        break;
                    case "Type" :
                        universities_list = universities_list.OrderBy(m => m.UnivType).ToList();
                        break;
                    case "Typedesc" :
                        universities_list = universities_list.OrderByDescending(m => m.UnivType).ToList();
                        break;
                    default :
                        break;
                }
                return View(universities_list.ToPagedList(page??1,10)); 
            }
        }
        
        public ActionResult reviews(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Universities");
            using(MsInUsEntities connection = new MsInUsEntities())
           {
               Universities_list universities_list = connection.Universities_list.SingleOrDefault(m => m.UnivId == id);
               universities_list.universitycourses = connection.University_Deadlines.Where(m => m.Univ_Id == id).Select(m => m.Course).Distinct().ToList();
               return View(universities_list);
           }
        }

        public JsonResult searchUniversities(string term)
        {
            MsInUsEntities connection = new MsInUsEntities();
            //List<string> jsresult = connection.Universities_list.Where(m => (m.UnivName.ToLower().StartsWith(term.ToLower()) || m.NickName.ToLower().Contains(term.ToLower()))).Select(m => m.UnivName).ToList();
            Dictionary<string, string> jsresult = connection.Universities_list.Where(m => m.UnivName.ToLower().Contains(term.ToLower())).ToDictionary(m => Convert.ToString(m.UnivId),m=>m.UnivName);
               return Json(jsresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult searchCourses(string term, int? univid)
        {
            MsInUsEntities connection = new MsInUsEntities();
             List<string> jsresult = connection.University_Deadlines.Where(m =>(m.Univ_Id == univid && m.Course.ToLower().Contains(term.ToLower()))).Select(m=>m.Course).Distinct().ToList();
            return Json(jsresult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult compare(string universities, int? page)
        {
            if(universities!=""){
                ViewBag.sortparamUniv = universities;
            using(MsInUsEntities connection = new MsInUsEntities()){
                IPagedList<Universities_list> universitylist = connection.Universities_list.Where(m=> universities.Contains(m.UnivId.ToString())).ToList().ToPagedList(page ?? 1, 5);
                return View(universitylist);
            }
          }
          return RedirectToAction("CompareUniversities","Universities");
        }

        public ActionResult CompareUniversities()
        {
            return View();
        }

        public ActionResult UniversitiesDeadlines(int? page, string sortby)
        {
            MsInUsEntities ms = new MsInUsEntities();
            ViewBag.sortUniv = String.IsNullOrEmpty(sortby) ? "Univdesc" : "";
            ViewBag.sortCourse = sortby == "Course" ? "Coursedesc" : "Course";
            ViewBag.sortTerm = sortby == "Term" ? "Termdesc" : "Term";
            ViewBag.sortDeadline = sortby == "Deadline" ? "Deadlinedesc" : "Deadline";
            IEnumerable<University_Deadlines> univ = ms.University_Deadlines.Where(m=>m.Deadline!= null).ToList();
            switch(sortby)
            { case "Univdesc" :
                    univ = univ.OrderByDescending(m => m.Universities_list.UnivName);
                    break;
              case "Coursedesc":
                     univ = univ.OrderByDescending(m => m.Course);
                    break;
              case "Course":
                    univ = univ.OrderBy(m => m.Course);
                    break;
              case "Termdesc":
                    univ = univ.OrderByDescending(m => m.Term);
                    break;
              case "Term":
                    univ = univ.OrderBy(m => m.Term);
                    break;
              case "Deadline":
                    univ = univ.OrderBy(m => m.Deadline);
                    break;
              case "Deadlinedesc":
                    univ = univ.OrderByDescending(m => m.Deadline);
                    break;
             default :
                    univ = univ.OrderBy(m => m.Universities_list.UnivName);
                    break;
            }
            return View(univ.ToPagedList(page??1,10));
            
        }
    }
}
