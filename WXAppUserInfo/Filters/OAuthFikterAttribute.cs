using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WXAppUserInfo.Filters
{
    public class OAuthFikterAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //判断AccessToken为授权表示，如果不为空就以登陆
            if (filterContext.HttpContext.Session["AccessToken"] != null) return;
            //登陆授权阶段

            //获取用户请求的url
            var requestual = filterContext.HttpContext.Request.RawUrl;
            //跳转到OAuth登陆页面
            var urlHelp = new UrlHelper(filterContext.RequestContext);
            //重定向
            filterContext.Result = new RedirectResult(urlHelp.Action("Login", "OAuth",new { requestual}));
        }
    }
}