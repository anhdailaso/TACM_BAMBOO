using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biz.TACM.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CaptchaValidationAttribute : ActionFilterAttribute
    {
        public CaptchaValidationAttribute()
            : this("captcha") { }

        public CaptchaValidationAttribute(string field)
        {
            Field = field;
        }

        public string Field { get; private set; }

        /// Called when [action executed].
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool captchaValid = true;
            string actualValue = filterContext.HttpContext.Request.Form[Field].Trim();
            string expectedValue = CryptoService.DecryptString(HttpUtility.UrlDecode(filterContext.HttpContext.Request.Form["captcha_text"].ToString()));

            // validate the captch

            if (!String.IsNullOrEmpty(expectedValue) && !String.Equals(actualValue, expectedValue))
            {
                captchaValid = false;
            }

            filterContext.ActionParameters["captchaValid"] = captchaValid;
        }
    }
}