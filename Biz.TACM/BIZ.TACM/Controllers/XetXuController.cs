using Biz.Lib.TACM.XetXu.Model;
using Biz.TACM.Infrastructure.Attributes;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.Lib.TACM.Resources.Resources;
using System.Web.Mvc;
using Biz.Lib.Authentication;
using Biz.TACM.Enums;

namespace Biz.TACM.Controllers
{
    [WorkFlow(WorkFlow.ChuanBiXetXu)]
    public class XetXuController : BizController
    {
        private readonly IXetXuService _xetXuService;
        private readonly ISettingDataService _settingDataService;
        private readonly INhanDonService _nhandonService;

        public XetXuController(IXetXuService xetXuService, ISettingDataService settingDataService, INhanDonService nhanDonService)
        {
            _xetXuService = xetXuService;
            _settingDataService = settingDataService;
            _nhandonService = nhanDonService;
        }

        // GET: XetXu
        //public ActionResult Index(int id)
        //{
        //    var viewModel = NhanDonService.ChiTietHoSoVuAn(id);
        //    ViewBag.ActiveCongDoan = viewModel.CongDoanHoSo;
        //    ViewBag.ddlTrangThai = NhanDonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo);

        //    return View("~/Views/ChuanBiXetXu/IndexXet",viewModel);
        //}

        #region Noi dung
        public ActionResult GetXetXuNoiDungTheoHoSoVuAnID(int id, int xetXuGroup)
        {
            var anSession = GetAnSessionInfo();
            ViewBag.MaNhomAn = anSession.MaNhomAn;
            var viewModel = _xetXuService.ChiTietXetXuTheoHoSoVuAnID(id, anSession.GiaiDoanId, xetXuGroup);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _xetXuService.DanhSachNgayXetXu(viewModel.HoSoVuAnID, anSession.GiaiDoanId, xetXuGroup, 0);
            }

            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                if (anSession.MaNhomAn.Equals(Setting.MANHOMAN_HANHCHINH) && xetXuGroup == XetXuGroup.MoPhienHop.GetHashCode())
                {
                    return PartialView("~/Views/ChuanBiXetXu/NoiDung/_MoPhienHopHanhChinhPhucThamThongTin.cshtml", viewModel);
                }
                return PartialView("~/Views/ChuanBiXetXu/NoiDung/_ThongTinNoiDungPhucTham.cshtml", viewModel);
            }
            return PartialView("~/Views/ChuanBiXetXu/NoiDung/_ThongTinNoiDung.cshtml", viewModel);
        }

        public ActionResult GetXetXuNoiDungTheoXetXuID(int id)
        {
            var anSession = GetAnSessionInfo();
            ViewBag.MaNhomAn = anSession.MaNhomAn;
            var viewModel = _xetXuService.ChiTietXetXuTheoXetXuID(id);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _xetXuService.DanhSachNgayXetXu(viewModel.HoSoVuAnID, anSession.GiaiDoanId, viewModel.XetXuGroup, 0);
            }

            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                if (anSession.MaNhomAn.Equals(Setting.MANHOMAN_HANHCHINH) && viewModel.XetXuGroup == XetXuGroup.MoPhienHop.GetHashCode())
                {
                    return PartialView("~/Views/ChuanBiXetXu/NoiDung/_MoPhienHopHanhChinhPhucThamThongTin.cshtml", viewModel);
                }
                return PartialView("~/Views/ChuanBiXetXu/NoiDung/_ThongTinNoiDungPhucTham.cshtml", viewModel);
            }
            return PartialView("~/Views/ChuanBiXetXu/NoiDung/_ThongTinNoiDung.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditXetXuNoiDung(int id, int xetXuGroup, int loaiKetQuaXetXu)
        {            
            var anSession = GetAnSessionInfo();
            var viewModel = _xetXuService.ChiTietXetXuTheoHoSoVuAnID(id, anSession.GiaiDoanId, xetXuGroup);

            if (viewModel == null)
            {
                var chiTietHoSo = _nhandonService.ChiTietHoSoVuAnTheoId(id);
                var toaAn = _settingDataService.GetToaAnTheoToaAnID(anSession.ToaAnId);
                viewModel = new XetXuModel
                {
                    HoSoVuAnID = id,
                    NgayRaQuyetDinh = DateTime.Now,
                    ThoiGianMoPhienToa = DateTime.Now.Date.Add(new TimeSpan(7, 30, 0)),
                    DiaDiemMoPhienToa = toaAn.TenToaAn,
                    ThamPhan = chiTietHoSo.ThamPhan,
                    ThamPhan1 = chiTietHoSo.ThamPhan1,
                    ThamPhan2 = chiTietHoSo.ThamPhan2,
                    ThamPhanKhac = chiTietHoSo.ThamPhanKhac,
                    HoiThamNhanDan = chiTietHoSo.HoiThamNhanDan,
                    HoiThamNhanDan2 = chiTietHoSo.HoiThamNhanDan2,
                    HoiThamNhanDan3 = chiTietHoSo.HoiThamNhanDan3,
                    ThuKy = chiTietHoSo.ThuKy,
                    KiemSatVien = chiTietHoSo.KiemSatVien,
                    HoiDong = chiTietHoSo.HoiDong,
                    ThuTuc = chiTietHoSo.ThuLyTheoThuTuc,
                    XetXuGroup = xetXuGroup,
                    LoaiKetQuaXetXu = loaiKetQuaXetXu
                    
                };
                _xetXuService.AddDanhSachDuKhuyetTheoHoSoVuAnIdToModel(viewModel);
                //ViewBag.GioMoPhienToaDDL = _settingDataService.SettingDataItemXML("Hour", "7");
                //ViewBag.PhutMoPhienToaDDL = _settingDataService.SettingDataItemXML("Minute", "30");
            }

            InitThuTucTheoNhomAn(anSession.MaNhomAn, anSession.GiaiDoanId);
            //ViewBag.loaiQuanHe = _settingDataService.GetLoaiQuanHe(id);
            ViewBag.ddlVuAnDuocXetXu = _settingDataService.SettingDataItem("DS_SOTHAM_VUANDUOCXETXU", null);

            ViewBag.ddlThamPhan = _settingDataService.DanhSachNhanVienTheoChucDanh("Thẩm Phán", anSession.ToaAnId, "");
            ViewBag.ddlHoiThamNhanDan = _settingDataService.DanhSachNhanVienTheoChucDanh("Hội Thẩm Nhân Dân", anSession.ToaAnId, "");
            ViewBag.ddlThuKy = _settingDataService.DanhSachNhanVienTheoChucDanh("Thư Ký", anSession.ToaAnId, "");
            
            var listKiemSatVien = _settingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_KIEMSATVIEN, anSession.ToaAnId, "").ToList();
            listKiemSatVien.Add(new SelectListItem() { Text = Setting.VALUE_KIEMSATVIEN_KHONG, Value = Setting.VALUE_KIEMSATVIEN_KHONG });
            ViewBag.ddlKiemSatVien = new SelectList(listKiemSatVien, "Value", "Text");

            ViewBag.listThamPhanDuKhuyet = _xetXuService.DanhSachNhanVien("Thẩm Phán", anSession.ToaAnId, viewModel.ThamPhanDuKhuyet);
            ViewBag.listHoiThamNhanDanDuKhuyet = _xetXuService.DanhSachNhanVien("Hội Thẩm Nhân Dân", anSession.ToaAnId, viewModel.HoiThamNhanDanDuKhuyet);
            ViewBag.listThuKyDuKhuyet = _xetXuService.DanhSachNhanVien("Thư Ký", anSession.ToaAnId, viewModel.ThuKyDuKhuyet);
            ViewBag.listKiemSatVienDuKhuyet = _xetXuService.DanhSachNhanVien("Kiểm Sát Viên", anSession.ToaAnId, viewModel.KiemSatVienDuKhuyet);

            ViewBag.GioMoPhienToaDDL = _settingDataService.SettingDataItemXML("Hour", viewModel.ThoiGianMoPhienToa.Hour.ToString());
            ViewBag.PhutMoPhienToaDDL = _settingDataService.SettingDataItemXML("Minute", viewModel.ThoiGianMoPhienToa.Minute.ToString());

            ViewBag.MaNhomAn = anSession.MaNhomAn;

            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                if (anSession.MaNhomAn.Equals(Setting.MANHOMAN_HANHCHINH) && xetXuGroup == XetXuGroup.MoPhienHop.GetHashCode())
                {
                    return PartialView("~/Views/ChuanBiXetXu/NoiDung/_MoPhienHopHanhChinhPhucThamModal.cshtml", viewModel);
                }
                return PartialView("~/Views/ChuanBiXetXu/NoiDung/_NoiDungPhucThamModel.cshtml", viewModel);
            }
            return PartialView("~/Views/ChuanBiXetXu/NoiDung/_NoiDungModel.cshtml", viewModel);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditXetXuNoiDung(XetXuModel viewModel, int GioMoPhienToa, int PhutMoPhienToa)
        {
            var anSession = GetAnSessionInfo();
            var loaiQuanHe = _settingDataService.GetLoaiQuanHe(viewModel.HoSoVuAnID);

            viewModel.ThoiGianMoPhienToa = viewModel.ThoiGianMoPhienToa.Date.Add(new TimeSpan(GioMoPhienToa, PhutMoPhienToa, 0));

            if (viewModel.NgayRaQuyetDinh > viewModel.ThoiGianMoPhienToa)
            {
                ModelState.AddModelError("ThoiGianMoPhienToa", ValidationMessages.VALIDATION_THOIGIANMOPHIENTOA_LONHON_RAQUYETDINH);
            }

            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                ModelState.Remove("HoiThamNhanDan");
                ModelState.Remove("HoiThamNhanDan2");
                ModelState.Remove("HoiThamNhanDan3");
                ModelState.Remove("ThamPhanKhac");
                if (anSession.MaNhomAn == Setting.MANHOMAN_APDUNG_BPXLHC)
                {
                    ModelState.Remove("ThamPhan1");
                    ModelState.Remove("ThamPhan2");
                }
                if (anSession.MaNhomAn.Equals(Setting.MANHOMAN_HANHCHINH))
                {
                    if (viewModel.ThuTuc.Equals(Setting.THUTUC_RUTGON))
                    {
                        ModelState.Remove("ThamPhan1");
                        ModelState.Remove("ThamPhan2");
                    }
                }
            }
            else
            {
                //if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                //{
                //    ModelState.Remove("HoiThamNhanDan");
                //    ModelState.Remove("HoiThamNhanDan2");
                //    ModelState.Remove("HoiThamNhanDan3");
                //}
                if (viewModel.HoiDong == HoiDong.HoiDong3.GetHashCode() || viewModel.HoiDong == 0)
                {
                    ModelState.Remove("ThamPhanKhac");
                    ModelState.Remove("HoiThamNhanDan3");
                }                
                ModelState.Remove("ThamPhan1");
                ModelState.Remove("ThamPhan2");
            }


            if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU || anSession.MaNhomAn.Equals(Setting.MANHOMAN_APDUNG_BPXLHC) || viewModel.LoaiKetQuaXetXu == LoaiKetQuaXetXu.QuyetDinh.GetHashCode())
            {
                ModelState.Remove("ThuTuc");
                ModelState.Remove("VuAnDuocXetXu");
                ModelState.Remove("HoiThamNhanDan");
                ModelState.Remove("HoiThamNhanDan2");
                ModelState.Remove("HoiThamNhanDan3");
            }

            if (ModelState.IsValid)
            {
                ResponseResult result = _xetXuService.ThemXetXu(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    _nhandonService.CapNhatBiCanBiCaoHinhSu(viewModel.HoSoVuAnID);
                    Success(result.ResponseMessage);
                    return RedirectToAction("GetXetXuNoiDungTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID, xetXuGroup = viewModel.XetXuGroup });
                }
            }
            
            InitThuTucTheoNhomAn(anSession.MaNhomAn, anSession.GiaiDoanId);
            ViewBag.ddlVuAnDuocXetXu = _settingDataService.SettingDataItem("DS_SOTHAM_VUANDUOCXETXU", null);

            ViewBag.ddlThamPhan = _settingDataService.DanhSachNhanVienTheoChucDanh("Thẩm Phán", anSession.ToaAnId, "");
            ViewBag.ddlHoiThamNhanDan = _settingDataService.DanhSachNhanVienTheoChucDanh("Hội Thẩm Nhân Dân", anSession.ToaAnId, "");
            ViewBag.ddlThuKy = _settingDataService.DanhSachNhanVienTheoChucDanh("Thư Ký", anSession.ToaAnId, "");

            var listKiemSatVien = _settingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_KIEMSATVIEN, anSession.ToaAnId, "").ToList();
            listKiemSatVien.Add(new SelectListItem() { Text = Setting.VALUE_KIEMSATVIEN_KHONG, Value = Setting.VALUE_KIEMSATVIEN_KHONG });
            ViewBag.ddlKiemSatVien = new SelectList(listKiemSatVien, "Value", "Text");

            ViewBag.listThamPhanDuKhuyet = _xetXuService.DanhSachNhanVien("Thẩm Phán", anSession.ToaAnId, viewModel.ThamPhanDuKhuyet);
            ViewBag.listHoiThamNhanDanDuKhuyet = _xetXuService.DanhSachNhanVien("Hội Thẩm Nhân Dân", anSession.ToaAnId, viewModel.HoiThamNhanDanDuKhuyet);
            ViewBag.listThuKyDuKhuyet = _xetXuService.DanhSachNhanVien("Thư Ký", anSession.ToaAnId, viewModel.ThuKyDuKhuyet);
            ViewBag.listKiemSatVienDuKhuyet = _xetXuService.DanhSachNhanVien("Kiểm Sát Viên", anSession.ToaAnId, viewModel.KiemSatVienDuKhuyet);

            ViewBag.GioMoPhienToaDDL = _settingDataService.SettingDataItemXML("Hour", viewModel.ThoiGianMoPhienToa.Hour.ToString());
            ViewBag.PhutMoPhienToaDDL = _settingDataService.SettingDataItemXML("Minute", viewModel.ThoiGianMoPhienToa.Minute.ToString());

            ViewBag.MaNhomAn = anSession.MaNhomAn;

            ViewBag.openValidTab = null;
            if (!ModelState.IsValidField("ThamPhamChuToaPhienToa") || !ModelState.IsValidField("ThamPhanKhac") || !ModelState.IsValidField("ThamPhan1") || !ModelState.IsValidField("ThamPhan2"))
                ViewBag.openValidTab = "ThamPham";
            else if (!ModelState.IsValidField("HoiThamNhanDan") || !ModelState.IsValidField("HoiThamNhanDan2") || !ModelState.IsValidField("HoiThamNhanDan3"))
                ViewBag.openValidTab = "HoiThamNhanDan";
            else if (!ModelState.IsValidField("ThuKy"))
                ViewBag.openValidTab = "ThuKy";
            else if (!ModelState.IsValidField("KiemSatVien"))
                ViewBag.openValidTab = "KiemSatVien";

            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                if (anSession.MaNhomAn.Equals(Setting.MANHOMAN_HANHCHINH) && viewModel.XetXuGroup == XetXuGroup.MoPhienHop.GetHashCode())
                {
                    return PartialView("~/Views/ChuanBiXetXu/NoiDung/_MoPhienHopHanhChinhPhucThamModal.cshtml", viewModel);
                }
                return PartialView("~/Views/ChuanBiXetXu/NoiDung/_NoiDungPhucThamModel.cshtml", viewModel);
            }
            return PartialView("~/Views/ChuanBiXetXu/NoiDung/_NoiDungModel.cshtml", viewModel);
        }

        private void InitThuTucTheoNhomAn(string nhomAn, int giaiDoan)
        {
            if (nhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                {
                    ViewBag.ddlThuTuc = _settingDataService.SettingDataItem("HC_PHUCTHAM_THULYTHEOTHUTUC", "");
                }
                else
                {
                    ViewBag.ddlThuTuc = _settingDataService.SettingDataItem("HC_SOTHAM_THULYTHEOTHUTUC", "");
                }
            }
            else
            {
                ViewBag.ddlThuTuc = _settingDataService.SettingDataItem("DS_SOTHAM_THULYTHEOTHUTUC", "");
            }            
        }
        #endregion

        #region TrieuTap
        public ActionResult GetXetXuTrieuTapTheoHoSoVuAnID(int id)
        {
            var viewModel = _xetXuService.ChiTietTrieuTapTheoHoSoVuAnID(id, GetAnSessionInfo().GiaiDoanId);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _xetXuService.DanhSachNgayTrieuTap(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, 0);
                ViewBag.listDuongSu = _xetXuService.DanhSachTrieuTapDuongSu(viewModel.TrieuTapID);
            }

            return PartialView("~/Views/ChuanBiXetXu/TrieuTap/_ThongTinTrieuTap.cshtml", viewModel);
        }

        public ActionResult GetXetXuTrieuTapTheoTrieuTapID(int id)
        {
            var viewModel = _xetXuService.ChiTietTrieuTapTheoTrieuTapID(id);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _xetXuService.DanhSachNgayTrieuTap(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, 0);
                ViewBag.listDuongSu = _xetXuService.DanhSachTrieuTapDuongSu(viewModel.TrieuTapID);
            }

            return PartialView("~/Views/ChuanBiXetXu/TrieuTap/_ThongTinTrieuTap.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditXetXuTrieuTap(int id)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _xetXuService.ChiTietTrieuTapTheoHoSoVuAnID(id, anSession.GiaiDoanId);

            if (viewModel == null)
            {
                viewModel = new TrieuTapModel();
                viewModel.HoSoVuAnID = id;
                viewModel.ThoiGianMoPhienToa = DateTime.Now.Date.Add(new TimeSpan(7, 30, 0));
                //ViewBag.GioMoPhienToaDDL = _settingDataService.SettingDataItemXML("Hour", "7");
                //ViewBag.PhutMoPhienToaDDL = _settingDataService.SettingDataItemXML("Minute", "30");
                var toaAn = _settingDataService.GetToaAnTheoToaAnID(anSession.ToaAnId);
                viewModel.DiaDiemMoPhienToa = toaAn.TenToaAn;
                viewModel.LanThu = 0;
            }
            ViewBag.ddlNguoiKy = _settingDataService.DanhSachNhanVienTheoChucDanh("Thẩm Phán", anSession.ToaAnId, null);
            ViewBag.listDuongSu = _xetXuService.DanhSachDuongSu(viewModel.HoSoVuAnID, viewModel.DuongSuID);
            ViewBag.ddlLanThu = _xetXuService.SelectListLanThu(viewModel.LanThu);
            ViewBag.GioMoPhienToaDDL = _settingDataService.SettingDataItemXML("Hour", viewModel.ThoiGianMoPhienToa.Hour.ToString());
            ViewBag.PhutMoPhienToaDDL = _settingDataService.SettingDataItemXML("Minute", viewModel.ThoiGianMoPhienToa.Minute.ToString());
            return PartialView("~/Views/ChuanBiXetXu/TrieuTap/_TrieuTapModel.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditXetXuTrieuTap(TrieuTapModel viewModel, int GioMoPhienToa, int PhutMoPhienToa)
        {
            var anSession = GetAnSessionInfo();
            viewModel.ThoiGianMoPhienToa = viewModel.ThoiGianMoPhienToa.Date + new TimeSpan(GioMoPhienToa, PhutMoPhienToa, 0);
            if (viewModel.DuongSuID == null)
            {
                ModelState.AddModelError("DuongSuID", string.Format(ValidationMessages.VALIDATION_KIEMTRARONG, ViewText.LABEL_DUONGSU));
            }

            if (ModelState.IsValid)
            {
                ResponseResult result = _xetXuService.ThemTrieuTap(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    Success(result.ResponseMessage);
                    return RedirectToAction("GetXetXuTrieuTapTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }
            ViewBag.ddlLanThu = _xetXuService.SelectListLanThu(viewModel.LanThu);
            ViewBag.ddlNguoiKy = _settingDataService.DanhSachNhanVienTheoChucDanh("Thẩm Phán", anSession.ToaAnId, null);
            ViewBag.listDuongSu = _xetXuService.DanhSachDuongSu(viewModel.HoSoVuAnID, viewModel.DuongSuID);

            ViewBag.GioMoPhienToaDDL = _settingDataService.SettingDataItemXML("Hour", viewModel.ThoiGianMoPhienToa.Hour.ToString());
            ViewBag.PhutMoPhienToaDDL = _settingDataService.SettingDataItemXML("Minute", viewModel.ThoiGianMoPhienToa.Minute.ToString());

            return PartialView("~/Views/ChuanBiXetXu/TrieuTap/_TrieuTapModel.cshtml", viewModel);
        }
        #endregion
    }
}