using Biz.Lib.TACM.KetQuaXetXu.Model;
using Biz.TACM.Infrastructure.Attributes;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Authentication;
using Biz.Lib.TACM.NhanDon.Model;
using Biz.TACM.Enums;
using Biz.Lib.TACM.Resources.Resources;

using ResponseResult = Biz.Lib.TACM.KetQuaXetXu.Model.ResponseResult;
using Biz.TACM.Models.ViewModel.Shared;
using System.Text.RegularExpressions;
using Biz.Lib.Helpers;

namespace Biz.TACM.Controllers
{
    [WorkFlow(WorkFlow.KetQuaXetXu)]
    public class KetQuaXetXuController : BizController
    {
        private readonly IKetQuaXetXuService _ketQuaXetXuService;
        private INhanDonService _nhandonService;
        private INhanDonService NhanDonService { get { return _nhandonService ?? (_nhandonService = new NhanDonService()); } }
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        public KetQuaXetXuController(IKetQuaXetXuService ketQuaXetXuService)
        {
            _ketQuaXetXuService = ketQuaXetXuService;
        }

        // GET: KetQuaXetXu
        [CustomAuthorize]
        public ActionResult Index(int id)
        {
            var anSession = GetAnSessionInfo();
            var chiTietHoSo = NhanDonService.ChiTietHoSoVuAnTheoId(id);

            if (anSession.GiaiDoanId == 0)
                anSession.GiaiDoanId = chiTietHoSo.GiaiDoan;
            UpdateAnSessionInfo(idToaAn: chiTietHoSo.ToaAnID, maToaAn: anSession.MaToaAn, maNhomAn: chiTietHoSo.NhomAn, idGiaiDoan: anSession.GiaiDoanId, idCongDoan: CongDoan.KetQuaXetXu.GetHashCode(), idVuAn: chiTietHoSo.HoSoVuAnID);

            string qh = SettingDataService.GetLoaiQuanHe(id);
            ViewBag.QuanHe = qh;
            var viewModel = NhanDonService.ChiTietHoSoVuAn(id);
            if (viewModel==null || viewModel.HoSoVuAnID == 0)
            {
                viewModel = new HoSoVuAnModel();
                viewModel.HoSoVuAnID = id;
                viewModel.CongDoanHoSo = GetAnSessionInfo().CongDoanId;
                viewModel.GiaiDoan = anSession.GiaiDoanId;
            }
            ViewBag.ActiveCongDoan = viewModel.CongDoanHoSo;
            ViewBag.ddlTrangThai = NhanDonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo, anSession.GiaiDoanId);

            var giaiDoanHoSo = _nhandonService.ChiTietHoSoVuAn(id).GiaiDoan;
            ViewBag.RoleGiaiDoan = giaiDoanHoSo == anSession.GiaiDoanId ? 1 : -1;

            //reload danh sach hosovuan is in role
            Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);

            return View(viewModel);
        }

        #region QuyetDinh
        public ActionResult GetKetQuaXetXuQuyetDinhTheoHoSoVuAnID(int id)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _ketQuaXetXuService.ChiTietQuyetDinhTheoHoSoVuAnID(id, anSession.GiaiDoanId);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _ketQuaXetXuService.DanhSachNgayQuyetDinh(viewModel.HoSoVuAnID, anSession.GiaiDoanId, 0);
            }

            return PartialView("~/Views/KetQuaXetXu/QuyetDinh/_ThongTinQuyetDinh.cshtml", viewModel);
        }

        public ActionResult GetKetQuaXetXuQuyetDinhTheoQuyetDinhID(int id)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _ketQuaXetXuService.ChiTietQuyetDinhTheoQuyetDinhID(id);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _ketQuaXetXuService.DanhSachNgayQuyetDinh(viewModel.HoSoVuAnID, anSession.GiaiDoanId, 0);
            }
            
            return PartialView("~/Views/KetQuaXetXu/QuyetDinh/_ThongTinQuyetDinh.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditKetQuaXetXuQuyetDinh(int id)
        {
            var anSession = GetAnSessionInfo();
            var chiTietHoSo = NhanDonService.ChiTietHoSoVuAnTheoId(id);
            var viewModel = _ketQuaXetXuService.ChiTietQuyetDinhTheoHoSoVuAnID(id, anSession.GiaiDoanId);

            //ViewBag.SoQuyetDinh = viewModel != null ? viewModel.SoQuyetDinh : _ketQuaXetXuService.SoQuyetDinhMax(anSession.ToaAnId, chiTietHoSo.NhomAn, anSession.GiaiDoanId).ToString();

            if (viewModel==null || viewModel.HoSoVuAnID == 0)
            {
                viewModel = new QuyetDinhModel();
                viewModel.HoSoVuAnID = id;
                viewModel.NgayRaQuyetDinh = DateTime.Now;
                //viewModel.HieuLuc = DateTime.Now;
                viewModel.LoaiQuanHe = chiTietHoSo.LoaiQuanHe;
                viewModel.QuanHePhapLuat = chiTietHoSo.QuanHePhapLuat;
                viewModel.SoQuyetDinh = _ketQuaXetXuService.SoQuyetDinhMax(id, DateTime.Now).ToString();
            }
            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                viewModel.LoaiQuanHe = Setting.LOAIQUANHE_TRANHCHAP;
                var listLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null).ToList().Where(x => x.Value == "Tranh chấp");
                ViewBag.ddlLoaiQuanHe = new SelectList(listLoaiQuanHe, "Value", "Text");
            }
            else
            {
                ViewBag.ddlLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null);
            }
            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
                {
                    ViewBag.ddlKetQuaGiaiQuyet = SettingDataService.SettingDataItem("HC_PHUCTHAM_KETQUAGIAIQUYET", null);
                }
                else
                {
                    ViewBag.ddlKetQuaGiaiQuyet = SettingDataService.SettingDataItem("DS_PHUCTHAM_KETQUAGIAIQUYET", null);
                }               
            }
            var listKiemSatVien = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_KIEMSATVIEN, anSession.ToaAnId, "").ToList();
            listKiemSatVien.Add(new SelectListItem() { Text = Setting.VALUE_KIEMSATVIEN_VANGMAT, Value = Setting.VALUE_KIEMSATVIEN_VANGMAT });
            InitQuanHePhapLuatTheoNhomAnVaLoaiQuanHe(anSession.MaNhomAn, viewModel.LoaiQuanHe);
            ViewBag.ddlLoaiQuyetDinh = InitLoaiQuyetDinh(anSession.MaNhomAn, anSession.GiaiDoanId, "Tranh chấp");
            ViewBag.ddlLoaiQuyetDinhVuViec = InitLoaiQuyetDinh(anSession.MaNhomAn, anSession.GiaiDoanId, "Yêu cầu");
            ViewBag.xemxetQD = new SelectList(XMLUtils.BindData("XemXetQuyetDinh"), "text", "text");
            ViewBag.ddlBienPhapXLHC = SettingDataService.SettingDataItem("AD_SOTHAM_BIENPHAPXULYHANHCHINH", null);
            ViewBag.ddlThoiHanApDung = SettingDataService.SettingDataItem("AD_SOTHAM_THOIHANAPDUNG", null);
            ViewBag.ddlThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THAMPHAN, anSession.ToaAnId, "");
            ViewBag.ddlHoiThamNhanDan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_HOITHAMNHANDAN, anSession.ToaAnId, "");
            ViewBag.ddlThuKy = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THUKY, anSession.ToaAnId, "");
            ViewBag.ddlKiemSatVien = new SelectList(listKiemSatVien, "Value", "Text");
            //ViewBag.ddlLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null);
            //ViewBag.ddlHieuLuc = _ketQuaXetXuService.SelectListHieuLuc(null);
            ViewBag.giaidoan = anSession.GiaiDoanId;
            return PartialView("~/Views/KetQuaXetXu/QuyetDinh/_QuyetDinhModel.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditKetQuaXetXuQuyetDinh(QuyetDinhModel viewModel, string SoQuyetDinhLienTuc)
        {
            var anSession = GetAnSessionInfo();

            //if (SettingDataService.ParseCacLoaiSoToInt(SoQuyetDinhLienTuc) != SettingDataService.ParseCacLoaiSoToInt(viewModel.SoQuyetDinh))
            //{
            //    ModelState.AddModelError("SoQuyetDinh", "Số quyết định phải là " + SettingDataService.ParseCacLoaiSoToInt(SoQuyetDinhLienTuc) + "[XXX] X là ký tự.");
            //}
            //else
            //{
                //kiem tra trung so
                QuyetDinhModel model = _ketQuaXetXuService.ChiTietQuyetDinhTheoSoQuyetDinh(viewModel.SoQuyetDinh, viewModel.HoSoVuAnID, viewModel.NgayRaQuyetDinh);
                if (model != null && model.HoSoVuAnID != viewModel.HoSoVuAnID)
                {
                    ModelState.AddModelError("SoQuyetDinh", NotifyMessage.WARNING_TRUNG_SOQUYETDINH);
                }
            //}

            if (anSession.MaNhomAn == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                ModelState.Remove("LoaiQuyetDinh");
                ModelState.Remove("LoaiQuanHe");
                ModelState.Remove("QuanHePhapLuat");
                ModelState.Remove("ThamPhan");
                ModelState.Remove("ThamPhan1");
                ModelState.Remove("ThamPhan2");
                ModelState.Remove("ThamPhanKhac");
                ModelState.Remove("HoiThamNhanDan");
                ModelState.Remove("HoiThamNhanDan2");
                ModelState.Remove("HoiThamNhanDan3");
                ModelState.Remove("ThuKy");
                ModelState.Remove("KiemSatVien");
            }
            else if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                ModelState.Remove("LoaiQuanHe");
                ModelState.Remove("QuanHePhapLuat");
                ModelState.Remove("BienPhapXuLyHanhChinh");
                ModelState.Remove("ThoiHanApDung");               
                ModelState.Remove("LyDo");
                ModelState.Remove("ThamPhan");
                ModelState.Remove("ThamPhan1");
                ModelState.Remove("ThamPhan2");
                ModelState.Remove("ThamPhanKhac");
                ModelState.Remove("HoiThamNhanDan");
                ModelState.Remove("HoiThamNhanDan2");
                ModelState.Remove("HoiThamNhanDan3");
                ModelState.Remove("ThuKy");
                ModelState.Remove("KiemSatVien");
            }
            else
            {
                ModelState.Remove("BienPhapXuLyHanhChinh");
                ModelState.Remove("ThoiHanApDung");
                ModelState.Remove("LyDo");
            }
            if (anSession.GiaiDoanId == GiaiDoan.SoTham.GetHashCode() || anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                ModelState.Remove("KetQuaGiaiQuyet");
                ModelState.Remove("ThamPhan1");
                ModelState.Remove("ThamPhan2");
                if (viewModel.HoiDong == HoiDong.HoiDong3.GetHashCode() || viewModel.HoiDong == 0)
                {
                    ModelState.Remove("ThamPhanKhac");
                    ModelState.Remove("HoiThamNhanDan3");
                }

            }
            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode() || viewModel.LoaiQuanHe== Setting.LOAIQUANHE_YEUCAU)
            {
                ModelState.Remove("HoiThamNhanDan");
                ModelState.Remove("HoiThamNhanDan2");
                ModelState.Remove("HoiThamNhanDan3");
                ModelState.Remove("ThamPhanKhac");

            }
            if (ModelState.IsValid)
            {
                ResponseResult result = _ketQuaXetXuService.ThemQuyetDinh(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    Success(result.ResponseMessage);
                    return RedirectToAction("GetKetQuaXetXuQuyetDinhTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }
            
            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                viewModel.LoaiQuanHe = Setting.LOAIQUANHE_TRANHCHAP;
                var listLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null).ToList().Where(x => x.Value == "Tranh chấp");
                ViewBag.ddlLoaiQuanHe = new SelectList(listLoaiQuanHe, "Value", "Text");
            }
            else
            {
                ViewBag.ddlLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null);
            }
            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
                {
                    ViewBag.ddlKetQuaGiaiQuyet = SettingDataService.SettingDataItem("HC_PHUCTHAM_KETQUAGIAIQUYET", null);
                }
                else
                {
                    ViewBag.ddlKetQuaGiaiQuyet = SettingDataService.SettingDataItem("DS_PHUCTHAM_KETQUAGIAIQUYET", null);
                }
            }
            ViewBag.xemxetQD = new SelectList(XMLUtils.BindData("XemXetQuyetDinh"), "text", "text");
            InitQuanHePhapLuatTheoNhomAnVaLoaiQuanHe(anSession.MaNhomAn, viewModel.LoaiQuanHe);
            ViewBag.ddlLoaiQuyetDinh = InitLoaiQuyetDinh(anSession.MaNhomAn, anSession.GiaiDoanId,"Tranh chấp");
            ViewBag.ddlLoaiQuyetDinhVuViec = InitLoaiQuyetDinh(anSession.MaNhomAn, anSession.GiaiDoanId,"Yêu cầu");
            ViewBag.ddlBienPhapXLHC = SettingDataService.SettingDataItem("AD_SOTHAM_BIENPHAPXULYHANHCHINH", null);
            ViewBag.ddlThoiHanApDung = SettingDataService.SettingDataItem("AD_SOTHAM_THOIHANAPDUNG", null);
            ViewBag.ddlThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THAMPHAN, anSession.ToaAnId, "");
            ViewBag.ddlHoiThamNhanDan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_HOITHAMNHANDAN, anSession.ToaAnId, "");
            ViewBag.ddlThuKy = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THUKY, anSession.ToaAnId, "");
            var listKiemSatVien = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_KIEMSATVIEN, anSession.ToaAnId, "").ToList();
            listKiemSatVien.Add(new SelectListItem() { Text = Setting.VALUE_KIEMSATVIEN_VANGMAT, Value = Setting.VALUE_KIEMSATVIEN_VANGMAT });
            ViewBag.ddlKiemSatVien = new SelectList(listKiemSatVien, "Value", "Text");
            ViewBag.giaidoan = anSession.GiaiDoanId;
            //ViewBag.ddlLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null);
            //ViewBag.ddlHieuLuc = _ketQuaXetXuService.SelectListHieuLuc(null);

            return PartialView("~/Views/KetQuaXetXu/QuyetDinh/_QuyetDinhModel.cshtml", viewModel);
        }

        public void InitQuanHePhapLuatTheoNhomAnVaLoaiQuanHe(string nhomAn, string loaiQuanHe)
        {

            if (nhomAn == Setting.MANHOMAN_DANSU)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("DS_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("DS_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = SettingDataService.SettingDataItem("DS_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = SettingDataService.SettingDataItem("DS_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_HONNHAN)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("HN_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("HN_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = SettingDataService.SettingDataItem("HN_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = SettingDataService.SettingDataItem("HN_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_KINHTE)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("KT_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("KT_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = SettingDataService.SettingDataItem("KT_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = SettingDataService.SettingDataItem("KT_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_LAODONG)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("LD_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("LD_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = SettingDataService.SettingDataItem("LD_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = SettingDataService.SettingDataItem("LD_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("HC_SOTHAM_KHIEUKIEN", null);
            }
            else
            {
                ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.listQuanHePhapLuatTranhChap = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.listQuanHePhapLuatYeuCau = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }

        private SelectList InitLoaiQuyetDinh(string maNhomAn, int giaiDoan, string quanhe)
        {
            SelectList listLoaiQuyetDinh;
            if (maNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                {
                    listLoaiQuyetDinh = SettingDataService.SettingDataItem("HC_PHUCTHAM_LOAIQUYETDINH", null);
                }
                else
                {
                    listLoaiQuyetDinh = SettingDataService.SettingDataItem("HC_SOTHAM_LOAIQUYETDINH", null);
                }
            }
            else if (maNhomAn == Setting.MANHOMAN_HINHSU)
            {
                if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                {
                    listLoaiQuyetDinh = SettingDataService.SettingDataItem("HS_PHUCTHAM_LOAIQUYETDINH", null);
                }
                else
                {
                    listLoaiQuyetDinh = SettingDataService.SettingDataItem("HS_SOTHAM_LOAIQUYETDINH", null);
                }
            }
            else
            {
                if(quanhe=="Tranh chấp")
                {
                    if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                    {
                        listLoaiQuyetDinh = SettingDataService.SettingDataItem("DS_PHUCTHAM_LOAIQUYETDINH", null);
                    }
                    else
                    {
                        listLoaiQuyetDinh = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUYETDINH", null);
                    }
                }
                else 
                {
                    if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                    {
                        listLoaiQuyetDinh = SettingDataService.SettingDataItem("DS_PHUCTHAM_LOAIQUYETDINH_VUVIEC", null);
                    }
                    else if(maNhomAn==Setting.MANHOMAN_HONNHAN)
                    {
                        listLoaiQuyetDinh = SettingDataService.SettingDataItem("HN_SOTHAM_LOAIQUYETDINH_VUVIEC", null);
                    }
                    else
                    {
                        listLoaiQuyetDinh = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUYETDINH_VUVIEC", null);
                    }
                }
            }
            return listLoaiQuyetDinh;
        }
        #endregion

        #region QuyetDinh ADBPXLHC

        public ActionResult GetKetQuaXetXuQuyetDinhADBPXLHCTheoHoSoVuAnID(int id)
        {
            var viewModel = _ketQuaXetXuService.ChiTietQuyetDinhADBPXLHCTheoHoSoVuAnID(id, GiaiDoan.PhucTham.GetHashCode());

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _ketQuaXetXuService.DanhSachNgayQuyetDinh(viewModel.HoSoVuAnID, GiaiDoan.PhucTham.GetHashCode(), 0);
                
            }

            return PartialView("~/Views/KetQuaXetXu/QuyetDinh/_ThongTinQuyetDinhADBPXLHC.cshtml", viewModel);
        }

        public ActionResult GetKetQuaXetXuQuyetDinhADBPXLHCTheoQuyetDinhID(int id)
        {
            var viewModel = _ketQuaXetXuService.ChiTietQuyetDinhADBPXLHCTheoQuyetDinhID(id);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _ketQuaXetXuService.DanhSachNgayQuyetDinh(viewModel.HoSoVuAnID, GiaiDoan.PhucTham.GetHashCode(), 0);
                
            }

            return PartialView("~/Views/KetQuaXetXu/QuyetDinh/_ThongTinQuyetDinhADBPXLHC.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditKetQuaXetXuQuyetDinhADBPXLHC(int id)
        {
            var anSession = GetAnSessionInfo();
            var chiTietHoSo = NhanDonService.ChiTietHoSoVuAnTheoId(id);
            var viewModel = _ketQuaXetXuService.ChiTietQuyetDinhADBPXLHCTheoHoSoVuAnID(id, GiaiDoan.PhucTham.GetHashCode());

            //ViewBag.SoQuyetDinh = viewModel != null ? viewModel.SoQuyetDinh : _ketQuaXetXuService.SoQuyetDinhMax(anSession.ToaAnId, anSession.MaNhomAn, anSession.GiaiDoanId).ToString();

            if (viewModel==null || viewModel.HoSoVuAnID == 0)
            {
                viewModel = new QuyetDinhADBPXLHCModel()
                {
                    HoSoVuAnID = id,
                    NgayRaQuyetDinh = DateTime.Now,
                    ThamPhan = chiTietHoSo.ThamPhan,
                    ThuKy = chiTietHoSo.ThuKy,
                    KiemSatVien = chiTietHoSo.KiemSatVien,
                    SoQuyetDinh = _ketQuaXetXuService.SoQuyetDinhMax(id, DateTime.Now).ToString()
                };
            }
            
            ViewBag.ddlThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THAMPHAN, anSession.ToaAnId, "");
            ViewBag.ddlThuKy = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THUKY, anSession.ToaAnId, "");
            ViewBag.ddlKiemSatVien = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_KIEMSATVIEN, anSession.ToaAnId, "");
            ViewBag.ddlKetQuaGiaiQuyet = SettingDataService.SettingDataItem("AD_PHUCTHAM_KETQUAGIAIQUYET", null);

            return PartialView("~/Views/KetQuaXetXu/QuyetDinh/_QuyetDinhADBPXLHCModel.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditKetQuaXetXuQuyetDinhADBPXLHC(QuyetDinhADBPXLHCModel viewModel, string SoQuyetDinhLienTuc)
        {
            var anSession = GetAnSessionInfo();

            //if (SettingDataService.ParseCacLoaiSoToInt(SoQuyetDinhLienTuc) != SettingDataService.ParseCacLoaiSoToInt(viewModel.SoQuyetDinh))
            //{
            //    ModelState.AddModelError("SoQuyetDinh", "Số quyết định phải là " + SettingDataService.ParseCacLoaiSoToInt(SoQuyetDinhLienTuc) + "[XXX] X là ký tự.");
            //}
            //else
            //{
                //kiem tra trung so
                QuyetDinhModel model = _ketQuaXetXuService.ChiTietQuyetDinhTheoSoQuyetDinh(viewModel.SoQuyetDinh, viewModel.HoSoVuAnID, viewModel.NgayRaQuyetDinh);
                if (model != null && model.HoSoVuAnID != viewModel.HoSoVuAnID)
                {
                    ModelState.AddModelError("SoQuyetDinh", NotifyMessage.WARNING_TRUNG_SOQUYETDINH);
                }
            //}

            if (ModelState.IsValid)
            {
                ResponseResult result = _ketQuaXetXuService.ThemQuyetDinhADBPXLHC(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    return RedirectToAction("GetKetQuaXetXuQuyetDinhADBPXLHCTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }
            ViewBag.ddlThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THAMPHAN, anSession.ToaAnId, "");
            ViewBag.ddlThuKy = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THUKY, anSession.ToaAnId, "");
            ViewBag.ddlKiemSatVien = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_KIEMSATVIEN, anSession.ToaAnId, "");
            ViewBag.ddlKetQuaGiaiQuyet = SettingDataService.SettingDataItem("ADBPXLHC_PHUCTHAM_KETQUAGIAIQUYET", null);

            return PartialView("~/Views/KetQuaXetXu/QuyetDinh/_QuyetDinhADBPXLHCModel.cshtml", viewModel);
        }

        #endregion

        #region BanAn
        public ActionResult GetKetQuaXetXuBanAnTheoHoSoVuAnID(int id)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var viewModel = _ketQuaXetXuService.ChiTietBanAnTheoHoSoVuAnID(id, giaiDoan);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _ketQuaXetXuService.DanhSachNgayBanAn(viewModel.HoSoVuAnID, giaiDoan, 0);
            }

            return PartialView("~/Views/KetQuaXetXu/BanAn/_ThongTinBanAn.cshtml", viewModel);
        }

        public ActionResult GetKetQuaXetXuBanAnTheoBanAnID(int id)
        {
            var viewModel = _ketQuaXetXuService.ChiTietBanAnTheoBanAnID(id);

            if (viewModel != null)
            {
                ViewBag.ddlNgayTao = _ketQuaXetXuService.DanhSachNgayBanAn(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, 0);
            }

            return PartialView("~/Views/KetQuaXetXu/BanAn/_ThongTinBanAn.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditKetQuaXetXuBanAn(int id)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _ketQuaXetXuService.ChiTietBanAnTheoHoSoVuAnID(id, anSession.GiaiDoanId);

            //ViewBag.SoBanAn = viewModel != null ? viewModel.SoBanAn : _ketQuaXetXuService.SoBanAnMax(anSession.ToaAnId, anSession.MaNhomAn, anSession.GiaiDoanId).ToString();

            if (viewModel==null || viewModel.HoSoVuAnID == 0)
            {
                var chiTietHoSo = NhanDonService.ChiTietHoSoVuAnTheoId(id);
                viewModel = new BanAnModel
                {
                    HoSoVuAnID = id,
                    NgayThangNamBanAn = DateTime.Now,
                    NgayMoPhienToa = DateTime.Now,
                    NgayTuyenAn = DateTime.Now,
                    HieuLuc = null,
                    QuanHePhapLuat = chiTietHoSo.QuanHePhapLuat,
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
                    SoBanAn = _ketQuaXetXuService.SoBanAnMax(id, DateTime.Now).ToString()
                };
                //viewModel.NgayThangNamBanAn = DateTime.Now;
            }
            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                viewModel.HieuLuc = viewModel.NgayTuyenAn;
            }

            var listKiemSatVien = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_KIEMSATVIEN, anSession.ToaAnId, "").ToList();
            listKiemSatVien.Add(new SelectListItem() { Text = Setting.VALUE_KIEMSATVIEN_VANGMAT, Value = Setting.VALUE_KIEMSATVIEN_VANGMAT });

            InitQuanHePhapLuatTheoNhomAn(anSession.MaNhomAn);
            ViewBag.giaiDoan = anSession.GiaiDoanId;
            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                ViewBag.ddlKetQuaXetXu = SettingDataService.SettingDataItem("HS_PHUCTHAM_KETQUAXETXU", null);
            }
            else if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.ddlKetQuaXetXu = SettingDataService.SettingDataItem("HC_PHUCTHAM_KETQUAXETXU_BANAN", null);
            }
            else
            {
                ViewBag.ddlKetQuaXetXu = SettingDataService.SettingDataItem("DS_PHUCTHAM_KETQUAXETXU_BANAN", null);
            }
            ViewBag.ddlXetXu = SettingDataService.SettingDataItem("DS_SOTHAM_HINHTHUCXETXU", null);

            ViewBag.ddlThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THAMPHAN, anSession.ToaAnId, "");
            ViewBag.ddlHoiThamNhanDan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_HOITHAMNHANDAN, anSession.ToaAnId, "");
            ViewBag.ddlThuKy = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THUKY, anSession.ToaAnId, "");
            ViewBag.ddlKiemSatVien = new SelectList(listKiemSatVien, "Value", "Text");

            return PartialView("~/Views/KetQuaXetXu/BanAn/_BanAnModel.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditKetQuaXetXuBanAn(BanAnModel viewModel, string SoBanAnLienTuc)
        {
            var anSession = GetAnSessionInfo();
            var giaiDoan = anSession.GiaiDoanId;
            var loaiQuanHe = SettingDataService.GetLoaiQuanHe(viewModel.HoSoVuAnID);

            //if (SettingDataService.ParseCacLoaiSoToInt(SoBanAnLienTuc) != SettingDataService.ParseCacLoaiSoToInt(viewModel.SoBanAn))
            //{
            //    ModelState.AddModelError("SoBanAn", "Số bản án phải là " + SettingDataService.ParseCacLoaiSoToInt(SoBanAnLienTuc) + "[XXX] X là ký tự.");
            //}
            //else
            //{
                //kiem tra trung so
                BanAnModel model = _ketQuaXetXuService.ChiTietBanAnTheoSoBanAn(viewModel.SoBanAn, viewModel.HoSoVuAnID, viewModel.NgayTuyenAn);
                if (model != null && model.HoSoVuAnID != viewModel.HoSoVuAnID)
                {
                    ModelState.AddModelError("SoBanAn", NotifyMessage.WARNING_TRUNG_SOBANAN);
                }
            //}

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                if (String.IsNullOrEmpty(viewModel.QuanHePhapLuat))
                {
                    ModelState.Remove("QuanHePhapLuat");
                    ModelState.AddModelError("QuanHePhapLuat", string.Format(ValidationMessages.VALIDATION_KIEMTRARONG, ViewText.LABEL_KHIEUKIEN));
                }
            }
            else if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                ModelState.Remove("QuanHePhapLuat");
            }
            if (anSession.MaNhomAn != Setting.MANHOMAN_HINHSU)
            {
                ModelState.Remove("NgayThangNamBanAn");
            }
            if (giaiDoan == GiaiDoan.SoTham.GetHashCode())
            {
                ModelState.Remove("KetQuaXetXu");
                ModelState.Remove("ThamPhan1");
                ModelState.Remove("ThamPhan2");
                if (viewModel.HoiDong == HoiDong.HoiDong3.GetHashCode() || viewModel.HoiDong == 0)
                {
                    ModelState.Remove("ThamPhanKhac");
                    ModelState.Remove("HoiThamNhanDan3");
                }

            }
            else if (giaiDoan == GiaiDoan.PhucTham.GetHashCode() || loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
            {
                ModelState.Remove("HoiThamNhanDan");
                ModelState.Remove("HoiThamNhanDan2");
                ModelState.Remove("HoiThamNhanDan3");
                ModelState.Remove("ThamPhanKhac");

            }
            if (ModelState.IsValid)
            {
                ResponseResult result = _ketQuaXetXuService.ThemBanAn(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    if (giaiDoan == GiaiDoan.PhucTham.GetHashCode() && viewModel.KetQuaXetXu.ToLower().Contains("hủy bản án sơ thẩm"))
                    {
                        _ketQuaXetXuService.CapNhatHuyBanAnSoTham(viewModel.HoSoVuAnID);
                    }
                    Success(result.ResponseMessage);
                    return RedirectToAction("GetKetQuaXetXuBanAnTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }

            var listKiemSatVien = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_KIEMSATVIEN, anSession.ToaAnId, "").ToList();
            listKiemSatVien.Add(new SelectListItem() { Text = Setting.VALUE_KIEMSATVIEN_VANGMAT, Value = Setting.VALUE_KIEMSATVIEN_VANGMAT });

            InitQuanHePhapLuatTheoNhomAn(anSession.MaNhomAn);
            ViewBag.giaiDoan = anSession.GiaiDoanId;
            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                ViewBag.ddlKetQuaXetXu = SettingDataService.SettingDataItem("HS_PHUCTHAM_KETQUAXETXU", null);
            }
            else
            {
                ViewBag.ddlKetQuaXetXu = SettingDataService.SettingDataItem("DS_PHUCTHAM_KETQUAXETXU_BANAN", null);
            }
            ViewBag.ddlXetXu = SettingDataService.SettingDataItem("DS_SOTHAM_HINHTHUCXETXU", null);

            ViewBag.ddlThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THAMPHAN, anSession.ToaAnId, "");
            ViewBag.ddlHoiThamNhanDan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_HOITHAMNHANDAN, anSession.ToaAnId, "");
            ViewBag.ddlThuKy = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THUKY, anSession.ToaAnId, "");
            ViewBag.ddlKiemSatVien = new SelectList(listKiemSatVien, "Value", "Text");

            ViewBag.openValidTab = null;
            if (!ModelState.IsValidField("ThamPham") || !ModelState.IsValidField("ThamPham") || !ModelState.IsValidField("HoiThamNhanDan") || !ModelState.IsValidField("HoiThamNhanDan2") || !ModelState.IsValidField("HoiThamNhanDan3") || !ModelState.IsValidField("ThuKy"))
                ViewBag.openValidTab = "HoiDongXetXu";

            return PartialView("~/Views/KetQuaXetXu/BanAn/_BanAnModel.cshtml", viewModel);
        }

        public void InitQuanHePhapLuatTheoNhomAn(string nhomAn)
        {
            if (nhomAn == Setting.MANHOMAN_DANSU)
            {
                ViewBag.ddlQuanHePhapLuat = SettingDataService.SettingDataItem("DS_SOTHAM_QHPL_TRANHCHAP", null);
            }
            else if (nhomAn == Setting.MANHOMAN_HONNHAN)
            {
                ViewBag.ddlQuanHePhapLuat = SettingDataService.SettingDataItem("HN_SOTHAM_QHPL_TRANHCHAP", null);
            }
            else if (nhomAn == Setting.MANHOMAN_LAODONG)
            {
                ViewBag.ddlQuanHePhapLuat = SettingDataService.SettingDataItem("LD_SOTHAM_QHPL_TRANHCHAP", null);
            }
            else if (nhomAn == Setting.MANHOMAN_KINHTE)
            {
                ViewBag.ddlQuanHePhapLuat = SettingDataService.SettingDataItem("KT_SOTHAM_QHPL_TRANHCHAP", null);
            }
            else if (nhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.ddlQuanHePhapLuat = SettingDataService.SettingDataItem("HC_SOTHAM_KHIEUKIEN", null);
            }
            else
            {
                ViewBag.ddlQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }

        #region Toi Danh
        public ActionResult DanhSachKetQuaXetXu(int hoSoVuAnId)
        {
            var viewModel = _ketQuaXetXuService.DanhSachKetQuaXetXu(hoSoVuAnId);

            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChiTietToiDanh(int toiDanhId)
        {
            var viewModel = _ketQuaXetXuService.ChiTietToiDanhTheoID(toiDanhId);
            return PartialView("BanAn/ToiDanh/_ThongTinToiDanh", viewModel);
        }

        [CustomAuthorize]
        public ActionResult TaoToiDanh(int hoSoVuAnId)
        {
            var viewModel = new ToiDanhModel();
            var danhSachBiCao = NhanDonService.GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_BICAO);
            var listTemp = danhSachBiCao.Select(
                x => new SelectListItem
                {
                    Text = x.HoVaTen,
                    Value = x.DuongSuID.ToString()
                }
            );
            var listBiCao = new SelectList(listTemp, "Value", "Text", null);
            var toidanhtruyto = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
            foreach (var item in toidanhtruyto)
            {
                item.DieuLuatApDung = item.Dieu + " " + item.DieuLuatApDung;
            }
            ViewBag.ddlToiDanh = new SelectList(toidanhtruyto, "DieuLuatApDung", "ToiDanh");
            ViewBag.ddlDieuLuatApDung = new SelectList(toidanhtruyto.OrderBy(x => x.Dieu), "DieuLuatApDung", "DieuLuatApDung");
            viewModel.HoSoVuAnID = hoSoVuAnId;
            ViewBag.ddlBiCao = listBiCao;
            ViewBag.ddlKetQuaXetXu = SettingDataService.SettingDataItem("HS_SOTHAM_KETQUAXETXU", null);
            return PartialView("BanAn/ToiDanh/_TaoToiDanh", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult TaoToiDanh(ToiDanhModel model)
        {
            try
            {
                ResponseResult result = _ketQuaXetXuService.TaoToiDanh(model);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                if (result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(result.ResponseMessage));
                }
                return Json(JsonResponseViewModel.CreateFail("Tạo tội danh không thành công"));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [CustomAuthorize]
        public ActionResult SuaToiDanh(int toiDanhId)
        {
            var viewModel = _ketQuaXetXuService.ChiTietToiDanhTheoID(toiDanhId);
            var danhSachBiCao = NhanDonService.GetDanhSachDuongSu(viewModel.HoSoVuAnID, null, Setting.HS_TUCACHTOTUNG_BICAO);
            var listTemp = danhSachBiCao.Select(
                x => new SelectListItem
                {
                    Text = x.HoVaTen,
                    Value = x.DuongSuID.ToString()
                }
            );
            var listBiCao = new SelectList(listTemp, "Value", "Text", null);
            var toidanhtruyto = NhanDonService.GetDanhSachToiDanhTruyTo(viewModel.HoSoVuAnID);
            foreach (var item in toidanhtruyto)
            {
                item.DieuLuatApDung = item.Dieu + " " + item.DieuLuatApDung;
            }
            ViewBag.ddlToiDanh = new SelectList(toidanhtruyto, "DieuLuatApDung", "ToiDanh");
            ViewBag.ddlDieuLuatApDung = new SelectList(toidanhtruyto.OrderBy(x => x.Dieu), "DieuLuatApDung", "DieuLuatApDung");
            ViewBag.ddlBiCao = listBiCao;
            ViewBag.ddlKetQuaXetXu = SettingDataService.SettingDataItem("HS_SOTHAM_KETQUAXETXU", null);           
            return PartialView("BanAn/ToiDanh/_SuaToiDanh", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaToiDanh(ToiDanhModel model)
        {
            try
            {
                ResponseResult result = _ketQuaXetXuService.SuaToiDanh(model);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                if (result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(result.ResponseMessage));
                }
                return Json(JsonResponseViewModel.CreateFail("Sửa tội danh không thành công"));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [CustomAuthorize]
        public ActionResult GetXoaToiDanh(int toiDanhId)
        {
            return PartialView("BanAn/ToiDanh/_XoaToiDanh", toiDanhId);
        }

        [HttpPost]
        public ActionResult XoaToiDanh(int toiDanhId)
        {
            try
            {
                ResponseResult result = _ketQuaXetXuService.XoaToiDanh(toiDanhId);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGXOA_DUONGSU));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        #endregion

        #endregion

        [HttpPost]
        public ActionResult UploadFiles()
        {
            string[] allowFileType = { ".pdf", ".doc", ".docx" };

            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (!allowFileType.Contains(Path.GetExtension(fname).ToLower()))
                    {
                        return Json(new { status = "fail", msg = Setting.FILEHOTRO });
                    }

                    if (file.ContentLength / 1024f / 1024f > 5f)
                    {
                        return Json(new { status = "fail", msg = Setting.DUNGLUONGTOIDA });
                    }

                    fname = string.Concat(
                        Path.GetFileNameWithoutExtension(fname),
                        "_",
                        DateTime.Now.ToString("yyyyMMddHHmmss"),
                        Path.GetExtension(fname)
                    );

                    // Get the complete folder path and store the file inside it.  
                    file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fname));

                    // Returns message that successfully uploaded  
                    return Json(new { status = "success", fileName = fname });
                }
                catch (Exception ex)
                {
                    return Json(new { status = "fail", msg = Setting.THONGBAOLOI + ex.Message });
                }
            }
            else
            {
                return Json(new { status = "fail", msg = Setting.THONGBAO_KHONGFILE });
            }
        }
    }
}