using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.ThuLy.IDataAccess;
using Biz.Lib.TACM.ThuLy.Model;
using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.ThuLy;
using System.Globalization;
using Biz.Lib.SettingData.Model;
using Biz.TACM.Enums;

namespace Biz.TACM.Services
{
    public class ThuLyService : IThuLyService
    {
        
        private readonly ISettingDataService _settingDataService;
        private readonly IThuLyDataAccess _thuLyDataAccess;
        public ThuLyService(IThuLyDataAccess thuLyDataAccess, ISettingDataService settingDataService)
        {
            _thuLyDataAccess = thuLyDataAccess;
            _settingDataService = settingDataService;
        }

        public ThuLyService()
        {
        }

        #region HoSoVuAn

        public SelectList GetDanhSachNgaySuaDoiHoSoVuAnThuLy(int hoSoVuAnId, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = _thuLyDataAccess.GetDanhSachNgaySuaDoiHoSoVuAnThuLy(hoSoVuAnId, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = $@"{x.NgayTao:dd/MM/yyyy HH:mm:ss}",
                        Value = x.HoSoVuAnLogID.ToString()
                    }
                );
                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetDanhSachNgaySuaDoiHoSoVuAn with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public HoSoVuAnThuLyViewModel ChiTietHoSoVuAnThuLyTheoId(int hoSoVuAnLogId)
        {
            try
            {
                var dbModel = _thuLyDataAccess.ChiTietHoSoVuAnThuLyTheoId(hoSoVuAnLogId);
                return new HoSoVuAnThuLyViewModel()
                {
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    HoSoVuAnLogID = dbModel.HoSoVuAnLogID,
                    MaHoSo = dbModel.MaHoSo,
                    SoHoSo = dbModel.SoHoSo,
                    NhomAn = dbModel.NhomAn,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    TrangThaiCongDoan = dbModel.TrangThaiCongDoan,
                    NgayLamDon = $"{dbModel.NgayLamDon:dd/MM/yyyy}",
                    NgayNopDonTaiToaAn = $"{dbModel.NgayNopDonTaiToaAn:dd/MM/yyyy}",
                    HinhThucGoiDon = dbModel.HinhThucGoiDon,
                    LoaiDon = dbModel.LoaiDon,
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat,
                    YeuToNuocNgoai = dbModel.YeuToNuocNgoai,
                    NguoiKyXacNhanDaNhanDon = dbModel.NguoiKyXacNhanDaNhanDon,
                    CanBoNhanDon = dbModel.CanBoNhanDon,
                    ThuLyTheoThuTuc = dbModel.ThuLyTheoThuTuc,
                    TrangThai = dbModel.TrangThai,
                    ThamPhan = dbModel.ThamPhan,
                    GiaTriDuNop = string.Format("{0:n0}", dbModel.GiaTriDuNop),
                    HanNopAnPhi = dbModel.HanNopAnPhi,
                    NgayChuyenDon = dbModel.NgayChuyenDon,
                    NgayTraDon = dbModel.NgayTraDon,
                    NgayKhieuNai = dbModel.NgayKhieuNai,
                    NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu,
                    NhanVienThamPhan = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan),
                    NhanVienCanBoNhanDon = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.CanBoNhanDon),
                    NhanVienNguoiKyXacNhanDaNhanDon = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiKyXacNhanDaNhanDon),
                    NhanVienNguoiLapDon = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao)
            };
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietHoSoVuAnThuLyTheoId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new HoSoVuAnThuLyViewModel();
            }
        }

        public HoSoVuAnThuLyViewModel ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(int hoSoVuAnId, int giaiDoan)
        {
            try
            {
                var dbModel = _thuLyDataAccess.ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(hoSoVuAnId, giaiDoan);
                if(dbModel == null)
                    return null;
                return new HoSoVuAnThuLyViewModel()
                {
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    MaHoSo = dbModel.MaHoSo,
                    SoHoSo = dbModel.SoHoSo,
                    NhomAn = dbModel.NhomAn,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    TrangThaiCongDoan = dbModel.TrangThaiCongDoan,
                    NgayLamDon = $"{dbModel.NgayLamDon:dd/MM/yyyy}",
                    NgayNopDonTaiToaAn = $"{dbModel.NgayNopDonTaiToaAn:dd/MM/yyyy}",
                    HinhThucGoiDon = dbModel.HinhThucGoiDon,
                    LoaiDon = dbModel.LoaiDon,
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat,
                    YeuToNuocNgoai = dbModel.YeuToNuocNgoai,
                    NguoiKyXacNhanDaNhanDon = dbModel.NguoiKyXacNhanDaNhanDon,
                    CanBoNhanDon = dbModel.CanBoNhanDon,
                    ThuLyTheoThuTuc = dbModel.ThuLyTheoThuTuc,
                    TrangThai = dbModel.TrangThai,
                    ThamPhan = dbModel.ThamPhan,
                    GiaTriDuNop = string.Format("{0:n0}", dbModel.GiaTriDuNop),
                    HanNopAnPhi = dbModel.HanNopAnPhi,
                    NgayChuyenDon = dbModel.NgayChuyenDon,
                    NgayTraDon = dbModel.NgayTraDon,
                    NgayKhieuNai = dbModel.NgayKhieuNai,
                    NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu,
                    NhanVienThamPhan = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan),
                    NhanVienCanBoNhanDon = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.CanBoNhanDon),
                    NhanVienNguoiKyXacNhanDaNhanDon = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiKyXacNhanDaNhanDon),
                    NhanVienNguoiLapDon = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao)
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietHoSoVuAnThuLyTheoHoSoVuAnId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new HoSoVuAnThuLyViewModel();
            }
        }

        public ResponseResult SuaDoiHoSoVuAnThuLy(HoSoVuAnThuLyViewModel viewModel, int giaiDoan)
        {
            ResponseResult result = null;
            try
            {
                var dbModel = new HoSoVuAnThuLyModel();
                if (giaiDoan == GiaiDoan.SoTham.GetHashCode())
                {
                    dbModel.HoSoVuAnID = viewModel.HoSoVuAnID;
                    dbModel.NgayLamDon = Convert.ToDateTime(viewModel.NgayLamDon);
                    dbModel.NgayNopDonTaiToaAn = Convert.ToDateTime(viewModel.NgayNopDonTaiToaAn);
                    dbModel.HinhThucGoiDon = viewModel.HinhThucGoiDon;
                    dbModel.LoaiDon = viewModel.LoaiDon;
                    dbModel.LoaiQuanHe = viewModel.LoaiQuanHe;
                    dbModel.QuanHePhapLuat = viewModel.QuanHePhapLuat;
                    dbModel.YeuToNuocNgoai = viewModel.YeuToNuocNgoai;
                    dbModel.NguoiKyXacNhanDaNhanDon = viewModel.NguoiKyXacNhanDaNhanDon;
                    dbModel.CongDoanHoSo = CongDoan.ThuLy.GetHashCode();
                    dbModel.NguoiTao = viewModel.NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV;
                }
                else
                {
                    dbModel.HoSoVuAnID = viewModel.HoSoVuAnID;
                    dbModel.NgayLamDon = Convert.ToDateTime(viewModel.NgayLamDon);
                    dbModel.LoaiQuanHe = viewModel.LoaiQuanHe;
                    dbModel.QuanHePhapLuat = viewModel.QuanHePhapLuat;
                    dbModel.CongDoanHoSo = CongDoan.ThuLy.GetHashCode();
                    dbModel.NguoiTao = viewModel.NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV;
                }

                result = _thuLyDataAccess.SuaDoiHoSoVuAnThuLy(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaDoiHoSoVuAnThuLy with error [{ex.ToString()}]", AppName.BizSecurity);
                result = null;
            }
            return result;
        }
        #endregion

        #region AnPhiLePhi

        public SelectList GetDanhSachNgaySuaDoiAnPhi(int hoSoVuAnId, int giaiDoan, int congDoan,  int selected)
        {
            try
            {
                var dbModel = _thuLyDataAccess.GetDanhSachNgaySuaDoiAnPhi(hoSoVuAnId, giaiDoan, congDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = $@"{x.NgayTao:dd/MM/yyyy HH:mm:ss}",
                        Value = x.AnPhiID.ToString()
                    }
                );
                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());

            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetDanhSachNgaySuaDoiAnPhi with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public AnPhiViewModel ChiTietAnPhiTheoId(int hoSoVuAnId, int anPhiId, int giaiDoan, int congDoan)
        {
            try
            {
                var dbModel = _thuLyDataAccess.ChiTietAnPhiTheoId(hoSoVuAnId, anPhiId, giaiDoan, congDoan);
                if (dbModel == null)
                    return null;
                var yeuCauDuNopAnPhiModel = new YeuCauDuNopAnPhiViewModel
                {
                    NopAnPhi = dbModel.NopAnPhi,
                    GiaTriTranhChap = string.Format("{0:n0}", dbModel.GiaTriTranhChap),
                    TongAnPhi = string.Format("{0:n0}", dbModel.TongAnPhi),
                    PhanTramDuNop = dbModel.PhanTramDuNop,
                    GiaTriDuNop = string.Format("{0:n0}", dbModel.GiaTriDuNop),
                    HanNopAnPhi = $"{dbModel.HanNopAnPhi:dd/MM/yyyy}",
                    CoQuanThiHanhAnThu = dbModel.CoQuanThiHanhAnThu,
                    DiaChiCoQuanThiHanhAnThu = dbModel.DiaChiCoQuanThiHanhAnThu,
                    NgayRaThongBao = string.Format("{0:dd/MM/yyyy}", dbModel.NgayRaThongBao),
                    NgayGiaoThongBao = $"{dbModel.NgayGiaoThongBao:dd/MM/yyyy}",
                    NguoiDuNop=dbModel.NguoiDuNop
                };
                var ketQuaNopAnPhiModel = new KetQuaNopAnPhiViewModel
                {
                    NgayNopAnPhi = $"{dbModel.NgayNopAnPhi:dd/MM/yyyy}",
                    SoBienLai = dbModel.SoBienLai,
                    NgayNopBienLaiChoToaAn = $"{dbModel.NgayNopBienLaiChoToaAn:dd/MM/yyyy}",
                    NguoiNhanBienLai = dbModel.NguoiNhanBienLai,
                    NhomNghiepVu = dbModel.NhomNghiepVu,
                    NhanVienNguoiNhanBienLai = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiNhanBienLai)
                };
                return new AnPhiViewModel
                {
                    AnPhiID = dbModel.AnPhiID,
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    SoAnPhi = dbModel.SoAnPhi,
                    NopAnPhi = dbModel.NopAnPhi,
                    YeuCauDuNopViewModel = yeuCauDuNopAnPhiModel,
                    KetQuaNopViewModel = ketQuaNopAnPhiModel,
                    NhomNghiepVu = dbModel.NhomNghiepVu,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    TrangThai = dbModel.TrangThai,
                    NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd/MM/yyyy"),
                    GhiChu = dbModel.GhiChu,
                    NguoiDuNop=dbModel.HoTenNguoiDuNop
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietAnPhiTheoId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new AnPhiViewModel();
            }
        }

        public ResponseResult SuaYeuCauDuNopAnPhi(AnPhiViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.GiaTriTranhChap, out decimal giaTriTranhChap);
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.TongAnPhi, out decimal tongAnPhi);
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.GiaTriDuNop, out decimal giaTriDuNop);
                float phanTramDuNop = float.Parse(viewModel.YeuCauDuNopViewModel.PhanTramDuNop);
                phanTramDuNop /= 100;

                var dbModel = new YeuCauDuNopAnPhiModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    NopAnPhi = viewModel.YeuCauDuNopViewModel.NopAnPhi,
                    GiaTriTranhChap = giaTriTranhChap,
                    TongAnPhi = tongAnPhi,
                    PhanTramDuNop = phanTramDuNop,
                    GiaTriDuNop = giaTriDuNop,
                    HanNopAnPhi = (viewModel.YeuCauDuNopViewModel.HanNopAnPhi != null ? (DateTime?)Convert.ToDateTime(viewModel.YeuCauDuNopViewModel.HanNopAnPhi) : null),
                    CoQuanThiHanhAnThu = viewModel.YeuCauDuNopViewModel.CoQuanThiHanhAnThu,
                    DiaChiCoQuanThiHanhAnThu = viewModel.YeuCauDuNopViewModel.DiaChiCoQuanThiHanhAnThu,
                    NgayRaThongBao = Convert.ToDateTime(viewModel.YeuCauDuNopViewModel.NgayRaThongBao),
                    NgayGiaoThongBao = (viewModel.YeuCauDuNopViewModel.NgayGiaoThongBao != null ? (DateTime?)Convert.ToDateTime(viewModel.YeuCauDuNopViewModel.NgayGiaoThongBao) : null),
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = 2, // ThuLy
                    NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                    GhiChu = viewModel.GhiChu,
                    NguoiDuNop=viewModel.YeuCauDuNopViewModel.NguoiDuNop
                };
                result = _thuLyDataAccess.SuaYeuCauDuNopAnPhi(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaDoiYeuCauDuNopAnPhi with error [{ex.ToString()}]", AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public ResponseResult SuaMienDuNopAnPhi(AnPhiViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                var dbModel = new MienDuNopModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    NopAnPhi = viewModel.MienDuNopViewModel.NopAnPhi,
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = 2, // ThuLy
                    NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                    GhiChu = viewModel.GhiChu
                };
                result = _thuLyDataAccess.SuaMienDuNopAnPhi(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaMienDuNopAnPhi with error [{ex.ToString()}]", AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public ResponseResult SuaKetQuaDuNopAnPhi(AnPhiViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.GiaTriTranhChap, out decimal giaTriTranhChap);
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.TongAnPhi, out decimal tongAnPhi);
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.GiaTriDuNop, out decimal giaTriDuNop);
                float phanTramDuNop = float.Parse(viewModel.YeuCauDuNopViewModel.PhanTramDuNop);
                phanTramDuNop /= 100;
                var dbModel = new AnPhiModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    NopAnPhi = viewModel.YeuCauDuNopViewModel.NopAnPhi,
                    GiaTriTranhChap = giaTriTranhChap,
                    TongAnPhi = tongAnPhi,
                    PhanTramDuNop = phanTramDuNop.ToString(CultureInfo.InvariantCulture),
                    GiaTriDuNop = giaTriDuNop,
                    HanNopAnPhi = Convert.ToDateTime(viewModel.YeuCauDuNopViewModel.HanNopAnPhi),
                    CoQuanThiHanhAnThu = viewModel.YeuCauDuNopViewModel.CoQuanThiHanhAnThu,
                    DiaChiCoQuanThiHanhAnThu = viewModel.YeuCauDuNopViewModel.DiaChiCoQuanThiHanhAnThu,
                    NgayRaThongBao = Convert.ToDateTime(viewModel.YeuCauDuNopViewModel.NgayRaThongBao),
                    NgayGiaoThongBao = Convert.ToDateTime(viewModel.YeuCauDuNopViewModel.NgayGiaoThongBao),
                    NgayNopAnPhi = Convert.ToDateTime(viewModel.KetQuaNopViewModel.NgayNopAnPhi),
                    SoBienLai = viewModel.KetQuaNopViewModel.SoBienLai,
                    NgayNopBienLaiChoToaAn = Convert.ToDateTime(viewModel.KetQuaNopViewModel.NgayNopBienLaiChoToaAn),
                    NguoiNhanBienLai = viewModel.KetQuaNopViewModel.NguoiNhanBienLai,
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = CongDoan.ThuLy.GetHashCode(), // ThuLy
                    NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                    GhiChu = viewModel.GhiChu
                };
                result = _thuLyDataAccess.SuaKetQuaDuNopAnPhi(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaKetQuaDuNopAnPhi with error [{ex.ToString()}]", AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        #endregion

        #region ThongTinThuLy
        public SelectList GetDanhSachNgaySuaDoiThongTinThuLy(int hoSoVuAnId, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = _thuLyDataAccess.GetDanhSachNgaySuaDoiThongTinThuLy(hoSoVuAnId, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = $@"{x.NgayTao:dd/MM/yyyy HH:mm:ss}",
                        Value = x.ThongTinThuLyID.ToString()
                    }
                );
                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetDanhSachNgaySuaDoiThongTinThuLy with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
                return null;
            }
        }

        public ThongTinThuLyViewModel ChiTietThongTinThuLyTheoId(int thongTinThuLyId, int hoSoVuAnId)
        {
            try
            {
                var dbModel = _thuLyDataAccess.ChiTietThongTinThuLyTheoId(thongTinThuLyId, hoSoVuAnId);
                if (dbModel == null)
                    return null;
                return new ThongTinThuLyViewModel
                {
                    ThongTinThuLyID = dbModel.ThongTinThuLyID,
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    SoThuLy = dbModel.SoThuLy,
                    TruongHopThuLy = dbModel.TruongHopThuLy,
                    ThuLyTheoThuTuc = dbModel.ThuLyTheoThuTuc,
                    NgayThuLy = string.Format("{0:dd/MM/yyyy}", dbModel.NgayThuLy),
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat,
                    ThoiHanGiaiQuyet = dbModel.ThoiHanGiaiQuyet,
                    ThoiHanGiaiQuyetTuNgay = string.Format("{0:dd/MM/yyyy}", dbModel.ThoiHanGiaiQuyetTuNgay),
                    ThoiHanGiaiQuyetDenNgay = string.Format("{0:dd/MM/yyyy}", dbModel.ThoiHanGiaiQuyetDenNgay),
                    NoiDungYeuCau = dbModel.NoiDungYeuCau,
                    NoiDungKhangCao = dbModel.NoiDungKhangCao,
                    NhomNghiepVu = dbModel.NhomNghiepVu,
                    TaiLieuChungTuKemTheo = dbModel.TaiLieuChungTuKemTheo,
                    QuyetDinh = dbModel.QuyetDinh,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    TrangThai = dbModel.TrangThai,
                    NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietThongTinThuLyTheoId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new ThongTinThuLyViewModel();
            }
        }

        public ThongTinThuLyViewModel ChiTietNoiDungKhangCaoTheoHoSoVuAnId(int hoSoVuAnId)
        {
            try
            {
                var dbModel = _thuLyDataAccess.ChiTietNoiDungKhangCaoTheoHoSoVuAnId(hoSoVuAnId);
                if (dbModel == null)
                    return new ThongTinThuLyViewModel();
                return new ThongTinThuLyViewModel
                {
                    NoiDungKhangCao = dbModel.NoiDungKhangCao
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietNoiDungKhangCaoTheoHoSoVuAnId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new ThongTinThuLyViewModel();
            }
        }

        public ThongTinThuLyViewModel ChiTietThongTinThuLyCopySangPhucTham(int hoSoVuAnId)
        {
            try
            {
                var dbModel = _thuLyDataAccess.ChiTietThongTinThuLyCopySangPhucTham(hoSoVuAnId);
                if (dbModel == null)
                    return null;
                return new ThongTinThuLyViewModel
                {
                    ThongTinThuLyID = dbModel.ThongTinThuLyID,
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    ThuLyTheoThuTuc = dbModel.ThuLyTheoThuTuc,
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietThongTinThuLyCopySangPhucTham with error [{ex.ToString()}]", AppName.BizSecurity);
                return new ThongTinThuLyViewModel();
            }
        }

        public ResponseResult SuaDoiThongTinThuLy(ThongTinThuLyViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                var dbModel = new ThongTinThuLyModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    SoThuLy = viewModel.SoThuLy,
                    TruongHopThuLy = viewModel.TruongHopThuLy,
                    ThuLyTheoThuTuc = viewModel.ThuLyTheoThuTuc,
                    NgayThuLy = Convert.ToDateTime(viewModel.NgayThuLy),
                    LoaiQuanHe = viewModel.LoaiQuanHe,
                    QuanHePhapLuat = viewModel.QuanHePhapLuat,
                    ThoiHanGiaiQuyet = viewModel.ThoiHanGiaiQuyet,
                    ThoiHanGiaiQuyetTuNgay = Convert.ToDateTime(viewModel.ThoiHanGiaiQuyetTuNgay),
                    ThoiHanGiaiQuyetDenNgay = Convert.ToDateTime(viewModel.ThoiHanGiaiQuyetDenNgay),
                    NoiDungYeuCau = viewModel.NoiDungYeuCau,
                    NoiDungKhangCao = viewModel.NoiDungKhangCao,
                    TaiLieuChungTuKemTheo = viewModel.TaiLieuChungTuKemTheo,
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    QuyetDinh = viewModel.QuyetDinh,
                    GiaiDoan = viewModel.GiaiDoan,
                    TrangThai = viewModel.TrangThai,
                    NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                    GhiChu = viewModel.GhiChu
                };

                result = _thuLyDataAccess.SuaDoiThongTinThuLy(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaDoiThongTinThuLy with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }

        public ResponseResult SuaDoiThongTinThuLyADBPXLHC(ThongTinThuLyViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                var dbModel = new ThongTinThuLyModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    SoThuLy = viewModel.SoThuLy,
                    NgayThuLy = Convert.ToDateTime(viewModel.NgayThuLy),
                    QuyetDinh = viewModel.QuyetDinh,
                    GiaiDoan = viewModel.GiaiDoan,
                    NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV
                };

                result = _thuLyDataAccess.SuaDoiThongTinThuLy(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaDoiThongTinThuLyADBPXLHC with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }

        public int KiemTraTinhTrangNhapThongTinAnPhi(int hoSoVuAnId)
        {
            int result = 1;
            try
            {
                result = _thuLyDataAccess.KiemTraTinhTrangNhapThongTinAnPhi(hoSoVuAnId);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"KiemTraTinhTrangNhapThongTinAnPhi with error [{ex.ToString()}]", AppName.BizSecurity);
            }
            return result;
        }

        public int SoThuLyMax(int hoSoVuAnId, DateTime? ngayThuLy)
        {
            int t = 1;
            try
            {
                t += _thuLyDataAccess.SoThuLyMax(hoSoVuAnId, ngayThuLy);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SoThuLyMax with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
            }
            return t;
        }

        public int SoThuLyMaxChoADBPXLHC(int ToaAnID, string NhomAn, int giaiDoan)
        {
            int t = 1;
            try
            {
                t += _thuLyDataAccess.SoThuLyMaxChoADBPXLHC(ToaAnID, NhomAn, giaiDoan);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SoThuLyMax with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
            }
            return t;
        }

        //public ResponseResult CheckSoThuLy(string SoThuLy, int HoSoVuAnID, int ToaAnID, string NhomAn, int GiaiDoan)
        public ResponseResult CheckSoThuLy(string SoThuLy, int HoSoVuAnID, DateTime NgayThuLy)
        {
            ResponseResult result = null;
            try
            {
                result = _thuLyDataAccess.CheckSoThuLy(SoThuLy, HoSoVuAnID, NgayThuLy);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"CheckSoThuLy with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
            }
            return result;
        }

        #endregion

        #region PhanCongThamPhan
        public SelectList GetDanhSachNgaySuaDoiPhanCongThamPhan(int hoSoVuAnId, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = _thuLyDataAccess.GetDanhSachNgaySuaDoiPhanCongThamPhan(hoSoVuAnId, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = $@"{x.NgayTao:dd/MM/yyyy HH:mm:ss}",
                        Value = x.ThamPhanID.ToString()
                    }
                );
                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetDanhSachNgaySuaDoiPhanCongThamPhan with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<ThamPhanDuKhuyetModel> DanhSachThamPhanDuKhuyetTheoThamPhanId(int thamPhanId)
        {
            try
            {
                var dbModel = _thuLyDataAccess.DanhSachThamPhanDuKhuyetTheoThamPhanId(thamPhanId).ToList();
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachThamPhanDuKhuyetTheoThamPhanId with error [{ex.ToString()}]",
                    AppName.BizSecurity);
                return new List<ThamPhanDuKhuyetModel>();
            }
        }

        public List<HoiThamNhanDanDuKhuyetModel> DanhSachHoiThamNhanDanDuKhuyetTheoThamPhanId(int thamPhanId)
        {
            try
            {
                var dbModel = _thuLyDataAccess.DanhSachHoiThamNhanDanDuKhuyetTheoThamPhanId(thamPhanId).ToList();
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachThamPhanDuKhuyetTheoThamPhanId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new List<HoiThamNhanDanDuKhuyetModel>();
            }
        }

        public List<ThuKyDuKhuyetModel> DanhSachThuKyDuKhuyetTheoThamPhanId(int thamPhanId)
        {
            try
            {
                var dbModel = _thuLyDataAccess.DanhSachThuKyDuKhuyetTheoThamPhanId(thamPhanId).ToList();
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachThuKyDuKhuyetTheoThamPhanId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new List<ThuKyDuKhuyetModel>();
            }
        }

        public PhanCongThamPhanViewModel ChiTietPhanCongThamPhanTheoId(int thamPhanId)
        {
            try
            {
                var dbModel = _thuLyDataAccess.ChiTietPhanCongThamPhanTheoId(thamPhanId);
                return new PhanCongThamPhanViewModel()
                {
                    ThamPhanID = dbModel.ThamPhanID,
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    NgayPhanCong = dbModel.NgayPhanCong.ToString("dd/MM/yyyy"),
                    TenNguoiPhanCong = dbModel.TenNguoiPhanCong,
                    ThamPhan = dbModel.ThamPhan,
                    ThamPhan1 = dbModel.ThamPhan1,
                    ThamPhan2 = dbModel.ThamPhan2,
                    ThamPhanKhac = dbModel.ThamPhanKhac,
                    HoiThamNhanDan = dbModel.HoiThamNhanDan,
                    HoiThamNhanDan2 = dbModel.HoiThamNhanDan2,
                    HoiThamNhanDan3 = dbModel.HoiThamNhanDan3,
                    ThuKy = dbModel.ThuKy,
                    HoiDong = dbModel.HoiDong,
                    NhomNghiepVu = dbModel.NhomNghiepVu,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    TrangThai = dbModel.TrangThai,
                    NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu,
                    NhanVienThamPhan = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan),
                    NhanVienThamPhan1 = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan1),
                    NhanVienThamPhan2 = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan2),
                    NhanVienThamPhanKhac = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhanKhac),
                    NhanVienNguoiPhanCong = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.TenNguoiPhanCong),
                    NhanVienHoiThamNhanDan = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan),
                    NhanVienHoiThamNhanDan2 = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan2),
                    NhanVienHoiThamNhanDan3 = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan3),
                    NhanVienThuKy = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThuKy)
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietPhanCongThamPhanTheoId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new PhanCongThamPhanViewModel();
            }
        }

        public ResponseResult SuaDoiPhanCongThamPhan(PhanCongThamPhanViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                var dbModel = new PhanCongThamPhanModel()
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    ThamPhan = viewModel.ThamPhan,
                    ThamPhan1 = viewModel.ThamPhan1,
                    ThamPhan2 = viewModel.ThamPhan2,
                    ThamPhanKhac = viewModel.ThamPhanKhac,
                    NgayPhanCong = Convert.ToDateTime(viewModel.NgayPhanCong),
                    TenNguoiPhanCong = viewModel.TenNguoiPhanCong,
                    HoiThamNhanDan = viewModel.HoiThamNhanDan,
                    HoiThamNhanDan2 = viewModel.HoiThamNhanDan2,
                    HoiThamNhanDan3 = viewModel.HoiThamNhanDan3,
                    HoiDong = viewModel.HoiDong,
                    ThuKy = viewModel.ThuKy,
                    ThamPhanDuKhuyet = viewModel.ThamPhanDuKhuyet,
                    HoiThamNhanDanDuKhuyet = viewModel.HoiThamNhanDanDuKhuyet,
                    ThuKyDuKhuyet = viewModel.ThuKyDuKhuyet,
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    TrangThai = viewModel.TrangThai,
                    NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                    GhiChu = viewModel.GhiChu
                };
                result = _thuLyDataAccess.SuaDoiPhanCongThamPhan(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaDoiPhanCongThamPhan with error [{ex.ToString()}]", AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public List<NhanVienModel> DanhSachThamPhanDuKhuyetSelected(string chucDanh, int toaAnId, List<ThamPhanDuKhuyetModel> selected)
        {
            try
            {
                var strNhanVien = new List<string>();
                if (selected != null)
                {
                    foreach (var nv in selected)
                    {
                        strNhanVien.Add(nv.ThamPhanDuKhuyet);
                    }
                }
                var list = _settingDataService.DanhSachNhanVienTheoChucDanhSelected(chucDanh, toaAnId, strNhanVien);
                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachThamPhanDuKhuyetSelected with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<NhanVienModel> DanhSachHoiThamNhanDanDuKhuyetSelected(string chucDanh, int toaAnId, List<HoiThamNhanDanDuKhuyetModel> selected)
        {
            try
            {
                var strNhanVien = new List<string>();
                if (selected != null)
                {
                    foreach (var nv in selected)
                    {
                        strNhanVien.Add(nv.HoiThamNhanDanDuKhuyet);
                    }
                }
                var list = _settingDataService.DanhSachNhanVienTheoChucDanhSelected(chucDanh, toaAnId, strNhanVien);
                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachHoiThamNhanDanDuKhuyetSelected with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<NhanVienModel> DanhSachThuKyDuKhuyetSelected(string chucDanh, int toaAnId, List<ThuKyDuKhuyetModel> selected)
        {
            try
            {
                var strNhanVien = new List<string>();
                if (selected != null)
                {
                    foreach (var nv in selected)
                    {
                        strNhanVien.Add(nv.ThuKyDuKhuyet);
                    }
                }
                var list = _settingDataService.DanhSachNhanVienTheoChucDanhSelected(chucDanh, toaAnId, strNhanVien);
                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachThuKyDuKhuyetSelected with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        #endregion

        #region GhiChuPhanCong

        public SelectList GetDanhSachNgaySuaDoiGhiChuPhanCong(int hoSoVuAnId, int giaiDoan)
        {
            try
            {
                var dbModel = _thuLyDataAccess.GetDanhSachNgaySuaDoiGhiChuPhanCong(hoSoVuAnId, giaiDoan);
                if (dbModel != null && dbModel.Any())
                {
                    var listNgayTao = dbModel.Select(
                        x => new SelectListItem
                        {
                            Text = $@"{x.NgayTao:dd/MM/yyyy HH:mm:ss}",
                            Value = x.GhiChuPhanCongID.ToString()
                        }
                    );
                    return new SelectList(listNgayTao, "Value", "Text");
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetDanhSachNgaySuaDoiGhiChuPhnCong with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
                return null;
            }
        }

        public GhiChuPhanCongModel ChiTietGhiChuPhanCongTheoId(int Id)
        {
            try
            {
                return  _thuLyDataAccess.ChiTietGhiChuPhanCongId(Id);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietGhiChuPhanCongTheoId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new GhiChuPhanCongModel();
            }
        }
      
        public ResponseResult SuaDoiGhiChuPhanCong(GhiChuPhanCongModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV;
                result = _thuLyDataAccess.ThemGhiChuPhanCong(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaDoiGhiChuPhanCong with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }

        #endregion
    }
}