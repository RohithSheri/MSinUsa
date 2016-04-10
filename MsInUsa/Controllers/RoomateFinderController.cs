using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MsInUsa.Model;

namespace MsInUsa.Controllers
{
    [UserAuthorize]
    public class RoomateFinderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
