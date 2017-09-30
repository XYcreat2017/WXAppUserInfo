using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin;
using Senparc.Weixin.MP.Helpers;

namespace WXAppUserInfo.Controllers
{

    public class OAuthController : Controller
    {
        //声明AppID
        private string _AppID = "wxc3996d2b2f2c8827";
        //声明appsecret
        private string _Appsecret = "fda873bf2c48137699089f24121f0955";
        //声明域名
        private string _domine = "http://wx.lylweb.top";
        /// <summary>
        /// OAuth登陆方法
        /// </summary>
        /// <returns></returns>
        // GET: OAuth
        public ActionResult Login(string requestual)
        {
            //生成微信授权后的回掉地址requestual
            var redirectURI = _domine + Url.Action("CallBack",new { requestual });
            //生成验证码
            var state = "wx" + DateTime.Now.Millisecond;
            //存到session
            Session["state"] = state;
            //生成静默地址
          var url=  OAuthApi.GetAuthorizeUrl(_AppID, redirectURI, state, OAuthScope.snsapi_base);
            return Redirect(url);
        }
        public ActionResult CallBack(string code,string state,string requestual)
        {
            //比较验证码
            if (state != Session["state"] as string)
            {
                return Content("非法访问");
            }
            //验证码只能用一次
            Session.Remove("state");
            //判断code是否有效
            if (string.IsNullOrEmpty(code))
            {
                return Content("无法授权");
            }
            //用code换取令牌
            var result = OAuthApi.GetAccessToken(_AppID, _Appsecret, code);
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("授权失败");
            }
            //以获取令牌登陆成功
            //尝试获取用互信息,如果获取不到则用户没有关注，令牌权限不够
            //如果获取则令牌可以
            try
            {
                var userinfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                Session["userinfo"] = userinfo;
                //授权用户全部完成,重定向到用户刚开始打开的用户
                Session["AccessToken"] = result;
                return Redirect(requestual);
            }
            catch (Exception )
            {
                //生成微信授权后的回掉地址requestual
                var redirectURI = _domine + Url.Action("CallBack", new { requestual });
                //生成验证码
                 state = "wx" + DateTime.Now.Millisecond;
                //存到session
                Session["state"] = state;
                //生成用户授权地址
                var url = OAuthApi.GetAuthorizeUrl(_AppID, redirectURI, state, OAuthScope.snsapi_userinfo);
                return Redirect(url);
            }
          
            //return Content($"{code}<br/>{state}<br/>{requestual}");
        }
        public ActionResult JsSdkHelp()
        {
            //获取当前完整url，带上前缀域名
            var ual = _domine + Request.RawUrl;
            //获取JSHelp
            var config = JSSDKHelper.GetJsSdkUiPackage(_AppID, _Appsecret, ual);
            return PartialView(config);
        }
    }
}