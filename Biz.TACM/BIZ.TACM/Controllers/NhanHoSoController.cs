using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.NhanDon.Model;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using Biz.TACM.Enums;
using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.Models.ViewModel.Shared;

namespace Biz.TACM.Controllers
{
    public class NhanHoSoController : BizController
    {
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        private INhanDonService _nhandonService;
        private INhanDonService NhanDonService => _nhandonService ?? (_nhandonService = new NhanDonService());

        [CustomAuthorize]
        public ActionResult Index(int id)
        {
            var anSession = GetAnSessionInfo();
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            if (giaiDoan == GiaiDoan.SoTham.GetHashCode())
            {
                return RedirectToAction("ChiTietHoSo", "NhanDon", new { id = id });
            }
            var viewModel = NhanDonService.ChiTietHoSoVuAnTheoId(id);
            UpdateAnSessionInfo(idToaAn: viewModel.ToaAnID, maToaAn: anSession.MaToaAn, maNhomAn: viewModel.NhomAn, idGiaiDoan: giaiDoan, idCongDoan: CongDoan.NhanHoSo.GetHashCode());
            
            ViewBag.ddlTrangThai = NhanDonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo, giaiDoan);
            ViewBag.ActiveCongDoan = viewModel.CongDoanHoSo;
            ViewBag.RoleGiaiDoan = viewModel.GiaiDoan == giaiDoan ? 1 : -1;

            return View(viewModel);
        }

        public ActionResult ChiTietNhanHoSo(int hoSoVuAnId)
        {
            var listNgayTao = NhanDonService.DanhSachNgaySuaNhanHoSo(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, 0);
            if (listNgayTao.Any())
            {
                var nhanHoSoId = Protect.ToInt32(listNgayTao.First().Value);
                var viewModel = NhanDonService.ChiTietNhanHoSoTheoId(nhanHoSoId);
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("_NhanHoSo", viewModel);
            }
            ViewBag.listNgayTao = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return PartialView("_NhanHoSo");
        }

        public ActionResult GetChiTietNhanHoSoTheoId(int nhanHoSoId)
        {
            var viewModel = NhanDonService.ChiTietNhanHoSoTheoId(nhanHoSoId);
            if (viewModel == null)
            {
                return PartialView("_ChiTietNhanHoSo");
            }
            ViewBag.listNgayTao = NhanDonService.DanhSachNgaySuaNhanHoSo(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, 0);
            return PartialView("_ChiTietNhanHoSo", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditChiTietNhanHoSo(int hoSoVuAnId, int nhanHoSoId)
        {

            var viewModel = new NhanHoSoModel();
            if (nhanHoSoId != 0)
            {
                viewModel = NhanDonService.ChiTietNhanHoSoTheoId(nhanHoSoId);
                return PartialView("_NhanHoSoModal", viewModel);
            }
            viewModel.HoSoVuAnID = hoSoVuAnId;
            return PartialView("_NhanHoSoModal", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditChiTietNhanHoSo(NhanHoSoModel viewModel)
        {
            try
            {
                viewModel.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
                viewModel.CongDoanHoSo = CongDoan.NhanHoSo.GetHashCode();

                ResponseResult result = NhanDonService.SuaChiTietNhanHoSo(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess(NotifyMessage.THONGBAO_CAPNHAT_HOSO_THANHCONG));
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_CAPNHAT_HOSO_KHONGTHANHCONG));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult GetChiTietChuyenHoSoTheoHoSoVuAnID(int hoSoVuAnId)
        {
            //chi tiet chuyen ho so phuc tham
            var viewModel = NhanDonService.ChiTietChuyenDonTheoHoSoVuAnID(hoSoVuAnId, GiaiDoan.SoTham.GetHashCode(), CongDoan.SauXetXu.GetHashCode());
            if (viewModel == null)
            {
                viewModel = NhanDonService.ChiTietChuyenDonTheoHoSoVuAnID(hoSoVuAnId, GiaiDoan.SoTham.GetHashCode(), CongDoan.ChuanBiXetXu.GetHashCode());
            }

            viewModel.NguoiTao = SettingDataService.GetSessionInfoNhanVienTheoMaNhanVien(viewModel.NguoiTao).HoTenVaMaNV;

            return PartialView("_ChiTietChuyenHoSo", viewModel);
        }
    }
}