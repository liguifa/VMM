using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.BLL;
using Common.Function;

namespace Manager.UI.Controllers
{
    public class SystemController : Controller
    {
        //
        // GET: /System/

        [UserAuthorization("user")]
        public ActionResult Index()
        {
            ViewBag.vmSystem = new VMSystem().GetVMSystemInfo(Guid.Parse(Session["user"].ToString()));
            return View();
        }

        public JsonResult Active(string names)
        {
            
            return Json(new VMSystem().ActiveSystem(names));
        }
    }
}
