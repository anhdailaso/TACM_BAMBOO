using System;
using System.Linq;
using System.Web.Mvc;
using Biz.TACM.Models.ViewModel.HuyHoSo;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using Biz.Lib.TACM.Resources.Resources;
using Biz.Lib.Authentication;
using Biz.Lib.Helpers;


namespace Biz.TACM.Controllers
{
    public class HuyHoSoController : BizController
    {
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        private readonly IHuyHoSoService HuyHoSoService;
        public HuyHoSoController(IHuyHoSoService _HuyHoSoService)
        {
            HuyHoSoService = _HuyHoSoService;
        }
        // GET: HuyHoSo

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DanhSachHuyHoSo()
        {
            var viewModel = HuyHoSoService.DanhSachHoSo();
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult ChiTietHuyHoSo(int hoSoVuAnID)
        {
            var viewModel = HuyHoSoService.ChiTietHuy(hoSoVuAnID);
            return PartialView("~/Views/HuyHoSo/_XemChiTietHuyHoSo.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult SuaHuyHoSo(int hoSoVuAnID)
        {
            var viewModel = HuyHoSoService.ChiTietHuy(hoSoVuAnID);                
            return PartialView("~/Views/HuyHoSo/_SuaHuyHoSo.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateInput (false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaHuyHoSo(int hoSoVuAnID, string lyDoHuy)
        {
            try
            {
                var result = HuyHoSoService.SuaHuyHoSo(hoSoVuAnID, lyDoHuy);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess(NotifyMessage.SUA_HUYHOSO_THANHCONG));
                }
                else if (result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(result.ResponseMessage));
                }
                else
                    return Json(JsonResponseViewModel.CreateFail(NotifyMessage.SUA_HUYHOSO_THATBAI));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult TimHoSo(string MaHoSo)
        {
            var model = HuyHoSoService.TimHoSo(MaHoSo);
            return PartialView("~/Views/HuyHoSo/_TimHoSo.cshtml", model);
        }

        public ActionResult XacNhanHuy(string maHoSo)
        {
           return PartialView("~/Views/HuyHoSo/_XacNhanHuy.cshtml", maHoSo);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHuyHoSo(int HoSoVuAnID, string LyDoHuy)
        {
            try
            {
                var result = HuyHoSoService.HuyHoSo(HoSoVuAnID, LyDoHuy, SettingDataService.GetNhanVienSessionInfo().MaNV);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess(NotifyMessage.HUYHOSO_THANHCONG));
                }
                else if (result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(result.ResponseMessage));
                }
                else
                    return Json(JsonResponseViewModel.CreateFail(NotifyMessage.HUYHOSO_THATBAI));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
    }
}