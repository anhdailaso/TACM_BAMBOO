using System;
using System.Linq;
using System.Web.Mvc;
using Biz.TACM.Models.ViewModel.GiamDocThamTaiTham;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using Biz.Lib.Authentication;
using Biz.Lib.Helpers;


namespace Biz.TACM.Controllers
{
    public class GiamDocThamTaiThamController : BizController
    {
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        private readonly IGiamDocThamTaiThamService GiamDocThamTaiThamService;
        public GiamDocThamTaiThamController(IGiamDocThamTaiThamService _giamDocThamTaiThamService)
        {
            GiamDocThamTaiThamService = _giamDocThamTaiThamService;
        }
        // GET: GiamDocThamTaiTham

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DanhSachGDTTT()
        {
            var viewModel = GiamDocThamTaiThamService.DanhSachGDTTT().ToList();
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult ChiTietGDTTT(int gDTTTid)
        {
            var viewModel = GiamDocThamTaiThamService.ChiTietGDTTT(gDTTTid);
            if(viewModel.Nhom=="Giám đốc thẩm")
                ViewBag.noidung = new SelectList(SettingDataService.ListSettingDataItem("GDT_QUYETDINH"), "ItemText", "ItemText");
            else
                ViewBag.noidung = new SelectList(SettingDataService.ListSettingDataItem("TT_QUYETDINH"), "ItemText", "ItemText");
            var nhoman = SettingDataService.ListSettingDataItem("DINHNGHIACHUNG_NHOMANTOANAN");
            viewModel.NhomAn = nhoman.Where(x => x.ItemRemark == viewModel.NhomAn).FirstOrDefault().ItemText;
            return PartialView("~/Views/GiamDocThamTaiTham/_XemChiTietGDTTT.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult SuaGDTTT(int gDTTTid)
        {
            var viewModel = GiamDocThamTaiThamService.ChiTietGDTTT(gDTTTid);
            ViewBag.GiamDocTham = new SelectList(SettingDataService.ListSettingDataItem("GDT_QUYETDINH"), "ItemText", "ItemText"); 
            ViewBag.TaiTham = new SelectList(SettingDataService.ListSettingDataItem("TT_QUYETDINH"), "ItemText", "ItemText");
            ViewBag.Nhomddl = new SelectList(XMLUtils.BindData("GiamDocThamTaiTham"), "value", "text");
            ViewBag.NhomAnddl = new SelectList(SettingDataService.ListSettingDataItem("DINHNGHIACHUNG_NHOMANTOANAN"), "ItemRemark", "ItemText");
            ViewBag.MaHoSoddl = new SelectList(GiamDocThamTaiThamService.DanhSachHoSo(viewModel == null ? "" : viewModel.NhomAn),"MaHoSo", "MaHoSo");
            if (viewModel == null)
            {
                viewModel = new GiamDocThamTaiThamViewModel();
                viewModel.GiamDocThamTaiThamID = 0;
            }  
            if(viewModel.Hoso==null)
            {
                var hs = new HoSoVuAnViewModel();
                viewModel.Hoso = hs;
            }
            if(viewModel.GiamDocThamTaiThamID!=0)
                return PartialView("~/Views/GiamDocThamTaiTham/_SuaGDTTT_Sua.cshtml", viewModel);
            return PartialView("~/Views/GiamDocThamTaiTham/_SuaGDTTT.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateInput (false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaGDTTT(GiamDocThamTaiThamViewModel model)
        {
            try
            {
                string fail = " hồ sơ giám đốc thẩm, tái thẩm không thành công!", sucsess = " hồ sơ giám đốc thẩm, tái thẩm thành công!";
                if(model.GiamDocThamTaiThamID==0)
                {
                    fail = "Tạo" + fail;
                    sucsess = "Tạo" + sucsess;
                }
                else
                {
                    fail = "Sửa" + fail;
                    sucsess = "Sửa" + sucsess;
                }
                model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                var result = GiamDocThamTaiThamService.SuaGDTTT(model);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess(sucsess));
                }
                else if (result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(result.ResponseMessage));
                }
                else
                    return Json(JsonResponseViewModel.CreateFail(fail));
                
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult TimHoSo(string MaHoSo)
        {
            var model = GiamDocThamTaiThamService.TimHoSo(MaHoSo);
            var viewModel = new GiamDocThamTaiThamViewModel();
            viewModel.Hoso = model;
            return PartialView("~/Views/GiamDocThamTaiTham/_HoSo.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult XoaGDTTT(int gDTTTid)
        {
            return PartialView("~/Views/GiamDocThamTaiTham/_XoaGDTTT.cshtml", gDTTTid);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult XoaGDTTTConfirm(int gDTTTid)
        {
            try
            {
                var result = GiamDocThamTaiThamService.XoaGDTTT(gDTTTid, SettingDataService.GetNhanVienSessionInfo().MaNV);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess("Xoá hồ sơ giám đốc thẩm, tái thẩm không thành công!"));
                }
                return Json(JsonResponseViewModel.CreateFail("Xoá hồ sơ giám đốc thẩm, tái thẩm không thành công!"));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        public ActionResult DanhSachMaHoSo(string nhomAn)
        {
            var model = GiamDocThamTaiThamService.DanhSachHoSo(nhomAn);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}