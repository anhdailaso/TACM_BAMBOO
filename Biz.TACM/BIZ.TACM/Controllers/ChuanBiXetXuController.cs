using Biz.Lib.TACM.ChuanBiXetXu.Model;
using Biz.TACM.Models.ViewModel.ChuanBiXetXu;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.Lib.Helpers;
using Biz.TACM.Infrastructure.Attributes;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Authentication;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.Enums;
using ResponseResult = Biz.Lib.TACM.ChuanBiXetXu.Model.ResponseResult;
using HoSoVuAnModel = Biz.Lib.TACM.NhanDon.Model.HoSoVuAnModel;

namespace Biz.TACM.Controllers
{
    [WorkFlow(WorkFlow.ChuanBiXetXu)]
    public class ChuanBiXetXuController : BizController
    {
        private readonly IChuanBiXetXuService _chuanBiXetXuService;
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        private INhanDonService _nhandonService;
        private INhanDonService NhanDonService { get { return _nhandonService ?? (_nhandonService = new NhanDonService()); } }

        private IKetQuaXetXuService _ketQuaXetXuService;
        private IKetQuaXetXuService KetQuaXetXuService { get { return _ketQuaXetXuService ?? (_ketQuaXetXuService = new KetQuaXetXuService()); } }

        public ChuanBiXetXuController(IChuanBiXetXuService chuanBiXetXuService)
        {
            _chuanBiXetXuService = chuanBiXetXuService;
        }

        // GET: ChuanBiXetXu
        [CustomAuthorize]
        public ActionResult Index(int id)
        {
            var anSession = GetAnSessionInfo();            
            var viewModel = NhanDonService.ChiTietHoSoVuAnTheoId(id);
            
            if (viewModel == null)
            {
                viewModel = new HoSoVuAnModel();
                viewModel.HoSoVuAnID = id;
                viewModel.CongDoanHoSo = anSession.CongDoanId;
                viewModel.GiaiDoan = anSession.GiaiDoanId;
            }
            if (anSession.GiaiDoanId == 0)
                anSession.GiaiDoanId = viewModel.GiaiDoan;

            UpdateAnSessionInfo(idToaAn: viewModel.ToaAnID, maToaAn: anSession.MaToaAn, maNhomAn: viewModel.NhomAn, idGiaiDoan: anSession.GiaiDoanId, idCongDoan: CongDoan.ChuanBiXetXu.GetHashCode(), idVuAn: viewModel.HoSoVuAnID);

            //Kiem tra kqxx so tham ra ban an hay quyet dinh
            ViewBag.LoaiKetQuaXetXu = LoaiKetQuaXetXu.BanAn.GetHashCode();
            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                viewModel.LoaiKetQuaXetXu = GetLoaiKetQuaXetXu(id);
                ViewBag.LoaiKetQuaXetXu = viewModel.LoaiKetQuaXetXu;
            }            

            ViewBag.ActiveCongDoan = viewModel.CongDoanHoSo;
            ViewBag.ddlTrangThai = NhanDonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo, anSession.GiaiDoanId);

            var giaiDoanHoSo = viewModel.GiaiDoan;
            ViewBag.RoleGiaiDoan = giaiDoanHoSo == anSession.GiaiDoanId ? 1 : -1;

            //reload danh sach hosovuan is in role
            Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);

            return View(viewModel);
        }

        private int GetLoaiKetQuaXetXu(int hoSoVuAnId)
        {
            int loai = 0;
            var modelKQXXBanAn = KetQuaXetXuService.ChiTietBanAnTheoHoSoVuAnID(hoSoVuAnId, GiaiDoan.SoTham.GetHashCode());
            var modelKQXXQuyetDinh = KetQuaXetXuService.ChiTietQuyetDinhTheoHoSoVuAnID(hoSoVuAnId, GiaiDoan.SoTham.GetHashCode());
            if (modelKQXXBanAn != null && modelKQXXQuyetDinh != null)
            {
                loai = LoaiKetQuaXetXu.BanAnVaQuyetDinh.GetHashCode();
            }
            else if (modelKQXXBanAn != null)
            {
                loai = LoaiKetQuaXetXu.BanAn.GetHashCode();
            }
            else if (modelKQXXQuyetDinh != null)
            {
                loai = LoaiKetQuaXetXu.QuyetDinh.GetHashCode();
            }
            return loai;
        }

        #region HoaGiai
        public ActionResult GetChuanBiXetXuHoaGiaiTheoHoSoVuAnID(int id)
        {
            var viewModel = _chuanBiXetXuService.ChiTietHoaGiaiTheoHoSoVuAnID(id, GetAnSessionInfo().GiaiDoanId);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _chuanBiXetXuService.DanhSachNgayHoaGiai(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, 0);
            }

            return PartialView("~/Views/ChuanBiXetXu/HoaGiai/_ThongTinHoaGiai.cshtml", viewModel);
        }

        public ActionResult GetChuanBiXetXuHoaGiaiTheoHoaGiaiID(int id)
        {
            var viewModel = _chuanBiXetXuService.ChiTietHoaGiaiTheoHoaGiaiID(id);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _chuanBiXetXuService.DanhSachNgayHoaGiai(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, 0);
            }

            return PartialView("~/Views/ChuanBiXetXu/HoaGiai/_ThongTinHoaGiai.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditChuanBiXetXuHoaGiai(int id)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _chuanBiXetXuService.ChiTietHoaGiaiTheoHoSoVuAnID(id, anSession.GiaiDoanId);

            if (viewModel == null)
            {
                viewModel = new HoaGiaiModel();
                viewModel.HoSoVuAnID = id;
                viewModel.NgayMoPhienHop = DateTime.Now.Date.Add(new TimeSpan(7, 30, 0));
                //ViewBag.GioMoPhienHopDDL = SettingDataService.SettingDataItemXML("Hour", "7");
                //ViewBag.PhutMoPhienHopDDL = SettingDataService.SettingDataItemXML("Minute", "30");

                var modelHoSoVuAn = NhanDonService.ChiTietHoSoVuAn(id);
                viewModel.ThamPhan = modelHoSoVuAn.ThamPhan;
                viewModel.ThuKy = modelHoSoVuAn.ThuKy;
            }

            var danhSachQuyetDinh = _chuanBiXetXuService.DanhSachQuyetDinh(viewModel.HoSoVuAnID, anSession.GiaiDoanId);
            var quyetDinh = danhSachQuyetDinh.OrderBy(m => m.NgayRaQuyetDinh).FirstOrDefault();
            ViewBag.NgayRaQuyetDinh = (quyetDinh != null ? quyetDinh.NgayRaQuyetDinh.AddDays(-1) : DateTime.MaxValue);

            ViewBag.ddlThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh("Thẩm Phán", anSession.ToaAnId, "");
            ViewBag.ddlThuKy = SettingDataService.DanhSachNhanVienTheoChucDanh("Thư Ký", anSession.ToaAnId, "");
            ViewBag.MaNhomAn = GetAnSessionInfo().MaNhomAn;

            ViewBag.GioMoPhienHopDDL = SettingDataService.SettingDataItemXML("Hour", viewModel.NgayMoPhienHop.Hour.ToString());
            ViewBag.PhutMoPhienHopDDL = SettingDataService.SettingDataItemXML("Minute", viewModel.NgayMoPhienHop.Minute.ToString());

            //if (viewModel.ThamPhan != null)
            //{
            //    ViewBag.ddlThuKy = _chuanBiXetXuService.SelectListThuKyTheoThamPhan(viewModel.ThamPhan, null);
            //}

            return PartialView("~/Views/ChuanBiXetXu/HoaGiai/_HoaGiaiModel.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditChuanBiXetXuHoaGiai(HoaGiaiModel viewModel, int GioMoPhienHop, int PhutMoPhienHop)
        {
            var anSession = GetAnSessionInfo();
            var danhSachQuyetDinh = _chuanBiXetXuService.DanhSachQuyetDinh(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId);
            var quyetDinh = danhSachQuyetDinh.OrderBy(m => m.NgayRaQuyetDinh).FirstOrDefault();

            viewModel.NgayMoPhienHop = viewModel.NgayMoPhienHop.Date.Add(new TimeSpan(GioMoPhienHop, PhutMoPhienHop, 0));

            if (quyetDinh != null && viewModel.NgayMoPhienHop >= quyetDinh.NgayRaQuyetDinh)
            {
                ModelState.AddModelError("NgayMoPhienHop", ValidationMessages.VALIDATION_GIOIHAN_NGAYMOPHIENHOP);
            }

            if (ModelState.IsValid)
            {
                ResponseResult result = _chuanBiXetXuService.ThemHoaGiai(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    Success(result.ResponseMessage);
                    return RedirectToAction("GetChuanBiXetXuHoaGiaiTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }
                        
            ViewBag.NgayRaQuyetDinh = (quyetDinh != null ? quyetDinh.NgayRaQuyetDinh.AddDays(-1) : DateTime.MaxValue);

            ViewBag.ddlThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh("Thẩm Phán", anSession.ToaAnId, "");
            ViewBag.ddlThuKy = SettingDataService.DanhSachNhanVienTheoChucDanh("Thư Ký", anSession.ToaAnId, "");
            ViewBag.MaNhomAn = GetAnSessionInfo().MaNhomAn;

            ViewBag.GioMoPhienHopDDL = SettingDataService.SettingDataItemXML("Hour", viewModel.NgayMoPhienHop.Hour.ToString());
            ViewBag.PhutMoPhienHopDDL = SettingDataService.SettingDataItemXML("Minute", viewModel.NgayMoPhienHop.Minute.ToString());

            //if (viewModel.ThamPhan != null)
            //{
            //    ViewBag.ddlThuKy = _chuanBiXetXuService.SelectListThuKyTheoThamPhan(viewModel.ThamPhan, null);
            //}

            return PartialView("~/Views/ChuanBiXetXu/HoaGiai/_HoaGiaiModel.cshtml", viewModel);
        }

        public JsonResult GetThuKyTheoThamPhan(string manv)
        {
            var nhanVien = _chuanBiXetXuService.DanhSachThuKyTheoThamPhan(manv).OrderBy(x=>x.HoVaTenNV);           
            return Json(nhanVien, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ResetThuKyTheoThamPhan(string manv)
        {
            var anSession = GetAnSessionInfo();
            var nhanVien = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THUKY, anSession.ToaAnId, "");
            return Json(nhanVien, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region QuyetDinh
        public ActionResult GetChuanBiXetXuDanhSachQuyetDinhTheoHoSoVuAnID(int id)
        {
            var viewModel = _chuanBiXetXuService.DanhSachQuyetDinh(id, GetAnSessionInfo().GiaiDoanId);

            return PartialView("~/Views/ChuanBiXetXu/QuyetDinh/_ThongTinQuyetDinh.cshtml", viewModel);
        }

        public ActionResult GetChuanBiXetXuQuyetDinhTheoQuyetDinhID(int id)
        {
            var viewModel = _chuanBiXetXuService.ChiTietQuyetDinhTheoQuyetDinhID(id);
            return PartialView("QuyetDinh/_ChiTietQuyetDinh", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditChuanBiXetXuQuyetDinhTheoQuyetDinhID(int id)
        {
            var anSession = GetAnSessionInfo();
            
            var viewModel = _chuanBiXetXuService.ChiTietQuyetDinhTheoQuyetDinhID(id);

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("HC_SOTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
            }
            else
            {
                ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("DS_SOTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
            }

            ViewBag.ddlTenQuyetDinh = InitTenQuyetDinh(anSession.MaNhomAn, anSession.GiaiDoanId, CBXXQuyetDinhLoai.QuyetDinh.GetHashCode());
            ViewBag.ddlQuyetDinhLoai = _chuanBiXetXuService.SelectListQuyetDinhLoai(viewModel.QuyetDinhLoai.ToString());           
            ViewBag.NgayMoPhienHop = DateTime.MinValue;

            var modelHoaGiai = _chuanBiXetXuService.ChiTietHoaGiaiTheoHoSoVuAnID(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId);            
            if (modelHoaGiai != null)
            {
                ViewBag.NgayMoPhienHop = modelHoaGiai.NgayMoPhienHop.AddDays(1);
            }

            return PartialView("~/Views/ChuanBiXetXu/QuyetDinh/_QuyetDinhModel.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult ThemChuanBiXetXuQuyetDinh(int id)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = new QuyetDinhModel
            {
                HoSoVuAnID = id,
                NgayRaQuyetDinh = DateTime.Now
            };

            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                viewModel.QuyetDinhLoai = CBXXQuyetDinhLoai.QuyetDinh.GetHashCode();
            }

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
                {
                    ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("HC_PHUCTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
                }
                else
                {
                    ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("HC_SOTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
                }               
            }
            else
            {
                if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
                {
                    ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("DS_PHUCTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
                }
                else
                {
                    ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("DS_SOTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
                }
            }

            ViewBag.ddlTenQuyetDinh = InitTenQuyetDinh(anSession.MaNhomAn, anSession.GiaiDoanId, CBXXQuyetDinhLoai.QuyetDinh.GetHashCode());
            ViewBag.ddlQuyetDinhLoai = _chuanBiXetXuService.SelectListQuyetDinhLoai(viewModel.QuyetDinhLoai.ToString());
            //ViewBag.ddlTenQuyetDinh = SettingDataService.SettingDataItem("DS_SOTHAM_TENQUYETDINH", "");

            var modelHoaGiai = _chuanBiXetXuService.ChiTietHoaGiaiTheoHoSoVuAnID(id, GetAnSessionInfo().GiaiDoanId);
            ViewBag.NgayMoPhienHop = DateTime.MinValue;

            if (modelHoaGiai != null)
            {
                ViewBag.NgayMoPhienHop = modelHoaGiai.NgayMoPhienHop.AddDays(1);
            }

            return PartialView("~/Views/ChuanBiXetXu/QuyetDinh/_QuyetDinhModel.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemChuanBiXetXuQuyetDinh(QuyetDinhModel viewModel)
        {
            var anSession = GetAnSessionInfo();
            if (viewModel.TenQuyetDinh.Trim().ToLower() != Setting.QUYETDINH_GIAHANTHOIHAN)
            {
                ModelState["ThoiHanGiaHan"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                ResponseResult result = null;

                if (viewModel.QuyetDinhID > 0)
                {
                    result = _chuanBiXetXuService.SuaQuyetDinh(viewModel);
                }
                else {
                    result = _chuanBiXetXuService.ThemQuyetDinh(viewModel);
                }

                if (result != null && result.ResponseCode == 1)
                {
                    Success(result.ResponseMessage);
                    return RedirectToAction("GetChuanBiXetXuDanhSachQuyetDinhTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }
            if (anSession.MaNhomAn.Equals(Setting.MANHOMAN_APDUNG_BPXLHC))
            {
                ViewBag.ddlQuyetDinhLoai = _chuanBiXetXuService.SelectListQuyetDinhLoai("DS_SOTHAM_TENQUYETDINH");
            }
            else
            {
                ViewBag.ddlQuyetDinhLoai = _chuanBiXetXuService.SelectListQuyetDinhLoai(viewModel.QuyetDinhLoai.ToString());
            }

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
                {
                    ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("HC_PHUCTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
                }
                else
                {
                    ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("HC_SOTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
                }
            }
            else
            {
                if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
                {
                    ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("DS_PHUCTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
                }
                else
                {
                    ViewBag.ddlThoiHanGiaHan = SettingDataService.SettingDataItem("DS_SOTHAM_THOIHANGIAHAN_CHUANBIXETXU", "");
                }
            }

            ViewBag.ddlTenQuyetDinh = InitTenQuyetDinh(anSession.MaNhomAn, anSession.GiaiDoanId, viewModel.QuyetDinhLoai);
            ViewBag.ddlQuyetDinhLoai = _chuanBiXetXuService.SelectListQuyetDinhLoai(viewModel.QuyetDinhLoai.ToString());

            var modelHoaGiai = _chuanBiXetXuService.ChiTietHoaGiaiTheoHoSoVuAnID(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId);

            ViewBag.NgayMoPhienHop = DateTime.MinValue;

            if (modelHoaGiai != null)
            {
                ViewBag.NgayMoPhienHop = modelHoaGiai.NgayMoPhienHop.AddDays(1);
            }

            return PartialView("~/Views/ChuanBiXetXu/QuyetDinh/_QuyetDinhModel.cshtml", viewModel);
        }

        public ActionResult XoaChuanBiXetXuQuyetDinh(int id)
        {
            var viewModel = _chuanBiXetXuService.ChiTietQuyetDinhTheoQuyetDinhID(id);

            _chuanBiXetXuService.XoaQuyetDinh(viewModel.QuyetDinhID);

            return RedirectToAction("GetChuanBiXetXuDanhSachQuyetDinhTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
        }

        //public JsonResult GetDanhSachQuyetDinh(int id)
        //{
        //    var nhanVien = _chuanBiXetXuService.DLDanhSachQuyetDinh(id);

        //    return Json(nhanVien, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetDanhSachTenQuyetDinh(int quyetDinhLoai)
        {
            var anSession = GetAnSessionInfo();
            var listTenQuyetDinh = InitTenQuyetDinh(anSession.MaNhomAn, anSession.GiaiDoanId, quyetDinhLoai);

            return Json(listTenQuyetDinh, JsonRequestBehavior.AllowGet);
        }

        private SelectList InitTenQuyetDinh(string maNhomAn, int giaiDoan, int quyetDinhLoai)
        {
            SelectList listTenQuyetDinh;
            if (maNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                if (quyetDinhLoai == CBXXQuyetDinhLoai.QuyetDinh.GetHashCode())
                {
                    if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("HC_PHUCTHAM_TENQUYETDINH", "");
                    }
                    else
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("HC_SOTHAM_TENQUYETDINH", "");
                    }
                }
                else
                {
                    if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("HC_PHUCTHAM_TENTHONGBAO", "");
                    }
                    else
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("HC_SOTHAM_TENTHONGBAO", "");
                    }
                }
            }
            else if (maNhomAn.Equals(Setting.MANHOMAN_APDUNG_BPXLHC))
            {
                
                if (quyetDinhLoai == CBXXQuyetDinhLoai.QuyetDinh.GetHashCode())
                {
                    if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("AD_PHUCTHAM_TENQUYETDINH", "");
                    }
                    else
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("AD_SOTHAM_TENQUYETDINH", "");
                    }
                }
                else
                {
                    if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("AD_PHUCTHAM_TENTHONGBAO", "");
                    }
                    else
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("AD_SOTHAM_TENTHONGBAO", "");
                    }
                }

            }
            else if (maNhomAn.Equals(Setting.MANHOMAN_HINHSU))
            {
                if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                {
                    listTenQuyetDinh = SettingDataService.SettingDataItem("HS_PHUCTHAM_TENQUYETDINH", "");
                }
                else
                {
                    listTenQuyetDinh = SettingDataService.SettingDataItem("HS_SOTHAM_TENQUYETDINH", "");
                }
            }
            else
            {
                if (quyetDinhLoai == CBXXQuyetDinhLoai.QuyetDinh.GetHashCode())
                {
                    if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("DS_PHUCTHAM_TENQUYETDINH", "");
                    }
                    else
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("DS_SOTHAM_TENQUYETDINH", "");
                    }
                }
                else
                {
                    if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("DS_PHUCTHAM_TENTHONGBAO", "");
                    }
                    else
                    {
                        listTenQuyetDinh = SettingDataService.SettingDataItem("DS_SOTHAM_TENTHONGBAO", "");
                    }
                }
            }
            return listTenQuyetDinh;
        }
        #endregion

        #region QuyetDinh HinhSu

        public ActionResult GetChuanBiXetXuQuyetDinhHinhSuTheoHoSoVuAnID(int id, int quyetDinhGroup)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _chuanBiXetXuService.ChiTietQuyetDinhHinhSuTheoHoSoVuAnId(id, anSession.GiaiDoanId, quyetDinhGroup);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _chuanBiXetXuService.DanhSachNgaySuaQuyetDinhHinhSu(viewModel.HoSoVuAnID, anSession.GiaiDoanId, quyetDinhGroup, 0);
            }

            if (quyetDinhGroup == QuyetDinhHinhSuGroup.TraHoSoDieuTraBoSung.GetHashCode())
            {
                return PartialView("TraHoSoDieuTraBoSung/_ThongTinTraHoSoDieuTraBoSung", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.TamDinhChiVuAn.GetHashCode())
            {
                return PartialView("TamDinhChiVuAn/_ThongTinTamDinhChiVuAn", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.DinhChiVuAn.GetHashCode())
            {
                return PartialView("DinhChiVuAn/_ThongTinDinhChiVuAn", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.PhucHoiVuAn.GetHashCode())
            {
                return PartialView("PhucHoiVuAn/_ThongTinPhucHoiVuAn", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.DinhChiXetXuPhucTham.GetHashCode())
            {
                return PartialView("DinhChiXetXuPhucTham/_ThongTinDinhChi", viewModel);
            }
            return PartialView("TraHoSoDieuTraBoSung/_ThongTinTraHoSoDieuTraBoSung", viewModel);
        }

        public ActionResult GetChuanBiXetXuQuyetDinhHinhSuTheoQuyetDinhID(int id, int quyetDinhGroup)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _chuanBiXetXuService.ChiTietQuyetDinhHinhSuTheoQuyetDinhId(id);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _chuanBiXetXuService.DanhSachNgaySuaQuyetDinhHinhSu(viewModel.HoSoVuAnID, anSession.GiaiDoanId, viewModel.QuyetDinhGroup, 0);
            }

            if (quyetDinhGroup == QuyetDinhHinhSuGroup.TraHoSoDieuTraBoSung.GetHashCode())
            {
                return PartialView("TraHoSoDieuTraBoSung/_ThongTinTraHoSoDieuTraBoSung", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.TamDinhChiVuAn.GetHashCode())
            {
                return PartialView("TamDinhChiVuAn/_ThongTinTamDinhChiVuAn", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.DinhChiVuAn.GetHashCode())
            {
                return PartialView("DinhChiVuAn/_ThongTinDinhChiVuAn", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.PhucHoiVuAn.GetHashCode())
            {
                return PartialView("PhucHoiVuAn/_ThongTinPhucHoiVuAn", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.DinhChiXetXuPhucTham.GetHashCode())
            {
                return PartialView("DinhChiXetXuPhucTham/_ThongTinDinhChi", viewModel);
            }
            return PartialView("TraHoSoDieuTraBoSung/_ThongTinTraHoSoDieuTraBoSung", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditChuanBiXetXuQuyetDinhHinhSu(int id, int quyetDinhGroup)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _chuanBiXetXuService.ChiTietQuyetDinhHinhSuTheoHoSoVuAnId(id, anSession.GiaiDoanId, quyetDinhGroup);

            ViewBag.SoQuyetDinh = viewModel != null ? viewModel.SoQuyetDinh : KetQuaXetXuService.SoQuyetDinhMax(id, DateTime.Now).ToString();

            if (viewModel == null)
            {
                viewModel = new QuyetDinhHinhSuModel();
                viewModel.HoSoVuAnID = id;
                viewModel.NgayRaQuyetDinh = DateTime.Now;
                viewModel.NgayRutHoSo = DateTime.Now;
                
            }
            viewModel.QuyetDinhGroup = quyetDinhGroup;

            if (quyetDinhGroup == QuyetDinhHinhSuGroup.TraHoSoDieuTraBoSung.GetHashCode())
            {
                ViewBag.listLyDoTraHoSoDieuTraBoSung = _chuanBiXetXuService.DanhSachLyDoSelected("HS_SOTHAM_LYDO_TRAHOSO_DIEUTRABOSUNG", viewModel.DanhSachLyDo);
                ViewBag.ddlLanThu = SettingDataService.SelectListLanThu(viewModel.LanThu);
                return PartialView("TraHoSoDieuTraBoSung/_TraHoSoDieuTraBoSungModal", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.TamDinhChiVuAn.GetHashCode())
            {
                ViewBag.listLyDoTamDinhChiVuAn = _chuanBiXetXuService.DanhSachLyDoSelected("HS_SOTHAM_LYDO_TAMDINHCHIVUAN", viewModel.DanhSachLyDo);
                return PartialView("TamDinhChiVuAn/_TamDinhChiVuAnModal", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.DinhChiVuAn.GetHashCode())
            {
                ViewBag.listLyDoDinhChiVuAn = _chuanBiXetXuService.DanhSachLyDoSelected("HS_SOTHAM_LYDO_DINHCHIVUAN", viewModel.DanhSachLyDo);
                return PartialView("DinhChiVuAn/_DinhChiVuAnModal", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.PhucHoiVuAn.GetHashCode())
            {
                return PartialView("PhucHoiVuAn/_PhucHoiVuAnModal", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.DinhChiXetXuPhucTham.GetHashCode())
            {
                return PartialView("DinhChiXetXuPhucTham/_DinhChiModal", viewModel);
            }
            ViewBag.listLyDoTraHoSoDieuTraBoSung = _chuanBiXetXuService.DanhSachLyDoSelected("HS_SOTHAM_LYDO_TRAHOSO_DIEUTRABOSUNG", viewModel.DanhSachLyDo);
            return PartialView("TraHoSoDieuTraBoSung/_TraHoSoDieuTraBoSungModal", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditChuanBiXetXuQuyetDinhHinhSu(QuyetDinhHinhSuModel viewModel)
        {
            var quyetDinhGroup = viewModel.QuyetDinhGroup;

            //kiem tra trung so
            var model = KetQuaXetXuService.ChiTietQuyetDinhTheoSoQuyetDinh(viewModel.SoQuyetDinh, viewModel.HoSoVuAnID, viewModel.NgayRaQuyetDinh);
            if (model != null && model.HoSoVuAnID != viewModel.HoSoVuAnID)
            {
                ModelState.AddModelError("SoQuyetDinh", NotifyMessage.WARNING_TRUNG_SOQUYETDINH);
            }

            if (quyetDinhGroup == QuyetDinhHinhSuGroup.PhucHoiVuAn.GetHashCode())
            {
                ModelState.Remove("NgayRutHoSo");
                ModelState.Remove("DanhSachLyDo");
            }
            else if(quyetDinhGroup == QuyetDinhHinhSuGroup.DinhChiXetXuPhucTham.GetHashCode())
            {
                ModelState.Remove("LyDo");
                ModelState.Remove("DanhSachLyDo");
            }
            else
            {
                ModelState.Remove("NgayRutHoSo");
                ModelState.Remove("LyDo");
            }
            if (ModelState.IsValid)
            {
                ResponseResult result = _chuanBiXetXuService.ThemQuyetDinhHinhSu(viewModel, quyetDinhGroup);

                if (result != null && result.ResponseCode == 1)
                {
                    return RedirectToAction("GetChuanBiXetXuQuyetDinhHinhSuTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID, quyetDinhGroup = viewModel.QuyetDinhGroup });
                }
            }

            
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.TraHoSoDieuTraBoSung.GetHashCode())
            {
                ViewBag.listLyDoTraHoSoDieuTraBoSung = _chuanBiXetXuService.DanhSachLyDoSelected("HS_SOTHAM_LYDO_TRAHOSO_DIEUTRABOSUNG", viewModel.DanhSachLyDo);
                ViewBag.ddlLanThu = SettingDataService.SelectListLanThu(viewModel.LanThu);
                return PartialView("TraHoSoDieuTraBoSung/_TraHoSoDieuTraBoSungModal", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.TamDinhChiVuAn.GetHashCode())
            {
                ViewBag.listLyDoTamDinhChiVuAn = _chuanBiXetXuService.DanhSachLyDoSelected("HS_SOTHAM_LYDO_TAMDINHCHIVUAN", viewModel.DanhSachLyDo);
                return PartialView("TamDinhChiVuAn/_TamDinhChiVuAnModal", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.DinhChiVuAn.GetHashCode())
            {
                ViewBag.listLyDoDinhChiVuAn = _chuanBiXetXuService.DanhSachLyDoSelected("HS_SOTHAM_LYDO_DINHCHIVUAN", viewModel.DanhSachLyDo);
                return PartialView("DinhChiVuAn/_DinhChiVuAnModal", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.PhucHoiVuAn.GetHashCode())
            {
                return PartialView("PhucHoiVuAn/_PhucHoiVuAnModal", viewModel);
            }
            if (quyetDinhGroup == QuyetDinhHinhSuGroup.DinhChiXetXuPhucTham.GetHashCode())
            {
                return PartialView("DinhChiXetXuPhucTham/_DinhChiModal", viewModel);
            }
            ViewBag.listLyDoTraHoSoDieuTraBoSung = SettingDataService.SettingDataItem("HS_SOTHAM_LYDO_TRAHOSO_DIEUTRABOSUNG", "");
            return PartialView("TraHoSoDieuTraBoSung/_TraHoSoDieuTraBoSungModal", viewModel);
        }

        #endregion

        #region ChuyenHoSo  

        public ActionResult ChuyenHoSo(int hoSoVuAnId)
        {
            var listNgayTao = NhanDonService.GetDanhSachNgaySuaDoiChuyenDon(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, 0);
            if (listNgayTao.Any())
            {
                var chuyenHoSoId = Protect.ToInt32(listNgayTao.First().Value);
                var viewModel = _chuanBiXetXuService.ChiTietChuyenHoSoTheoId(chuyenHoSoId);
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("ChuyenHoSo/Detail", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return PartialView("ChuyenHoSo/Detail");
        }

        public ActionResult GetChuyenHoSoTheoId(int chuyenHoSoId)
        {
            var model = _chuanBiXetXuService.ChiTietChuyenHoSoTheoId(chuyenHoSoId);

            if (model != null)
            {
                ViewBag.listNgayTao = NhanDonService.GetDanhSachNgaySuaDoiChuyenDon(model.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId,GetAnSessionInfo().CongDoanId, 0);
                return PartialView("ChuyenHoSo/Detail",model);
            }
            return PartialView("ChuyenHoSo/Detail");
        }

        [CustomAuthorize]
        public ActionResult EditChuyenHoSo(int hoSoVuAnId, int chuyenHoSoId)
        {
            var viewModel = _chuanBiXetXuService.ChiTietChuyenHoSoTheoId(chuyenHoSoId);
            var toaAnActive = GetAnSessionInfo().ToaAnId;
            var listToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnID(null).Where(x => x.Value != toaAnActive.ToString()).ToList();
            string vungChuyenDon;
            if (viewModel == null)
            {
                viewModel = new ChuyenHoSoViewModel
                {
                    CongDoanHoSo = GetAnSessionInfo().CongDoanId,
                    GiaiDoan = GetAnSessionInfo().GiaiDoanId,
                    HoSoVuAnID = hoSoVuAnId
                };
                vungChuyenDon = Setting.VUNG_CHUYENHOSO_TRONGTINH;
            }
            else
            {
                vungChuyenDon = Setting.VUNG_CHUYENHOSO_NGOAITINH;
                foreach (var item in listToaAn)
                {
                    if (item.Text == viewModel.ToaAnChuyenDen)
                    {
                        vungChuyenDon = Setting.VUNG_CHUYENHOSO_TRONGTINH;
                        break;
                    }
                }
            }

            ViewBag.listLyDoChuyen = _settingDataService.SettingDataItem("DS_SOTHAM_LYDOCHUYENHOSO_CHUANBIXETXU", "");
            ViewBag.listVungChuyenDon = NhanDonService.SelectListVungChuyenDon(vungChuyenDon);
            ViewBag.listToaAnHuyen = new SelectList(listToaAn, "Text", "Text");

            return PartialView("ChuyenHoSo/Edit", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditChuyenHoSo(ChuyenHoSoViewModel viewModel)
        {
            try
            {
                var toaAnActive = SettingDataService.GetToaAnTheoToaAnID(GetAnSessionInfo().ToaAnId);
                viewModel.ToaAnChuyenDi = toaAnActive.TenToaAn;
                viewModel.TrangThaiCongDoan = Setting.TRANGTHAICONGDOAN_CHUYENHOSO;

                var result = _chuanBiXetXuService.SuaDoiChuyenHoSo(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    var toaAnChuyen = SettingDataService.GetToaAnTheoTenToaAn(viewModel.ToaAnChuyenDi);
                    var toaAnNhan = SettingDataService.GetToaAnTheoTenToaAn(viewModel.ToaAnChuyenDen);
                    var hoSo = NhanDonService.ChiTietHoSoVuAn(viewModel.HoSoVuAnID);
                    if (toaAnNhan != null)
                    {
                        if (viewModel.LyDoChuyenHoSo == "Xét kháng cáo quyết định tạm đình chỉ")
                        {
                            try
                            {
                                GuiEmailChuyenHoSoPhucTham(hoSo, viewModel, toaAnChuyen, toaAnNhan);
                            }
                            catch (Exception ex)
                            {
                                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.THONGBAO_KHONGTHANHCONG, ViewText.LABEL_GUI_EMAIL)));
                            }                           
                        }
                        else
                        {
                            try
                            {
                                GuiEmailChuyenHoSoSangToaAnKhacSoTham(hoSo, viewModel, toaAnChuyen, toaAnNhan);
                            }
                            catch (Exception ex)
                            {
                                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.THONGBAO_KHONGTHANHCONG, ViewText.LABEL_GUI_EMAIL)));
                            }                            
                        }
                    }
                    //reload danh sach hosovuan is in role
                    Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);

                    return Json(JsonResponseViewModel.CreateSuccess(string.Format(NotifyMessage.CAPNHAT_THANHCONG, ViewText.LABEL_CHUYENHOSO)));
                }
                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.CAPNHAT_KHONGTHANHCONG, ViewText.LABEL_CHUYENHOSO)));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public void GuiEmailChuyenHoSoSangToaAnKhacSoTham(HoSoVuAnModel hoSoModel, ChuyenHoSoViewModel chuyenDonModel, ToaAnModel toaAnChuyen, ToaAnModel toaAnNhan)
        {
            var nv = SettingDataService.GetNhanVienSessionInfo();
            string mailBody = EmailTemplate.CHUYENHOSO_EMAIL_BODY;
            mailBody = mailBody.Replace("{_ToaAnNhan_}", toaAnNhan.TenToaAn);
            mailBody = mailBody.Replace("{_ToaAnChuyen_}", toaAnChuyen.TenToaAn);
            mailBody = mailBody.Replace("{_MaHoSo_}", hoSoModel.MaHoSo);
            mailBody = mailBody.Replace("{_NgayChuyen_}", chuyenDonModel.NgayChuyenHoSo);
            mailBody = mailBody.Replace("{_NguoiChuyen_}", nv.HoVaTen + " (" + nv.MaNVMoi + ")");
            mailBody = mailBody.Replace("{_LyDoChuyen_}", chuyenDonModel.LyDoChuyenHoSo);
            mailBody = mailBody.Replace("{_EmailToaAnChuyen_}", toaAnChuyen.Email);
            Commons.SendMail(EmailTemplate.CHUYENHOSO_EMAIL_SUBJECT, toaAnNhan.Email, mailBody, cc: toaAnChuyen.Email);
        }

        public void GuiEmailChuyenHoSoPhucTham(HoSoVuAnModel hoSoModel, ChuyenHoSoViewModel chuyenHoSoModel, ToaAnModel toaAnChuyen, ToaAnModel toaAnNhan)
        {
            var nv = SettingDataService.GetNhanVienSessionInfo();
            string mailBody = EmailTemplate.CHUYENHOSOPHUCTHAM_EMAIL_BODY;
            mailBody = mailBody.Replace("{_ToaAnNhan_}", toaAnNhan.TenToaAn);
            mailBody = mailBody.Replace("{_ToaAnChuyen_}", toaAnChuyen.TenToaAn);
            mailBody = mailBody.Replace("{_MaHoSo_}", hoSoModel.MaHoSo);
            mailBody = mailBody.Replace("{_NgayChuyen_}", chuyenHoSoModel.NgayChuyenHoSo);
            mailBody = mailBody.Replace("{_NguoiChuyen_}", nv.HoVaTen + " (" + nv.MaNVMoi + ")");
            mailBody = mailBody.Replace("{_TruongHopChuyen_}", chuyenHoSoModel.LyDoChuyenHoSo);
            mailBody = mailBody.Replace("{_GhiChu_}", chuyenHoSoModel.GhiChu);
            mailBody = mailBody.Replace("{_EmailToaAnChuyen_}", toaAnChuyen.Email);
            Commons.SendMail(EmailTemplate.CHUYENHOSOPHUCTHAM_EMAIL_SUBJECT, toaAnNhan.Email, mailBody, cc: toaAnChuyen.Email);
        }
        #endregion
    }
}