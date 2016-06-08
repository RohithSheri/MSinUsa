using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MsInUsa.Model;

namespace MsInUsa.Controllers
{
    //Home controller for the home page of the web
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();//view for the index page of the home controller
        }
        
    }

}
