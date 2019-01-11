using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.TACM.Infrastructure.Attributes;

namespace Biz.TACM.Controllers
{
    public class CaptchaController : Controller
    {
        [NoCache]
        public void RenderCaptcha(int height, int width, string captcha)
        {
            string captcha1 = this.RenderCaptchaImage(height, width, captcha);
        }
        public void RenderCaptcha2()
        {
            Response.Output.WriteLine(Session.SessionID);
        }
    }
}