using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineGraduationManagement.Filter
{
    public class SystemAuthen : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RouteValueDictionary Rd = new RouteValueDictionary { { "controller", "login" }, { "action", "Index" } }; // ถ้าไม่เป็นไปตามเงื่อนไขก็ให้เข้าไปตามนี้
            if (filterContext.HttpContext.Session["usr"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(Rd); // ให้ตามไปยัง Rd ที่ได้สร้างไว้
            }
            else if (filterContext.HttpContext.Session["usr"].ToString() == "") // เขียนเผื่อเป็นค่าว่าง ไม่ใช่ค่า null
            {
                filterContext.Result = new RedirectToRouteResult(Rd);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}