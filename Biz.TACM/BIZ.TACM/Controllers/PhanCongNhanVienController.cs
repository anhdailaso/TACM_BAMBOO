using System;
using Biz.Lib.TACM.PhanCongNhanVien.Model;
using Biz.TACM.Infrastructure.Attributes;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Helpers;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.IO;
using Biz.Lib.TACM.Resources.Resources;
using Biz.Lib.Authentication;
using Biz.Lib.SettingData.Model;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.Models.ViewModel.PhanCongNhanVien;
using System.ComponentModel.Design;
namespace Biz.TACM.Controllers
{
    public class PhanCongNhanVienController : BizController
    {
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        private readonly IPhanCongNhanVienService _PhanCongNhanVienService;
        private IQuanLyNhanVienService _quanLyNhanVienService;
        private IQuanLyNhanVienService QuanLyNhanVienService => _quanLyNhanVienService ?? (_quanLyNhanVienService = new QuanLyNhanVienService());
        private ISoDoToChucService _soDoToChucService;
        private ISoDoToChucService SoDoToChucService => _soDoToChucService ?? (_soDoToChucService = new SoDoToChucService());
        public PhanCongNhanVienController(IPhanCongNhanVienService PhanCongNhanVienService)
        {
            _PhanCongNhanVienService = PhanCongNhanVienService;
        }

        public ActionResult Index()
        {
            int id = GetAnSessionInfo().ToaAnId;
            ViewBag.listToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnIDTheoNV(id, AccountUtils.CurrentUsername());
            ViewBag.listChucDanh = new SelectList(SoDoToChucService.DanhSachChucDanhTheoToaAn(id).OrderBy(x => x.ChucDanh), "SoDoToChucID", "ChucDanh");
            ViewBag.NhomAn = _PhanCongNhanVienService.DSNhomAn(id).OrderBy(x=>x.TenNhomAn).ToList();
            var nv = QuanLyNhanVienService.DanhsachNhanVienTheoToaAn(id);
            var model = new PhanCongViewModel();
            var list = new List<NhanVienNhomAnViewModel>();
            foreach (var n in nv)
            {
                var a = new NhanVienNhomAnViewModel();
                a.NhanVienID = n.NhanvienID;
                a.HoTen = n.HoVaTen;
                a.ChucDanh = n.ChucDanh;
                a.ischeck = false;
                list.Add(a);
            }
            model.listnv = list.OrderBy(x=>x.HoTen).ToList();
            return View(model);
        }
        public ActionResult ToaAnChangeNhomAn(int ToaAnID)
        {
            var model = _PhanCongNhanVienService.DSNhomAn(ToaAnID).OrderBy(x => x.TenNhomAn).ToList();
            return PartialView("~/Views/PhanCongNhanVien/_DSNhomAn.cshtml", model);
        }
        public ActionResult ToaAnChangeChucDanh(int ToaAnID)
        {
            var chucdanh = new SelectList(SoDoToChucService.DanhSachChucDanhTheoToaAn(ToaAnID), "SoDoToChucID", "ChucDanh").OrderBy(x=>x.Text);
            return Json(chucdanh, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ToaAnChangeNhanVien(int ToaAnID)
        {
            var nv = QuanLyNhanVienService.DanhsachNhanVienTheoToaAn(ToaAnID);
            var model = new PhanCongViewModel();
            var list = new List<NhanVienNhomAnViewModel>();
            foreach (var n in nv)
            {
                var a = new NhanVienNhomAnViewModel();
                a.NhanVienID = n.NhanvienID;
                a.HoTen = n.HoVaTen;
                a.ChucDanh = n.ChucDanh;
                a.ischeck = false;
                list.Add(a);
            }
            model.listnv = list.OrderBy(x => x.HoTen).ToList();
            return PartialView("~/Views/PhanCongNhanVien/_NhanVienChucDanh.cshtml", model);
        }
        public ActionResult NhomAnChanged(int NhomAnID, int ToaAnID)
        {
            var nv = QuanLyNhanVienService.DanhsachNhanVienTheoToaAn(ToaAnID);
            var nvcheck = _PhanCongNhanVienService.DSNhanVienNhomAn(NhomAnID);
            var model = new PhanCongViewModel();
            List<NhanVienNhomAnViewModel> list = new List<NhanVienNhomAnViewModel>();
            foreach (var n in nv)
            {
                var a = new NhanVienNhomAnViewModel();
                a.NhanVienID = n.NhanvienID;
                a.HoTen = n.HoVaTen;
                a.ChucDanh = n.ChucDanh;
                var x = nvcheck.Where(xx => xx.NhanVienID == n.NhanvienID).FirstOrDefault();
                if (x == null)
                    a.ischeck = false;
                else
                    a.ischeck = true;
                list.Add(a);
            }
            model.NhomAnID = NhomAnID;
            model.listnv = list.OrderBy(x => x.HoTen).ToList();
            return PartialView("~/Views/PhanCongNhanVien/_NhanVienChucDanh.cshtml", model);
        }
        public ActionResult ChucDanhChanged(int NhomAnID, int ToaAnID, int? SoDoToChucID)
        {
            var nv = _PhanCongNhanVienService.DSNhanVienTheoChucDanh(ToaAnID, SoDoToChucID);
            var nvcheck = _PhanCongNhanVienService.DSNhanVienNhomAn(NhomAnID);
            var model = new PhanCongViewModel();
            List<NhanVienNhomAnViewModel> list = new List<NhanVienNhomAnViewModel>();
            foreach (var n in nv)
            {
                var a = new NhanVienNhomAnViewModel();
                a.NhanVienID = n.NhanvienID;
                a.HoTen = n.HoVaTen;
                a.ChucDanh = n.ChucDanh;
                var x = nvcheck.Where(xx => xx.NhanVienID == n.NhanvienID).FirstOrDefault();
                if (x == null)
                    a.ischeck = false;
                else
                    a.ischeck = true;
                list.Add(a);
            }
            model.NhomAnID = NhomAnID;
            model.listnv = list.OrderBy(x => x.HoTen).ToList();
            return PartialView("~/Views/PhanCongNhanVien/_NhanVienChucDanh.cshtml", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Luu(PhanCongViewModel model)
        {
            var nvcheck = _PhanCongNhanVienService.DSNhanVienNhomAn(model.NhomAnID);
            try
            {
                foreach (var a in model.listnv)
                {
                    ResponseResult result = new ResponseResult();
                    var x = nvcheck.Where(xx => xx.NhanVienID == a.NhanVienID).FirstOrDefault();
                    if (a.ischeck)
                    {
                        if (x == null)
                            result = _PhanCongNhanVienService.ChonNhomAnNV(a.NhanVienID, model.NhomAnID);
                        else
                        {
                            result.ResponseCode = 1;
                            result.ResponseMessage = "OK";
                        }
                        if (result == null || result.ResponseCode != 1)
                            return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHEM_PHANCONG));
                    }
                    else if (a.ischeck != true)
                    {
                        if (x != null)
                            result = _PhanCongNhanVienService.HuyChonNhomAnNV_log(x.NhanVienNhomAnID, x.NhanVienID, x.NhomAnID);
                        else
                        {
                            result.ResponseCode = 1;
                            result.ResponseMessage = "OK";
                        }
                        if (result == null || result.ResponseCode != 1)
                            return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGXOA_PHANCONG));
                    }
                }
                return Json(JsonResponseViewModel.CreateSuccess());
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

    }
}