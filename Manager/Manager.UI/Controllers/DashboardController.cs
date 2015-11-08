﻿using Common.Logger;
using Manager.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.UI.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            Logger.Instance(typeof(DashboardController)).Info("用户：IP{0}打开Dashboard页.", HttpContext.Request.UserHostAddress);
            ViewBag.user = new User().GetUser(Guid.Parse(Session["user"].ToString()));
            return View();
        }

    }
}
