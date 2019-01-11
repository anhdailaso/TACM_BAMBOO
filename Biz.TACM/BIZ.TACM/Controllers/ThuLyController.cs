using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.ThuLy;
using Biz.Lib.Authentication;
using Biz.Lib.TACM.NhanDon.Model;
using Biz.TACM.Enums;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.Lib.TACM.ThuLy.Model;
using ResponseResult = Biz.Lib.TACM.ThuLy.Model.ResponseResult;

namespace Biz.TACM.Controllers
{
    public class ThuLyController : BizController
    {
        private readonly ISettingDataService _settingDataService;
        private readonly IThuLyService _thulyService;
        private readonly INhanDonService _nhandonService;

        public ThuLyController(IThuLyService thuLyService, ISettingDataService settingDataService, INhanDonService nhanDonService, NhanDonController nhanDonController)
        {
            _thulyService = thuLyService;
            _settingDataService = settingDataService;
            _nhandonService = nhanDonService;
        }

        [CustomAuthorize]
        public ActionResult Index(int id)
        {
            var anSession = GetAnSessionInfo();
            var chiTietHoSo = _nhandonService.ChiTietHoSoVuAnTheoId(id);
            if (anSession.GiaiDoanId == 0)
                anSession.GiaiDoanId = chiTietHoSo.GiaiDoan;
            UpdateAnSessionInfo(idToaAn: chiTietHoSo.ToaAnID, maToaAn: anSession.MaToaAn, maNhomAn: chiTietHoSo.NhomAn, idGiaiDoan: anSession.GiaiDoanId, idCongDoan: CongDoan.ThuLy.GetHashCode(), idVuAn: id);

            var viewModel = _thulyService.ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(id, anSession.GiaiDoanId);

            if (viewModel != null)
            {
                ViewBag.listNgayTao = _thulyService.GetDanhSachNgaySuaDoiHoSoVuAnThuLy(id, anSession.GiaiDoanId, 0);
            }
            else
            {
                //var modelSoTham = _thulyService.ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(id, GiaiDoan.SoTham.GetHashCode());
                viewModel = new HoSoVuAnThuLyViewModel();
                viewModel.GiaiDoan = anSession.GiaiDoanId;
                viewModel.SoHoSo = chiTietHoSo.SoHoSo;
                viewModel.MaHoSo = chiTietHoSo.MaHoSo;
                viewModel.CongDoanHoSo = chiTietHoSo.CongDoanHoSo;
                ViewBag.listNgayTao = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            ViewBag.ActiveCongDoan = chiTietHoSo.CongDoanHoSo;
            ViewBag.ddlTrangThai = _nhandonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo, anSession.GiaiDoanId);

            var giaiDoanHoSo = chiTietHoSo.GiaiDoan;
            ViewBag.RoleGiaiDoan = giaiDoanHoSo == anSession.GiaiDoanId ? 1 : -1;

            //reload danh sach hosovuan is in role
            Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);

            return View(viewModel);
        }

        #region ChiTietHoSoThuLy
        public ActionResult ChiTietHoSo(int hoSoVuAnId)
        {
            var chiTietHoSo = _nhandonService.ChiTietHoSoVuAnTheoId(hoSoVuAnId);
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var viewModel = _thulyService.ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(hoSoVuAnId, giaiDoan);
            ViewBag.listNgayTao = _thulyService.GetDanhSachNgaySuaDoiHoSoVuAnThuLy(hoSoVuAnId, giaiDoan, 0);
            ViewBag.ActiveCongDoan = chiTietHoSo.CongDoanHoSo;
            if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
            {
                return PartialView("_ThongTinChiTietHoSoPhucTham", viewModel);
            }
            return PartialView("_ThongTinChiTietHoSo", viewModel);
        }

        public ActionResult ChiTietHoSoTheoId(int id, int hoSoVuAnId) //hoSoVuAnLogId
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var viewModel = new HoSoVuAnThuLyViewModel();
            if (id == 0)
            {
                viewModel = _thulyService.ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
            }
            else
            {
                viewModel = _thulyService.ChiTietHoSoVuAnThuLyTheoId(id);
            }
            ViewBag.listNgayTao = _thulyService.GetDanhSachNgaySuaDoiHoSoVuAnThuLy(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, id);

            if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
            {
                return PartialView("_ThongTinChiTietHoSoPhucTham", viewModel);
            }
            return PartialView("_ThongTinChiTietHoSo", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditChiTietHoSo(int id, int hoSoVuAnId) //id = hoSoVuAnLogId
        {
            var anSession = GetAnSessionInfo();
            var giaiDoan = anSession.GiaiDoanId;
            var viewModel = new HoSoVuAnThuLyViewModel();
            if (id == 0)
            {
                if (giaiDoan == GiaiDoan.SoTham.GetHashCode())
                {
                    //hoso moi chua co thay doi o cong doan thu ly
                    viewModel = _thulyService.ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(hoSoVuAnId, giaiDoan);
                }
                else
                {
                    //copy du lieu sang hoso phuc tham moi
                    var chiTietHoSo = _nhandonService.ChiTietHoSoVuAnTheoId(hoSoVuAnId);
                    viewModel.HoSoVuAnID = chiTietHoSo.HoSoVuAnID;
                    viewModel.LoaiQuanHe = chiTietHoSo.LoaiQuanHe;
                    viewModel.QuanHePhapLuat = chiTietHoSo.QuanHePhapLuat;
                }
            }
            else
            {
                //truong hop da chinh sua o cong doan thu ly
                viewModel = _thulyService.ChiTietHoSoVuAnThuLyTheoId(id);
            }
            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.listHinhThucGoiDon = _settingDataService.SettingDataItem("HC_SOTHAM_HINHTHUCGOIDON", "");
                viewModel.LoaiQuanHe = Setting.LOAIQUANHE_TRANHCHAP;

                var listLoaiQuanHe = _settingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", "").ToList().Where(x => x.Value == Setting.LOAIQUANHE_TRANHCHAP);
                ViewBag.listLoaiQuanHe = new SelectList(listLoaiQuanHe, "Value", "Text");
            }
            else
            {
                ViewBag.listHinhThucGoiDon = _settingDataService.SettingDataItem("DINHNGHIACHUNG_HINHTHUCGOIDON", "");
                ViewBag.listLoaiQuanHe = _settingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", "");
            }
            InitQuanHePhapLuatTheoNhomAn(anSession.MaNhomAn, viewModel.LoaiQuanHe);
            ViewBag.listLoaiDon = _settingDataService.SettingDataItem("DINHNGHIACHUNG_LOAIDON", "");
            ViewBag.listYeuToNuocNgoai = _settingDataService.SettingDataItem("DINHNGHIACHUNG_YEUTONUOCNGOAI", "");
            ViewBag.listNguoiKyXacNhanDaNhanDon = _settingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu(Setting.CHUCNANG_KYXACNHAN_NHANDON, anSession.ToaAnId, null);

            if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
            {
                return PartialView("_EditChiTietHoSoPhucTham", viewModel);
            }
            return PartialView("_EditChiTietHoSo", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditChiTietHoSo(HoSoVuAnThuLyViewModel viewModel)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            try
            {
                if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                {
                    ModelState["NgayNopDonTaiToaAn"].Errors.Clear();
                    ModelState["HinhThucGoiDon"].Errors.Clear();
                    ModelState["LoaiDon"].Errors.Clear();
                    ModelState["YeuToNuocNgoai"].Errors.Clear();
                    ModelState["NguoiKyXacNhanDaNhanDon"].Errors.Clear();
                }
                ResponseResult result = _thulyService.SuaDoiHoSoVuAnThuLy(viewModel, giaiDoan);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_THONGTIN_HOSO));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        private void InitQuanHePhapLuatTheoNhomAn(string nhomAn, string loaiQuanHe)
        {

            if (nhomAn == Setting.MANHOMAN_DANSU)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("DS_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("DS_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = _settingDataService.SettingDataItem("DS_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = _settingDataService.SettingDataItem("DS_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_HONNHAN)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("HN_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("HN_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = _settingDataService.SettingDataItem("HN_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = _settingDataService.SettingDataItem("HN_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_KINHTE)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("KT_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("KT_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = _settingDataService.SettingDataItem("KT_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = _settingDataService.SettingDataItem("KT_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_LAODONG)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("LD_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("LD_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = _settingDataService.SettingDataItem("LD_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = _settingDataService.SettingDataItem("LD_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.listQuanHePhapLuat = _settingDataService.SettingDataItem("HC_SOTHAM_KHIEUKIEN", null);
            }
            else
            {
                ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }
        #endregion

        #region AnPhiLePhi
        public ActionResult AnPhi(int hoSoVuAnId)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var listNgayTao = _thulyService.GetDanhSachNgaySuaDoiAnPhi(hoSoVuAnId, giaiDoan, CongDoan.ThuLy.GetHashCode(), 0);
            if (listNgayTao.Any())
            {
                var anPhiId = Protect.ToInt32(listNgayTao.First().Value);
                var viewModel = _thulyService.ChiTietAnPhiTheoId(hoSoVuAnId, anPhiId, giaiDoan, CongDoan.ThuLy.GetHashCode());
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("AnPhi/_AnPhi", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return PartialView("AnPhi/_AnPhi");
        }

        public ActionResult ChiTietAnPhiTheoId(int hoSoVuAnId, int anPhiId)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var viewModel = _thulyService.ChiTietAnPhiTheoId(hoSoVuAnId, anPhiId, giaiDoan, CongDoan.ThuLy.GetHashCode());
            if (viewModel != null)
            {
                ViewBag.listNgayTao = _thulyService.GetDanhSachNgaySuaDoiAnPhi(hoSoVuAnId, giaiDoan, CongDoan.ThuLy.GetHashCode(), viewModel.AnPhiID);
                return PartialView("AnPhi/_AnPhi", viewModel);
            }
            return PartialView("AnPhi/_AnPhi");
        }

        [CustomAuthorize]
        public ActionResult AnPhiModal(int hoSoVuAnId, int anPhiId)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _thulyService.ChiTietAnPhiTheoId(hoSoVuAnId, anPhiId, anSession.GiaiDoanId, CongDoan.ThuLy.GetHashCode());
            if (viewModel == null)
            {
                if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
                {
                    var yeuCauDuNopModel = new YeuCauDuNopAnPhiViewModel()
                    {
                        TongAnPhi = Setting.HC_ANPHI_TONGANPHI,
                        PhanTramDuNop = Setting.HC_ANPHI_PHANTRAMDUNOP,
                        GiaTriDuNop = Setting.HC_ANPHI_GIATRIDUNOP
                    };
                    var ketQuaNopModel = new KetQuaNopAnPhiViewModel();
                    viewModel = new AnPhiViewModel()
                    {
                        HoSoVuAnID = hoSoVuAnId,
                        YeuCauDuNopViewModel = yeuCauDuNopModel,
                        KetQuaNopViewModel = ketQuaNopModel
                    };
                }
                else
                {
                    viewModel = new AnPhiViewModel
                    {
                        HoSoVuAnID = hoSoVuAnId,
                        YeuCauDuNopViewModel = new YeuCauDuNopAnPhiViewModel(),
                        KetQuaNopViewModel = new KetQuaNopAnPhiViewModel()
                    };
                }

            }
            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                if (string.IsNullOrEmpty(viewModel.YeuCauDuNopViewModel.GiaTriDuNop))
                {
                    viewModel.YeuCauDuNopViewModel.TongAnPhi = Setting.HC_ANPHI_TONGANPHI;
                    viewModel.YeuCauDuNopViewModel.PhanTramDuNop = Setting.HC_ANPHI_PHANTRAMDUNOP;
                    viewModel.YeuCauDuNopViewModel.GiaTriDuNop = Setting.HC_ANPHI_GIATRIDUNOP;
                }
            }
            viewModel.KetQuaNopViewModel.NhanVienNguoiNhanBienLai =
                _settingDataService.ChiTietNhanVienTheoMaNhanVien(AccountUtils.CurrentUsername());
            ViewBag.duongsuddl = new SelectList(_nhandonService.GetDanhSachDuongSu(hoSoVuAnId),"DuongSuID","HoVaTen");
            ViewBag.NhomAn = anSession.MaNhomAn;
            ViewBag.listNopAnPhi = _settingDataService.SettingDataItem("DS_SOTHAM_THULY_ANPHILEPHI", "");
            ViewBag.listCoQuanThiHanhAnThu = DanhSachCoQuanThiHanhAnThuTheoToaAn(anSession.ToaAnId);
            ViewBag.listDiaChiCoQuanThiHanhAnThu = _settingDataService.DanhSachDiaChiCoQuanThiHanhAn("DINHNGHIACHUNG_COQUANTHIHANHANTHU", "");
            ViewBag.listNguoiNhanBienLai = _settingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu(Setting.CHUCNANG_NHANBIENLAI, anSession.ToaAnId, null);

            return PartialView("AnPhi/_AnPhiModal", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaYeuCauDuNopAnPhi(AnPhiViewModel viewModel)
        {
            try
            {

                ResponseResult result = null;
                //sua yeu cau nop an phi, le phi
                if (viewModel.KetQuaNopViewModel == null)
                {
                    result = _thulyService.SuaYeuCauDuNopAnPhi(viewModel);
                    if (result != null && result.ResponseCode == 1)
                    {
                        return Json(JsonResponseViewModel.CreateSuccess());
                    }
                    return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_ANPHI_LEPHI));
                }

                //sua ket qua nop an phi, le phi
                viewModel.KetQuaNopViewModel.NguoiNhanBienLai = _settingDataService.GetNhanVienSessionInfo().MaNV;
                result = _thulyService.SuaKetQuaDuNopAnPhi(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_KETQUANOP_ANPHI_LEPHI));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SuaMienDuNopAnPhi(AnPhiViewModel viewModel)
        {
            try
            {
                ResponseResult result = _thulyService.SuaMienDuNopAnPhi(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_NOPTAMUNGANPHI_LEPHI));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        private SelectList DanhSachCoQuanThiHanhAnThuTheoToaAn(int toanAnId)
        {
            var listCoQuan = _settingDataService.SettingDataItemHaveValueText("DINHNGHIACHUNG_COQUANTHIHANHANTHU", "");

            var selectListItem = listCoQuan
                .Where(s => s.Value == toanAnId.ToString())
                .Select(s => new SelectListItem
                {
                    Text = s.Text,
                    Value = s.Text
                });

            return new SelectList(selectListItem, "Value", "Text");
        }
        #endregion

        #region ThongTinThuLy
        public ActionResult ThongTinThuLy(int hoSoVuAnId)
        {
            var anSession = GetAnSessionInfo();
            if (anSession.MaNhomAn == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                return RedirectToAction("ThongTinThuLyADBPXLHC", new { hoSoVuAnId = hoSoVuAnId });
            }

            var listNgayTao = _thulyService.GetDanhSachNgaySuaDoiThongTinThuLy(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, 0);
            if (listNgayTao.Any())
            {
                var thongTinThuLyId = Protect.ToInt32(listNgayTao.First().Value);
                var viewModel = _thulyService.ChiTietThongTinThuLyTheoId(thongTinThuLyId, hoSoVuAnId);
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("ThongTinThuLy/_ThongTinThuLy", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return PartialView("ThongTinThuLy/_ThongTinThuLy");
        }

        public ActionResult ChiTietThongTinThuLyTheoId(int id, int hoSoVuAnId) //thongtinthulyId
        {
            var anSession = GetAnSessionInfo();
            if (anSession.MaNhomAn == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                return RedirectToAction("ChiTietThongTinThuLyADBPXLHCTheoId", new { id = id, hoSoVuAnId = hoSoVuAnId });
            }
            var viewModel = _thulyService.ChiTietThongTinThuLyTheoId(id, hoSoVuAnId);
            if (viewModel != null)
            {
                ViewBag.listNgayTao = _thulyService.GetDanhSachNgaySuaDoiThongTinThuLy(viewModel.HoSoVuAnID, anSession.GiaiDoanId, id);
                return PartialView("ThongTinThuLy/_ThongTinThuLy", viewModel);
            }
            return PartialView("ThongTinThuLy/_ThongTinThuLy");
        }      
        [CustomAuthorize]
        public ActionResult EditThongTinThuLy(int hoSoVuAnId, int thongTinThuLyId)
        {
            var anSession = GetAnSessionInfo();
            if (anSession.GiaiDoanId == GiaiDoan.SoTham.GetHashCode() && anSession.MaNhomAn != Setting.MANHOMAN_APDUNG_BPXLHC && anSession.MaNhomAn != Setting.MANHOMAN_HINHSU)
            {
                var trangThaiAnphi = _thulyService.KiemTraTinhTrangNhapThongTinAnPhi(hoSoVuAnId);
                if (trangThaiAnphi == 0)
                {
                    string warning = NotifyMessage.WARNING_NHAP_ANPHI;
                    return PartialView("ThongTinThuLy/_MessageThongBao", warning);
                }
            }

            var viewModel = _thulyService.ChiTietThongTinThuLyTheoId(thongTinThuLyId, hoSoVuAnId);

            //ViewBag.SoThuLy = viewModel != null ? viewModel.SoThuLy : _thulyService.SoThuLyMax(anSession.ToaAnId, anSession.MaNhomAn, anSession.GiaiDoanId).ToString();

            if (viewModel == null)
            {
                var chiTietHoSo = _nhandonService.ChiTietHoSoVuAnTheoId(hoSoVuAnId);
                var khangCao = _thulyService.ChiTietNoiDungKhangCaoTheoHoSoVuAnId(hoSoVuAnId);
                viewModel = new ThongTinThuLyViewModel
                {
                    HoSoVuAnID = hoSoVuAnId,
                    ThuLyTheoThuTuc = chiTietHoSo.ThuLyTheoThuTuc,
                    LoaiQuanHe = chiTietHoSo.LoaiQuanHe,
                    QuanHePhapLuat = chiTietHoSo.QuanHePhapLuat,
                    NoiDungYeuCau = chiTietHoSo.NoiDungDon,
                    NoiDungKhangCao = khangCao.NoiDungKhangCao,
                    SoThuLy = _thulyService.SoThuLyMax(hoSoVuAnId, DateTime.Now).ToString()
                };
            }

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                viewModel.LoaiQuanHe = Setting.LOAIQUANHE_TRANHCHAP;

                var listLoaiQuanHe = _settingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", "").ToList().Where(x => x.Value == Setting.LOAIQUANHE_TRANHCHAP);
                ViewBag.listLoaiQuanHe = new SelectList(listLoaiQuanHe, "Value", "Text");
            }
            else
            {
                ViewBag.listLoaiQuanHe = _settingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", "");
            }

            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {

                if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
                {
                    ViewBag.listTruongHopThuLy = _settingDataService.SettingDataItem("HS_PHUCTHAM_TRUONGHOPTHULY", "");
                    ViewBag.listThoiHanGiaiQuyet = _settingDataService.SettingDataItem("HS_PHUCTHAM_THOIHANGIAIQUYET", "");
                }
                else
                {
                    ViewBag.listTruongHopThuLy = _settingDataService.SettingDataItem("HS_SOTHAM_TRUONGHOPTHULY", "");
                    ViewBag.listThoiHanGiaiQuyet = _settingDataService.SettingDataItem("HS_SOTHAM_THOIHANGIAIQUYET", "");
                }
            }
            else
            {
                InitQuanHePhapLuatTheoNhomAn(anSession.MaNhomAn, viewModel.LoaiQuanHe);
                InitThuLyThuTucTheoNhomAn(anSession.MaNhomAn, anSession.GiaiDoanId);
                InitTruongHopThuLyTheoGiaiDoan(anSession.GiaiDoanId);

                ViewBag.listQuyetDinh = _settingDataService.SettingDataItem("AD_PHUCTHAM_QUYETDINHBIKHIEUNAI_KIENNGHI_KHANGCAO", "");
            }

            return PartialView("ThongTinThuLy/_ThongTinThuLyModal", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditThongTinThuLy(ThongTinThuLyViewModel viewModel)
        {
            var anSession = GetAnSessionInfo();

            try
            {
                ResponseResult result = null;
                if (anSession.MaNhomAn == Setting.MANHOMAN_APDUNG_BPXLHC)
                {
                    result = _thulyService.SuaDoiThongTinThuLyADBPXLHC(viewModel);
                }
                else
                {
                    result = _thulyService.SuaDoiThongTinThuLy(viewModel);
                }

                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess(string.Format(NotifyMessage.CAPNHAT_THANHCONG, ViewText.TITLE_THONGTIN_THULY)));
                }
                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.CAPNHAT_KHONGTHANHCONG, ViewText.TITLE_THONGTIN_THULY)));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        private void InitThuLyThuTucTheoNhomAn(string nhomAn, int giaiDoan)
        {
            if (nhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                {
                    ViewBag.listThuLyTheoThuTuc = _settingDataService.SettingDataItem("HC_PHUCTHAM_THULYTHEOTHUTUC", "");
                }
                else
                {
                    ViewBag.listThuLyTheoThuTuc = _settingDataService.SettingDataItem("HC_SOTHAM_THULYTHEOTHUTUC", "");
                }
            }
            else
            {
                ViewBag.listThuLyTheoThuTuc = _settingDataService.SettingDataItem("DS_SOTHAM_THULYTHEOTHUTUC", "");
            }
        }

        private void InitTruongHopThuLyTheoGiaiDoan(int giaiDoan)
        {
            if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
            {
                ViewBag.listTruongHopThuLy = _settingDataService.SettingDataItem("DS_PHUCTHAM_TRUONGHOPTHULY", "");
            }
            else
            {
                ViewBag.listTruongHopThuLy = _settingDataService.SettingDataItem("DS_SOTHAM_TRUONGHOPTHULY", "");
            }
        }

        public ActionResult CheckSoThuLy(int HoSoVuAnID, string SoThuLy, string SoThuLyLienTuc, string NgayThuLy)
        {
            try
            {
                ResponseResult result = null;
                //if (_settingDataService.ParseCacLoaiSoToInt(SoThuLyLienTuc) != _settingDataService.ParseCacLoaiSoToInt(SoThuLy))
                //{
                //    return Json(JsonResponseViewModel.CreateFail("Số thụ lý phải là " + _settingDataService.ParseCacLoaiSoToInt(SoThuLyLienTuc).ToString() + "[XXX] X là ký tự."), JsonRequestBehavior.AllowGet);
                //}

                result = _thulyService.CheckSoThuLy(SoThuLy, HoSoVuAnID, Convert.ToDateTime(NgayThuLy));
                
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateFail(ValidationMessages.VALIDATION_SOTHULYBITRUNG), JsonRequestBehavior.AllowGet);
                }
                return Json(JsonResponseViewModel.CreateSuccess(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region PhanCongThamPhan
        public ActionResult PhanCongThamPhan(int hoSoVuAnId)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var listNgayTao = _thulyService.GetDanhSachNgaySuaDoiPhanCongThamPhan(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, 0);
            if (listNgayTao.Any())
            {
                var thamPhanId = Protect.ToInt32(listNgayTao.First().Value);
                ViewBag.listNgayTao = listNgayTao;
                var viewModel = _thulyService.ChiTietPhanCongThamPhanTheoId(thamPhanId);
                viewModel.ThamPhanDuKhuyetViewModel = _thulyService.DanhSachThamPhanDuKhuyetTheoThamPhanId(thamPhanId);
                viewModel.HoiThamNhanDanDuKhuyetViewModel = _thulyService.DanhSachHoiThamNhanDanDuKhuyetTheoThamPhanId(thamPhanId);
                viewModel.ThuKyDuKhuyetViewModel = _thulyService.DanhSachThuKyDuKhuyetTheoThamPhanId(thamPhanId);

                if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                {
                    return PartialView("PhanCongThamPhan/_PhanCongThamPhanPhucTham", viewModel);
                }
                return PartialView("PhanCongThamPhan/_PhanCongThamPhan", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
            {
                return PartialView("PhanCongThamPhan/_PhanCongThamPhanPhucTham");
            }
            return PartialView("PhanCongThamPhan/_PhanCongThamPhan");
        }

        public ActionResult ChiTietPhanCongThamPhanTheoId(int id) //thamphanId
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var viewModel = _thulyService.ChiTietPhanCongThamPhanTheoId(id);
            if (viewModel != null)
            {
                ViewBag.listNgayTao = _thulyService.GetDanhSachNgaySuaDoiPhanCongThamPhan(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, id);
                viewModel.ThamPhanDuKhuyetViewModel = _thulyService.DanhSachThamPhanDuKhuyetTheoThamPhanId(id);
                viewModel.HoiThamNhanDanDuKhuyetViewModel = _thulyService.DanhSachHoiThamNhanDanDuKhuyetTheoThamPhanId(id);
                viewModel.ThuKyDuKhuyetViewModel = _thulyService.DanhSachThuKyDuKhuyetTheoThamPhanId(id);
            }
            if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
            {
                return PartialView("PhanCongThamPhan/_PhanCongThamPhanPhucTham", viewModel);
            }
            return PartialView("PhanCongThamPhan/_PhanCongThamPhan", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditPhanCongThamPhan(int hoSoVuAnId, int thamPhanId)
        {            
            var chiTietHoSo = _nhandonService.ChiTietHoSoVuAnTheoId(hoSoVuAnId);
            UpdateAnSessionInfo(idToaAn: chiTietHoSo.ToaAnID, maToaAn: GetAnSessionInfo().MaToaAn, maNhomAn: chiTietHoSo.NhomAn, idGiaiDoan: chiTietHoSo.GiaiDoan, idCongDoan: CongDoan.ThuLy.GetHashCode());
            UpdateAnSessionInfo(idToaAn: chiTietHoSo.ToaAnID, maToaAn: GetAnSessionInfo().MaToaAn, maNhomAn: chiTietHoSo.NhomAn, idGiaiDoan: GetAnSessionInfo().GiaiDoanId, idCongDoan: CongDoan.ThuLy.GetHashCode(), idVuAn: hoSoVuAnId);
            var anSession = GetAnSessionInfo();
            var viewModel = new PhanCongThamPhanViewModel { HoSoVuAnID = hoSoVuAnId };            
            var listThamPhanDuKhuyet = _thulyService.DanhSachThamPhanDuKhuyetTheoThamPhanId(thamPhanId);
            var listHoiThamNhanDanDuKhuyet = _thulyService.DanhSachHoiThamNhanDanDuKhuyetTheoThamPhanId(thamPhanId);
            var listThuKyDuKhuyet = _thulyService.DanhSachThuKyDuKhuyetTheoThamPhanId(thamPhanId);

            if (thamPhanId != 0)
            {
                viewModel = _thulyService.ChiTietPhanCongThamPhanTheoId(thamPhanId);
            }

            ViewBag.nhomAn = chiTietHoSo.NhomAn;
            ViewBag.loaiQuanHe = chiTietHoSo.LoaiQuanHe;
            ViewBag.thuTuc = chiTietHoSo.ThuLyTheoThuTuc;
            ViewBag.listThamPhan = _settingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THAMPHAN, anSession.ToaAnId, viewModel.ThamPhan);
            ViewBag.listTenNguoiPhancong = _settingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu("Phân Công Thẩm Phán", anSession.ToaAnId, viewModel.TenNguoiPhanCong);
            ViewBag.listHoiThamNhanDan = _settingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_HOITHAMNHANDAN, anSession.ToaAnId, viewModel.HoiThamNhanDan);
            ViewBag.listThuKy = _settingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THUKY, anSession.ToaAnId, viewModel.ThuKy);
            ViewBag.listThamPhanDuKhuyet = _thulyService.DanhSachThamPhanDuKhuyetSelected(Setting.CHUCDANH_THAMPHAN, anSession.ToaAnId, listThamPhanDuKhuyet);
            ViewBag.listHoiThamNhanDanDuKhuyet = _thulyService.DanhSachHoiThamNhanDanDuKhuyetSelected(Setting.CHUCDANH_HOITHAMNHANDAN, anSession.ToaAnId, listHoiThamNhanDanDuKhuyet);
            ViewBag.listThuKyDuKhuyet = _thulyService.DanhSachThuKyDuKhuyetSelected(Setting.CHUCDANH_THUKY, anSession.ToaAnId, listThuKyDuKhuyet);
            var listngaytao = _thulyService.GetDanhSachNgaySuaDoiGhiChuPhanCong(hoSoVuAnId, anSession.GiaiDoanId);
            if (listngaytao != null)
            {
                viewModel.GhiChu = _thulyService.ChiTietGhiChuPhanCongTheoId(int.Parse(listngaytao.First().Value)).NoiDungGhiChu;
            }
            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                return PartialView("PhanCongThamPhan/_PhanCongThamPhanPhucThamModal", viewModel);
            }
            return PartialView("PhanCongThamPhan/_PhanCongThamPhanModal", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhanCongThamPhan(PhanCongThamPhanViewModel viewModel)
        {
            try
            {
                var giaiDoan = GetAnSessionInfo().GiaiDoanId;
                viewModel.GiaiDoan = giaiDoan;
                ResponseResult result = _thulyService.SuaDoiPhanCongThamPhan(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_THONGTIN_THULY));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        #endregion

        #region NhomAn ADBPXLHC
        public ActionResult ThongTinThuLyADBPXLHC(int hoSoVuAnId)
        {
            var listNgayTao = _thulyService.GetDanhSachNgaySuaDoiThongTinThuLy(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, 0);
            if (listNgayTao.Any())
            {
                var thongTinThuLyId = Protect.ToInt32(listNgayTao.First().Value);
                var viewModel = _thulyService.ChiTietThongTinThuLyTheoId(thongTinThuLyId, hoSoVuAnId);
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("ThongTinThuLy/_ThongTinThuLyADBPXLHC", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return PartialView("ThongTinThuLy/_ThongTinThuLyADBPXLHC");
        }

        public ActionResult ChiTietThongTinThuLyADBPXLHCTheoId(int id, int hoSoVuAnId)
        {
            var viewModel = _thulyService.ChiTietThongTinThuLyTheoId(id, hoSoVuAnId);
            var giaidoan = GetAnSessionInfo().GiaiDoanId;

            if (viewModel != null)
            {
                ViewBag.listNgayTao = _thulyService.GetDanhSachNgaySuaDoiThongTinThuLy(viewModel.HoSoVuAnID, giaidoan, id);
                return PartialView("ThongTinThuLy/_ThongTinThuLyADBPXLHC", viewModel);
            }
            return PartialView("ThongTinThuLy/_ThongTinThuLyADBPXLHC");
        }

        [CustomAuthorize]
        public ActionResult EditThongTinThuLyADBPXLHC(int hoSoVuAnId, int thongTinThuLyId)
        {
            var viewModel = _thulyService.ChiTietThongTinThuLyTheoId(thongTinThuLyId, hoSoVuAnId);

            //ViewBag.SoThuLy = viewModel != null ? viewModel.SoThuLy : _thulyService.SoThuLyMax(anSession.ToaAnId, anSession.MaNhomAn, anSession.GiaiDoanId).ToString();

            if (viewModel == null)
            {
                viewModel = new ThongTinThuLyViewModel
                {
                    HoSoVuAnID = hoSoVuAnId,
                    SoThuLy = _thulyService.SoThuLyMax(hoSoVuAnId, DateTime.Now).ToString(),
            };

            }

            ViewBag.listQuyetDinh = _settingDataService.SettingDataItem("AD_PHUCTHAM_QUYETDINHBIKHIEUNAI_KIENNGHI_KHANGCAO", "");

            return PartialView("ThongTinThuLy/_ThongTinThuLyModalADBPXLHC", viewModel);
        }
        #endregion

        #region GhiChuPhanCong
        [CustomAuthorize]
        public ActionResult GhiChuPhanCong(int hoSoVuAnID)
        {
            var an = GetAnSessionInfo();
            var listngaytao = _thulyService.GetDanhSachNgaySuaDoiGhiChuPhanCong(hoSoVuAnID, an.GiaiDoanId);
            ViewBag.NgayTao = listngaytao;
            if (listngaytao!=null)
            {
                var viewModel = _thulyService.ChiTietGhiChuPhanCongTheoId(int.Parse(listngaytao.First().Value));
                return PartialView("GhiChuPhanCong/_GhiChuPhanCong",viewModel);
            }
            return PartialView("GhiChuPhanCong/_GhiChuPhanCong");
        }
        public ActionResult ChiTietGhiChuPhanCong(int ghiChuPhanCongId)
        {
            var viewModel = _thulyService.ChiTietGhiChuPhanCongTheoId(ghiChuPhanCongId);
            return PartialView("GhiChuPhanCong/_ChiTietGhiChuPhanCong", viewModel);
        }
        [HttpGet, ValidateInput(false)]
        [CustomAuthorize]
        public ActionResult ThemGhiChuPhanCong(int hoSoVuAnId, string noiDung)
        {
            var viewModel = new GhiChuPhanCongModel();
            viewModel.NoiDungGhiChu = noiDung;
            viewModel.HoSoVuAnID = hoSoVuAnId;
            if (hoSoVuAnId == 0)
                viewModel.HoSoVuAnID = GetAnSessionInfo().VuAnId;
            return PartialView("GhiChuPhanCong/_ThemGhiChuPhanCong",viewModel);
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemGhiChuPhanCong(GhiChuPhanCongModel viewModel)
        {
            try
            {
                ResponseResult result = null;
                viewModel.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
                result = _thulyService.SuaDoiGhiChuPhanCong(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess(string.Format(NotifyMessage.CAPNHAT_THANHCONG,"ghi chú phân công")));
                }
                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.CAPNHAT_KHONGTHANHCONG, "ghi chú phân công")));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}