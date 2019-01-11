using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.Models.ViewModel.SoDoToChuc;
using Biz.Lib.TACM.SoDoToChuc.Models;
using Biz.TACM.Services;
using System;
using Biz.Lib.Authentication;
using Biz.Lib.TACM.Resources.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Biz.TACM.Controllers
{
    public class SoDoToChucController : BizController
    {
        private ISoDoToChucService _soDoToChucService;
        private ISoDoToChucService SoDoToChucService => _soDoToChucService ?? (_soDoToChucService = new SoDoToChucService());

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        // GET: SoDoToChuc
        public ActionResult Index()
        {
            var toaAnID = GetAnSessionInfo().ToaAnId;

            //var danhSachChucDanh = SoDoToChucService.DanhSachChucDanhTheoToaAn(toaAnId); //ToaAnID của user đang đăng nhập
            ViewBag.listToaAnSoDoToChuc = SettingDataService.SelectListDanhSachToaAnValueIsToaAnIDTheoNV(toaAnID, AccountUtils.CurrentUsername()); //ToaAnID của user đang đăng nhập
            ViewBag.listChucDanh = SoDoToChucService.DanhSachChucDanhCha(null, toaAnID); //ToaAnID của user đang đăng nhập
            ViewBag.listChucVu = SoDoToChucService.DanhSachChucVuCha(null, toaAnID); //ToaAnID của user đang đăng nhập
            ViewBag.toaAnID = toaAnID;
            return View();
        }

        public ActionResult ChiTietChucDanh(int id) //chucDanhId
        {
            var chucDanh = SoDoToChucService.ChiTietChucDanhTheoId(id);
            if(chucDanh.Loai==2)
                return PartialView("_ThongTinChucVu", chucDanh);
            return PartialView("_ThongTinChucDanh", chucDanh);
        }
        public ActionResult DanhSachChucDanhTheoToaAn(int? toaAnId)
        {
            var danhSachChucDanh = SoDoToChucService.DanhSachChucDanhTheoToaAn(toaAnId);
            ViewBag.ToaAnID = toaAnId;
            return PartialView("_SoDoToChucTreeView", danhSachChucDanh);
        }
        public ActionResult DanhSachChucVuTheoToaAn(int? toaAnId)
        {
            var danhSachChucDanh = SoDoToChucService.DanhSachChucVuTheoToaAn(toaAnId);
            ViewBag.ToaAnID = toaAnId;
            return PartialView("_SoDoToChucTreeViewChucVu", danhSachChucDanh);
        }
        public ActionResult ThemChucDanh(int? id, int toaAnID)
        {
            ChucDanhViewModel chucDanh = new ChucDanhViewModel();
            chucDanh.ToaAnID = toaAnID;
            if (id == null)
            {
                ViewBag.listChucDanh = SoDoToChucService.DanhSachChucDanhCha(null, toaAnID);
                return PartialView("_ThemChucDanh", chucDanh);
            }

            ViewBag.listChucDanh = SoDoToChucService.DanhSachChucDanhCha(/*chucDanh.SoDoToChucID.ToString()*/id.ToString(), toaAnID);
            var listChucDanhCha = SoDoToChucService.DanhSachChucDanhTheoToaAn(toaAnID).ToList();
            ChucDanhViewModel root = new ChucDanhViewModel()
            {
                ChucDanh = ViewText.LABEL_KHONGCO,
                SoDoToChucID = 0,
                ChucDanhChaID = 0,
                ToaAnID = toaAnID
            };
            listChucDanhCha.Insert(0, root);

            ViewBag.listChucDanh = listChucDanhCha;
            return PartialView("_ThemChucDanh", chucDanh);
        }
        public ActionResult ThemChucVu(int? id, int toaAnID)
        {
            ChucDanhViewModel chucDanh = new ChucDanhViewModel();
            chucDanh.ToaAnID = toaAnID;
            if (id == null)
            {
                ViewBag.listChucDanh = SoDoToChucService.DanhSachChucVuCha(null, toaAnID);
                return PartialView("_ThemChucVu", chucDanh);
            }

            ViewBag.listChucDanh = SoDoToChucService.DanhSachChucVuCha(/*chucDanh.SoDoToChucID.ToString()*/id.ToString(), toaAnID);
            var listChucDanhCha = SoDoToChucService.DanhSachChucVuTheoToaAn(toaAnID).ToList();
            ChucDanhViewModel root = new ChucDanhViewModel()
            {
                ChucDanh = ViewText.LABEL_KHONGCO,
                SoDoToChucID = 0,
                ChucDanhChaID = 0,
                ToaAnID = toaAnID
            };
            listChucDanhCha.Insert(0, root);

            ViewBag.listChucDanh = listChucDanhCha;
            return PartialView("_ThemChucVu", chucDanh);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ThemChucDanh(ChucDanhViewModel viewModel)
        {
            try
            {
                ResponseResult result = SoDoToChucService.ThemChucDanh(viewModel);
                if(result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                else if(result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_TENCHUCDANH_KHONGTONTAI));
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_THEMCHUCDANH_KHONGTHANHCONG));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult SuaChucDanh(int id, int toaAnID)
        {
            var chucDanh = SoDoToChucService.ChiTietChucDanhTheoId(id);
            //ViewBag.listChucDanh = SoDoToChucService.DanhSachChucDanhCha(chucDanh.ChucDanhChaID.ToString(), toaAnID);
            var listChucDanhCha = SoDoToChucService.DanhSachChucDanhTheoToaAn(toaAnID).ToList();
            ChucDanhViewModel root = new ChucDanhViewModel()
            {
                ChucDanh = ViewText.LABEL_KHONGCO,
                SoDoToChucID = 0,
                ChucDanhChaID = 0,
                ToaAnID = toaAnID
            };
            listChucDanhCha.Insert(0, root);

            ViewBag.listChucDanh = listChucDanhCha;
            return PartialView("_SuaChucDanh", chucDanh);
        }
        public ActionResult SuaChucVu(int id, int toaAnID)
        {
            var chucDanh = SoDoToChucService.ChiTietChucDanhTheoId(id);
            //ViewBag.listChucDanh = SoDoToChucService.DanhSachChucDanhCha(chucDanh.ChucDanhChaID.ToString(), toaAnID);
            var listChucDanhCha = SoDoToChucService.DanhSachChucVuTheoToaAn(toaAnID).ToList();
            ChucDanhViewModel root = new ChucDanhViewModel()
            {
                ChucDanh = ViewText.LABEL_KHONGCO,
                SoDoToChucID = 0,
                ChucDanhChaID = 0,
                ToaAnID = toaAnID
            };
            listChucDanhCha.Insert(0, root);

            ViewBag.listChucDanh = listChucDanhCha;
            return PartialView("_SuaChucVu", chucDanh);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SuaChucDanh(ChucDanhViewModel viewModel)
        {
            try
            {
                ResponseResult result = SoDoToChucService.SuaChucDanh(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                else if(result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_TENCHUCDANH_KHONGTONTAI));
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_CAPNHATCHUCDANH_KHONGTHANHCONG));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        public ActionResult XoaChucDanh(int id)
        {
            ViewBag.Test = id;
            var ct = SoDoToChucService.ChiTietChucDanhTheoId(id);
            if(ct.Loai==2)
                return PartialView("~/Views/SoDoToChuc/_XoaChucVu.cshtml", id);
            return PartialView("~/Views/SoDoToChuc/_XoaChucDanh.cshtml",id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaChucDanhConfirmed(int soDoToChucId)
        {
            try
            {
                ResponseResult result = SoDoToChucService.XoaChucDanh(soDoToChucId);
                if(result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGXOADUOC_CHUCDANH));
            }
            catch(Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
    }
}