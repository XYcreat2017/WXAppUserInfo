using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXAppUserInfo.Filters;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Helpers;

namespace WXAppUserInfo.Controllers
{
    [OAuthFikter]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //var config=JSSDKHelper.GetJsSdkUiPackage
            var userinfo = Session["userinfo"] as OAuthUserInfo;
            return View(userinfo);
        }
        public ActionResult Share()
        {
            return View();
           
        }
    }
}