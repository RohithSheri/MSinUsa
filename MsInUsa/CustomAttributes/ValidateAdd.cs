using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MsInUsa.Model
{
       public class ValidateAdd : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                using(MsInUsEntities connection = new MsInUsEntities()){
                int id = Convert.ToInt32(filterContext.HttpContext.Session["UserId"]);
                if(!connection.FullProfiles.Any(m => m.Id == id)){
                    filterContext.HttpContext.Session.Add("AddProfile", value: false);
                    filterContext.HttpContext.Response.Redirect("~/Profiles/add");}}
            }
        }
}