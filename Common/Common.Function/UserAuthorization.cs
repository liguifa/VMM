/*
 * 作者：李贵发
 * 时间：2015-03-13
 * 功能：自定义用户登录权限验证
 * 文件：UserAuthorization.cs
*/
namespace Common.Function
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// 权限验证类
    /// </summary>
    public class UserAuthorization : AuthorizeAttribute
    {
        /// <summary>
        /// 初始化权限验证类
        /// </summary>
        /// <param name="permissionName">权限名称</param>
        public UserAuthorization(string permissionName = "")
        {
            this.PermissionName = permissionName;
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName { get; private set; }

        /// <summary>
        /// 验证用户是否授权
        /// </summary>
        /// <param name="httpContext">http上下文，封装http信息</param>
        /// <returns>true用户授权，false用户未授权</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                return false;
            }
            else if (!httpContext.User.Identity.IsAuthenticated)
            {
                return httpContext.Session[this.PermissionName] != null;     //验证用户是否登陆
            }
            return false;
        }

        /// <summary>
        /// 重写OnAuthorization方法，完成未授权用重定向登陆页
        /// </summary>
        /// <param name="filterContext">对使用AuthorizeAttribute特性时所需的信息进行封装</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.Result != null && ((HttpStatusCodeResult)(filterContext.Result)).StatusCode == 401)      //状态码为401表明用户没有登陆，则重定向至登陆页面
            {
                filterContext.Result = new RedirectResult("/Home/Index.html");
            }
        }
    }
}
