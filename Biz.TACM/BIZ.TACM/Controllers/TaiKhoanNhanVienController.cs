using Biz.Lib.Authentication;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.QuanLyNhanVien.Model;
using Biz.TACM.Infrastructure.Extensions;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.TACM.Controllers
{
    public class TaiKhoanNhanVienController : Controller
    {
        private ITaiKhoanNhanVienService _taiKhoanNhanVienService;
        private ITaiKhoanNhanVienService TaiKhoanNhanVienService => _taiKhoanNhanVienService ?? (_taiKhoanNhanVienService = new TaiKhoanNhanVienService());

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        // GET: TaiKhoanNhanVien
        public ActionResult Index()
        {
            var model = TaiKhoanNhanVienService.ThongTinNhanVien(AccountUtils.CurrentUsername());
            return View(model);
        }

        public ActionResult ChiTietThongTinNhanVien()
        {
            var viewModel = TaiKhoanNhanVienService.ThongTinNhanVien(AccountUtils.CurrentUsername());
            return PartialView("_ThongTinNhanVien", viewModel);
        }

        public ActionResult CapNhatThongTinNhanVien()
        {
            var viewModel = TaiKhoanNhanVienService.ThongTinNhanVien(AccountUtils.CurrentUsername());
            ViewBag.listGioiTinh = new SelectList(XMLUtils.BindData("gender"), "value", "text");

            return PartialView("_CapNhatThongTinNhanVien", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatThongTinNhanVien(NhanVienModel viewModel)
        {
            try
            {
                ResponseResult result = TaiKhoanNhanVienService.CapNhatThongTinNhanVien(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_THONGTIN_TAIKHOAN));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult DoiMatKhau()
        {
            return PartialView("_DoiMatKhau");
        }

        public ActionResult DanhSachChucNang()
        {
            var viewModel = TaiKhoanNhanVienService.DanhSachChucNangTheoMaNhanVien(AccountUtils.CurrentUsername());
            ViewBag.DanhSachTatCaChucNang = SettingDataService.SettingDataItem("DS_SOTHAM_CHUCNANG", "");
            return PartialView("_DanhSachChucNang", viewModel);
        }

        public ActionResult CapNhatHinhDaiDien()
        {
            return PartialView("_CapNhatHinhDaiDien");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatHinhDaiDien(string maNV, HttpPostedFileBase file)
        {
            try
            {
                ResponseResult result = TaiKhoanNhanVienService.CapNhatHinhDaiDien(AccountUtils.CurrentUsername(), file);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGCAPNHAT_HINHDAIDIEN));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
    }
}