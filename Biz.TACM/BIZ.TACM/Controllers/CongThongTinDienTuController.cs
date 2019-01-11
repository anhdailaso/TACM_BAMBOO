using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.Infrastructure.Attributes;
using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.CongThongTinDienTu;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biz.TACM.Controllers
{
    public class CongThongTinDienTuController : Controller
    {
        private ITraCuuThongTinService _traCuuThongTinService;
        private ITraCuuThongTinService TraCuuThongTinService => _traCuuThongTinService ?? (_traCuuThongTinService = new TraCuuThongTinService());

        public ActionResult Index()
        {
            return View();
        }

        [CaptchaValidation("captcha")]
        [HttpPost]
        public ActionResult GetGetHoSoVuAnDuocTraCuu(bool captchaValid, string keyword)
        {
            if (!captchaValid)
            {
                return Json(JsonResponseViewModel.CreateFail(AccountMessage.CAPTCHA_INVALID));
            }
            else if (TraCuuThongTinService.GetGetHoSoVuAnDuocTraCuu(keyword) == null)
            {
                return Json(JsonResponseViewModel.CreateFail("Không tìm thấy hồ sơ với từ khóa đã nhập."));
            }
            else
            {
                return Json(JsonResponseViewModel.CreateSuccess());
            }
        }

        public ActionResult KetQuaTraCuu(string keyword)
        {
            var viewModel = TraCuuThongTinService.GetGetHoSoVuAnDuocTraCuu(keyword);
            return PartialView("_KetQua", viewModel);
        }
    }
}