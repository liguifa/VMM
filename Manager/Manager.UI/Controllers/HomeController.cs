using Common.Logger;
using Common.Message;
using Manager.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.UI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Login(string username, string password, string userType)
        {
            Logger.Instance(typeof(HomeController)).Info("IP:{0}登录系统.", HttpContext.Request.UserHostAddress);
            //执行登录操作
            Message result = new User().Login(username, password,userType);
            if (result.Status)
            {
                Session["user"] = result.Append;
            }
            return Json(result);
        }
    }
}
