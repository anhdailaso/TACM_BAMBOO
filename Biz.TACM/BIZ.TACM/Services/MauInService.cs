using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.MauIn.Model;
using Biz.TACM.Models.ViewModel.MauIn;
using Biz.Lib.TACM.MauIn.DataAccess;
using Biz.Lib.TACM.MauIn.IDataAccess;
using Biz.TACM.IServices;
using Biz.TACM.Enums;
using Biz.Lib.SettingData.Model;
using System.Web.Mvc;
using System.Globalization;
using Xceed.Words.NET;
using System.IO;
using System.Drawing;
using Spire.Doc;
using Spire.Doc.Fields;
using OpenXmlPowerTools;
using Biz.Lib.TACM.Resources.Resources;
using DocumentFormat.OpenXml;

namespace Biz.TACM.Services
{
    public class MauInService : IMauInService
    {
        private IMauInDataAccess _mauInDataAccess;
        private IMauInDataAccess MauInDataAccess => _mauInDataAccess ?? (_mauInDataAccess = new MauInDataAccess());

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        private INhanDonService _nhanDonService;
        private INhanDonService NhanDonService => _nhanDonService ?? (_nhanDonService = new NhanDonService());
        /// <summary>
        /// Save history when a MauIn was created
        /// </summary>
        /// <param class="LichSuInViewModel" name="viewModel"></param>
        /// <returns>ResponseResult</returns>
        public ResponseResult LuuLichSuIn(LichSuInViewModel viewModel)
        {
            viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
            var dbModel = new LichSuInModel
            {
                HoSoVuAnID = viewModel.HoSoVuAnID,
                SoMauIn = viewModel.SoMauIn,
                DuongDanFileIn = viewModel.DuongDanFileIn,
                NhomNghiepVu = viewModel.NhomNghiepVu,
                GiaiDoan = viewModel.GiaiDoan,
                CongDoanHoSo = viewModel.CongDoanHoSo,
                TrangThai = viewModel.TrangThai,
                NguoiTao = viewModel.NguoiTao,
                GhiChu = viewModel.GhiChu,
                NhomAn = viewModel.NhomAn,
            };
            var result = MauInDataAccess.LuuLichSuIn(dbModel);
            return result;
        }

        #region Chi Tiet Cac Mau In
        /// <summary>
        /// Get infomation of Mau In 24
        /// </summary>
        /// <param type="int" name="hoSoVuAnId"></param>
        /// <returns>MauInSo24ViewModel</returns>
        public MauInSo24ViewModel ChiTietMauInSo24(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.ChiTietMauInSo24(hoSoVuAnId);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuMauInSo24(hoSoVuAnId);
            if (dbModel == null)
                return null;
            else
            {
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";

                MauInSo24ViewModel mauInSo24ViewModel = new MauInSo24ViewModel()
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    MaToaAn = dbModel.MaToaAn,
                    MaHoSo = dbModel.MaHoSo,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    NgayLamDon = dbModel.NgayLamDon,
                    NgayNopDonTaiToaAn = dbModel.NgayNopDonTaiToaAn,
                    NoiDungDon = replacetag(dbModel.NoiDungDon) ?? "..............................................................................................................................",
                    HoTenNguoiKyXacNhanDaNhanDon = dbModel.HoTenNguoiKyXacNhanDaNhanDon ?? "..................",
                    ChucDanhNguoiKyXacNhan = dbModel.ChucDanhNguoiKyXacNhan ?? "..................",
                    ChucVuNguoiKyXacNhan=dbModel.ChucVuNguoiKyXacNhan ?? "..................",
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    HinhThucGoiDon = dbModel.HinhThucGoiDon,
                    DanhSachDuongSuViewModel = danhSachDuongSu.ToList()
                };

                return mauInSo24ViewModel;
            }
        }

        /// <summary>
        /// Get infomation of Mau In 30
        /// </summary>
        /// <param type="int" name="hoSoVuAnId"></param>
        /// <returns>MauInSo30ViewModel</returns>
        public MauInSo30ViewModel ChiTietMauInSo30(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.ChiTietMauInSo30(hoSoVuAnId);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuMauInSo30(hoSoVuAnId);
            if (dbModel == null)
                return null;
            else
            {
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                return new MauInSo30ViewModel
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    MaToaAn = dbModel.MaToaAn,
                    MaHoSo = dbModel.MaHoSo,
                    NhomAn = dbModel.NhomAn,
                    ThamPhan = dbModel.ThamPhan ?? "..................",
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    QuanHePhapLuat = dbModel.QuanHePhapLuat ?? "..................",
                    NoiDungYeuCau = replacetag(dbModel.NoiDungYeuCau),
                    TaiLieuChungTuKemTheo = replacetag(dbModel.TaiLieuChungTuKemTheo),
                    NgayThuLy = dbModel.NgayThuLy,
                    DanhSachDuongSuViewModel = danhSachDuongSu.ToList(),
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    ThuLyTheoThuTuc = dbModel.ThuLyTheoThuTuc ?? "",
                    TenCoQuanDeNghi = dbModel.TenCoQuanDeNghi,
                    BienPhapXuLyHanhChinh = dbModel.BienPhapXuLyHanhChinh ?? "..................",
                    ThuLyAD = AddZero(dbModel.ThuLyAD),
                    NgayNopDonTaiToaAn = dbModel.NgayNopDonTaiToaAn
                };
            }
        }

        /// <summary>
        /// Get infomation of Mau In 29
        /// </summary>
        /// <param type="int" name="hoSoVuAnId"></param>
        /// <returns>MauInSo29ViewModel</returns>
        public MauInSo29ViewModel DuLieuMauInSo29(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.DuLieuMauInSo29(hoSoVuAnId);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuMauInSo29(hoSoVuAnId);
            var nguoiDuNop = danhSachDuongSu.Where(x => x.DuongSuID == dbModel.NguoiDuNop).FirstOrDefault();
            if (dbModel == null)
                return null;
            else
            {
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                if (dbModel.GiaTriDuNop == null) dbModel.GiaTriDuNop = 0;
                return new MauInSo29ViewModel
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    MaToaAn = dbModel.MaToaAn,
                    MaHoSo = dbModel.MaHoSo,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    NgayRaThongBao = dbModel.NgayRaThongBao,
                    CoQuanThiHanhAnThu = dbModel.CoQuanThiHanhAnThu ?? ".................",
                    DiaChiCoQuanThiHanhAnThu = dbModel.DiaChiCoQuanThiHanhAnThu ?? ".................",
                    GiaTriDuNopFormatted = string.Format("{0:n0}", dbModel.GiaTriDuNop), //CultureInfo.GetCultureInfo("en-GB"), 
                    GiaTriDuNopBangChu = So_chu(double.Parse(dbModel.GiaTriDuNop.ToString())),
                    ThamPhan = dbModel.ThamPhan ?? "..................",
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    NguoiDuNop = nguoiDuNop,
                    DanhSachDuongSu = danhSachDuongSu.ToList()
                };
            }
        }

        /// <summary>
        /// Get infomation of Mau In 47
        /// </summary>
        /// <param type="int" name="hoSoVuAnId"></param>
        /// <returns>MauInSo47ViewModel</returns>
        public MauInSo47ViewModel DuLieuMauInSo47(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.DuLieuMauInSo47(hoSoVuAnId);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuMauInSo47(hoSoVuAnId);
            var danhSachThamPhanDuKhuyet = MauInDataAccess.DanhSachThamPhanDuKhuyetMauInSo47(hoSoVuAnId);
            var danhSachHoiThamNhanDanDuKhuyet = MauInDataAccess.DanhSachHoiThamNhanDanDuKhuyetMauInSo47(hoSoVuAnId);
            var danhSachKiemSatVienDuKhuyet = MauInDataAccess.DanhSachKiemSatVienDuKhuyetMauInSo47(hoSoVuAnId);
            var danhSachThuKyDuKhuyet = MauInDataAccess.DanhSachThuKyDuKhuyet(hoSoVuAnId);
            MauInSo47ViewModel mauIn47 = new MauInSo47ViewModel();

            if (dbModel == null)
                mauIn47 = null;
            else
            {
                string toi = "", luat = "";
                if (dbModel.NhomAn == "HS")
                {
                    var biCao = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAN || x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO).ToList();
                    var toiDanhTruyTo = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
                    foreach (var item in toiDanhTruyTo)
                    {
                        toi += item.ToiDanh + ", ";
                        luat += item.Dieu + " " + item.DieuLuatApDung + " - ";
                    }
                    var hinhPhat = MauInDataAccess.DanhSachBiCaoHinhPhat(hoSoVuAnId);                    
                    if (toi != "" &&  toi.LastIndexOf(", ")!=-1)
                        toi = toi.Remove(toi.LastIndexOf(","), 2);
                    if (luat != "" &&  luat .LastIndexOf(" - ")!=-1)
                        luat = luat.Remove(luat.LastIndexOf(" - "), 3);
                }
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                mauIn47 = new MauInSo47ViewModel
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    DiaChiToaAn = dbModel.DiaChiToaAn,
                    MaToaAn = dbModel.MaToaAn,
                    MaHoSo = dbModel.MaHoSo,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    NhomAn = dbModel.NhomAn,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat ?? "..................",
                    ThamPhanChuToa = dbModel.ThamPhanChuToa,
                    GioiTinhThamPhanChuToa = dbModel.GioiTinhThamPhanChuToa,
                    ThamPhanKhac = dbModel.ThamPhanKhac,
                    GioiTinhThamPhanKhac = dbModel.GioiTinhThamPhanKhac,
                    HoiThamNhanDan = dbModel.HoiThamNhanDan,
                    GioiTinhHoiThamNhanDan = dbModel.GioiTinhHoiThamNhanDan,
                    HoiThamNhanDan2 = dbModel.HoiThamNhanDan2,
                    GioiTinhHoiThamNhanDan2 = dbModel.GioiTinhHoiThamNhanDan2,
                    HoiThamNhanDan3 = dbModel.HoiThamNhanDan3,
                    GioiTinhHoiThamNhanDan3 = dbModel.GioiTinhHoiThamNhanDan3,
                    ThuKy = dbModel.ThuKy,
                    GioiTinhThuKy = dbModel.GioiTinhThuKy,
                    KiemSatVien = dbModel.KiemSatVien,
                    GioiTinhKiemSatVien = dbModel.GioiTinhKiemSatVien,
                    NgayThuLy = dbModel.NgayThuLy,
                    NgayRaQuyetDinhXetXu = dbModel.NgayRaQuyetDinhXetXu,
                    ThoiGianMoPhienToa = dbModel.ThoiGianMoPhienToa,
                    DiaDiemMoPhienToa = dbModel.DiaDiemMoPhienToa ?? "..................",
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    ThuTuc = dbModel.ThuTuc,
                    VuAnDuocXetXu = dbModel.VuAnDuocXetXu ?? "..................",
                    DanhSachDuongSu = danhSachDuongSu.ToList(),
                    HoiDong = dbModel.HoiDong,
                    TenCoQuanDeNghi = dbModel.TenCoQuanDeNghi,
                    BienPhapXuLyHanhChinh = dbModel.BienPhapXuLyHanhChinh,
                    VienKiemSatTruyTo = dbModel.VienKiemSatTruyTo,
                    VatChung = replacetag(dbModel.VatChung),
                    ThuLyAD = AddZero(dbModel.ThuLyAD),
                    NgayNopDonTaiToaAn = dbModel.NgayNopDonTaiToaAn,
                    DanhSachThamPhanDuKhuyet = danhSachThamPhanDuKhuyet.Select(ds => new MauInSo47ViewModel.ThamPhanDuKhuyetMauIn47ViewModel
                    {
                        ThamPhanDuKhuyet = ds.ThamPhanDuKhuyet,
                        GioiTinh = ds.GioiTinh
                    }).ToList(),
                    DanhSachHoiThamNhanDanDuKhuyet = danhSachHoiThamNhanDanDuKhuyet.Select(ds => new MauInSo47ViewModel.HoiThamNhanDanDuKhuyetMauInSo47ViewModel
                    {
                        HoiThamNhanDanDuKhuyet = ds.HoiThamNhanDanDuKhuyet,
                        GioiTinh = ds.GioiTinh
                    }).ToList(),
                    DanhSachKiemSatVienDuKhuyet = danhSachKiemSatVienDuKhuyet.Select(ds => new MauInSo47ViewModel.KiemSatVienDuKhuyetMauInSo47ViewModel
                    {
                        KiemSatVienDuKhuyet = ds.KiemSatVienDuKhuyet,
                        GioiTinh = ds.GioiTinh
                    }).ToList(),
                    DanhSachThuKyDuKhuyet = danhSachThuKyDuKhuyet,
                    ToiDanh=toi,
                    DieuLuat=luat
                };
            }
            return mauIn47;
        }

        public MauInSo47PTViewModel DuLieuMauInSo47PT(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.DuLieuMauInSo47PhucTham(hoSoVuAnId);
            var nguoiKhangCao = MauInDataAccess.NguoiKhangCaoMauInSo47(hoSoVuAnId);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuMauInSo47(hoSoVuAnId);
            var danhSachThamPhanDuKhuyet = MauInDataAccess.DanhSachThamPhanDuKhuyetMauInSo47(hoSoVuAnId);
            var danhSachThuKyDuKhuyet = MauInDataAccess.DanhSachThuKyDuKhuyet(hoSoVuAnId);
            var danhSachKiemSatVienDuKhuyet = MauInDataAccess.DanhSachKiemSatVienDuKhuyetMauInSo47(hoSoVuAnId);
            var biCao = new List<DuongSuModel>();
            MauInSo47PTViewModel mauIn47 = new MauInSo47PTViewModel();
            string toi = "", luat = "", hinhphat = "";
            if (dbModel == null)
                mauIn47 = null;
            else
            {
                if (dbModel.NhomAn == "HS")
                {
                    biCao = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAN || x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO).ToList();
                    var toiDanhTruyTo = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
                    foreach (var item in toiDanhTruyTo)
                    {
                        toi += item.ToiDanh + ", ";
                        luat += item.Dieu+" "+ item.DieuLuatApDung + " - ";
                    }
                    var hinhPhat = MauInDataAccess.DanhSachBiCaoHinhPhat(hoSoVuAnId);
                    foreach (var item in hinhPhat)
                    {
                        hinhphat += item.HinhPhat;
                    }
                    if (toi != "" &&  toi.LastIndexOf(", ")!=-1)
                        toi = toi.Remove(toi.LastIndexOf(","), 2);
                    if (luat != "" &&  luat .LastIndexOf(" - ")!=-1)
                        luat = luat.Remove(luat.LastIndexOf(" - "), 3);
                    if (hinhphat != "" &&  hinhphat.LastIndexOf(", ")!=-1)
                        hinhphat = hinhphat.Remove(hinhphat.LastIndexOf(", "), 2);
                }
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                if (dbModel.ToaAnSoTham == null) dbModel.ToaAnSoTham = "..................";
                mauIn47 = new MauInSo47PTViewModel
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    ToaAnSoTham = dbModel.ToaAnSoTham.Replace("TAND ", "") ?? "...............",
                    DiaChiToaAn = dbModel.DiaChiToaAn,
                    MaToaAn = dbModel.MaToaAn,
                    MaHoSo = dbModel.MaHoSo,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    NhomAn = dbModel.NhomAn,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat ?? "..................",
                    ThamPhanChuToa = dbModel.ThamPhanChuToa,
                    GioiTinhThamPhanChuToa = dbModel.GioiTinhThamPhanChuToa,
                    ThamPhan1 = dbModel.ThamPhan1,
                    GioiTinhThamPhan1 = dbModel.GioiTinhThamPhan1,
                    ThamPhan2 = dbModel.ThamPhan2,
                    GioiTinhThamPhan2 = dbModel.GioiTinhThamPhan2,
                    HoiThamNhanDan = dbModel.HoiThamNhanDan,
                    GioiTinhHoiThamNhanDan = dbModel.GioiTinhHoiThamNhanDan,
                    ThuKy = dbModel.ThuKy,
                    GioiTinhThuKy = dbModel.GioiTinhThuKy,
                    KiemSatVien = dbModel.KiemSatVien,
                    GioiTinhKiemSatVien = dbModel.GioiTinhKiemSatVien,
                    NgayThuLy = dbModel.NgayThuLy,
                    NgayRaQuyetDinhXetXu = dbModel.NgayRaQuyetDinhXetXu,
                    ThoiGianMoPhienToa = dbModel.ThoiGianMoPhienToa,
                    DiaDiemMoPhienToa = dbModel.DiaDiemMoPhienToa ?? "..................",
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    ThuTuc = dbModel.ThuTuc,
                    ToiDanh = toi,
                    DieuLuat = luat,
                    HinhPhat = hinhphat,
                    VuAnDuocXetXu = dbModel.VuAnDuocXetXu ?? "..................",
                    DanhSachDuongSu = danhSachDuongSu.ToList(),
                    NguoiKhangCao = nguoiKhangCao.ToList(),
                    VienKiemSatKhangNghi = dbModel.VienKiemSatKhangNghi,
                    HoiDong = dbModel.HoiDong,
                    VatChung = replacetag(dbModel.VatChung),
                    DanhSachBiCao = biCao,
                    DanhSachThamPhanDuKhuyet = danhSachThamPhanDuKhuyet.Select(ds => new MauInSo47PTViewModel.ThamPhanDuKhuyetMauIn47ViewModel
                    {
                        ThamPhanDuKhuyet = ds.ThamPhanDuKhuyet,
                        GioiTinh = ds.GioiTinh
                    }).ToList(),

                    DanhSachKiemSatVienDuKhuyet = danhSachKiemSatVienDuKhuyet.Select(ds => new MauInSo47PTViewModel.KiemSatVienDuKhuyetMauInSo47ViewModel
                    {
                        KiemSatVienDuKhuyet = ds.KiemSatVienDuKhuyet,
                        GioiTinh = ds.GioiTinh
                    }).ToList(),
                    DanhSachThuKyDuKhuyet = danhSachThuKyDuKhuyet,
                    SoQuyetDinh = AddZero(dbModel.SoQuyetDinh),
                    NgayRaQuyetDinh = dbModel.NgayRaQuyetDinh,
                    TenCoQuanDeNghi = dbModel.TenCoQuanDeNghi,
                    BienPhapXuLyHanhChinh=dbModel.BienPhapXuLyHanhChinh
                };
            }
            return mauIn47;
        }
        /// <summary>
        /// Get infomation of Mau In 61
        /// </summary>
        /// <param type="int" name="hoSoVuAnId"></param>
        /// <returns>MauInSo61ViewModel</returns>
        public MauInSo61ViewModel DuLieuMauInSo61(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.DuLieuMauInSo61(hoSoVuAnId);
            var danhSachNguoiKhangCao = MauInDataAccess.DanhSachNguoiKhangCaoMauInSo61(hoSoVuAnId);
            if (dbModel == null)
                return null;
            else
            {
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                if (dbModel.GiaTriDuNop == null) dbModel.GiaTriDuNop = 0;
                return new MauInSo61ViewModel
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    MaHoSo = dbModel.MaHoSo,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    NgayRaThongBao = dbModel.NgayRaThongBao,
                    CoQuanThiHanhAnThu = dbModel.CoQuanThiHanhAnThu ?? "..................",
                    DiaChiCoQuanThiHanhAnThu = dbModel.DiaChiCoQuanThiHanhAnThu ?? "..................",
                    GiaTriDuNopFormatted = string.Format("{0:n0}", dbModel.GiaTriDuNop ?? 0), //CultureInfo.GetCultureInfo("en-GB"), 
                    GiaTriDuNopBangChu = So_chu(double.Parse(dbModel.GiaTriDuNop.ToString())),
                    ThamPhan = dbModel.ThamPhan ?? "..................",
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    DanhSachNguoiKhangCao = danhSachNguoiKhangCao.ToList()
                };
            }
        }
        public MauInSo65ViewModel DuLieuMauInSo65(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.DuLieuMauInSo65(hoSoVuAnId);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId);
            var nguoikhangcao = MauInDataAccess.NguoiKhangCao(hoSoVuAnId);
            foreach (var item in nguoikhangcao)
            {
                item.NoidungKhangCao = replacetag(item.NoidungKhangCao);
            }
            if (dbModel == null)

                return new MauInSo65ViewModel
                {
                    TenToaAn = "..................",
                    VienKiemSatKhangNghi = "..................",
                    ToaAnSoTham = "................................",
                    ThamPhan = "......................................"
                };
            else
            {
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                return new MauInSo65ViewModel
                {
                    MaHoSo = dbModel.MaHoSo,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    NgayThuLy = dbModel.NgayThuLy,
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    VienKiemSatKhangNghi = dbModel.VienKiemSatKhangNghi,
                    SoBanAn = AddZero(dbModel.SoBanAn),
                    SoQuyetDinh = AddZero(dbModel.SoQuyetDinh),
                    NgayTuyenAn = dbModel.NgayTuyenAn,
                    NgayRaQuyetDinh = dbModel.NgayTuyenAn,
                    ToaAnSoTham = dbModel.ToaAnSoTham.Replace("TAND ", ""),
                    ThamPhan = dbModel.ThamPhan ?? "......................................",
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat,
                    NoiDungKhangNghi = replacetag(dbModel.NoiDungKhangNghi),
                    DanhSachDuongSu = danhSachDuongSu,
                    NguoiKhangCao = nguoikhangcao
                };
            }
        }
        public MauInQuyetDinhPCTPViewModel DuLieuMauInQuyetDinhPCTP(int hoSoVuAnId, int giaiDoan)
        {
            string toi = "", luat="";
            var dbModel = MauInDataAccess.ChiTietMauInQuetDinhPCTP(hoSoVuAnId, giaiDoan);
            var danhSachDuongSu = new List<DuongSuModel>();
            danhSachDuongSu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId).ToList();
            var danhSachThamPhanDuKhuyet = new List<ThamPhanDuKhuyetMauInSo47Model>();
            danhSachThamPhanDuKhuyet = MauInDataAccess.DanhSachThamPhanDuKhuyetMauInSo47(hoSoVuAnId).ToList();
            var danhSachHoiThamNhanDanDuKhuyet = new List<HoiThamNhanDanDuKhuyetMauInSo47Model>();
            danhSachHoiThamNhanDanDuKhuyet = MauInDataAccess.DanhSachHoiThamNhanDanDuKhuyetMauInSo47(hoSoVuAnId).ToList();
            int khangcao = 0;
            List<DuongSuModel> danhSachNguyenDon = new List<DuongSuModel>();
            List<DuongSuModel> danhSachBiDon = new List<DuongSuModel>();
            List<DuongSuModel> danhSachNguoiLQ = new List<DuongSuModel>();
            List<DuongSuModel> danhSachBiCao = new List<DuongSuModel>();
            var danhSachThuKyDuKhuyet = MauInDataAccess.DanhSachThuKyDuKhuyet(hoSoVuAnId);
            if (dbModel == null)
                return null;
            else
            {
                if (dbModel.NhomAn == "HS")
                {
                    foreach (var duongSu in danhSachDuongSu)
                    {
                        if (duongSu.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAN) || duongSu.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAO))
                        {
                            danhSachBiCao.Add(duongSu);
                        }

                    }
                    var toiDanhTruyTo = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
                    foreach (var item in toiDanhTruyTo)
                    {
                        luat += item.Dieu + " " + item.DieuLuatApDung + " - ";
                        toi += item.ToiDanh + ", ";
                    }
                    if (toi != "" &&  toi.LastIndexOf(", ")!=-1)
                        toi = toi.Remove(toi.LastIndexOf(","), 2);
                    if (luat != "" && luat.LastIndexOf(" - ") != -1)
                        luat = luat.Remove(luat.LastIndexOf(" - "), 3);
                }
                else if (dbModel.NhomAn == "AD")
                {
                    var temp = MauInDataAccess.DanhSachNguoiKhangCaoGXN(hoSoVuAnId);
                    if (temp != null)
                        khangcao = temp.Count();
                }
                else
                    foreach (var duongSu in danhSachDuongSu)
                    {
                        if (dbModel.LoaiQuanHe.Equals(ViewText.LABEL_LOAIQUANHE_TRANHCHAP))
                        {
                            if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON) | duongSu.TuCachThamGiaToTung.Equals("Người khởi kiện"))
                            {
                                danhSachNguyenDon.Add(duongSu);
                            }
                            else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_BIDON) | duongSu.TuCachThamGiaToTung.Equals("Người bị kiện"))
                            {
                                danhSachBiDon.Add(duongSu);
                            }

                        }
                        else if (dbModel.LoaiQuanHe.Equals(ViewText.LABEL_LOAIQUANHE_YEUCAU))
                        {
                            if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU))
                            {
                                danhSachNguyenDon.Add(duongSu);
                            }
                            else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN))
                            {
                                danhSachBiDon.Add(duongSu);
                            }
                        }
                        if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN))
                        {
                            danhSachNguoiLQ.Add(duongSu);
                        }
                    }

                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                if (dbModel.ToaAnSoTham == null) dbModel.ToaAnSoTham = "..................";
                return new MauInQuyetDinhPCTPViewModel()
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    ToaAnSoTham = dbModel.ToaAnSoTham.Replace("TAND ", "") ?? "...............",
                    MaToaAn = dbModel.MaToaAn,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    MaHoSo = dbModel.MaHoSo,
                    NhomAn = dbModel.NhomAn,
                    ThamPhan = dbModel.ThamPhan ?? "..............................",
                    GioiTinhThamPhan = dbModel.GioiTinhThamPhan,
                    ThamPhan1 = dbModel.ThamPhan1 ?? "..............................",
                    GioiTinhThamPhan1 = dbModel.GioiTinhThamPhan1,
                    ThamPhan2 = dbModel.ThamPhan2 ?? "..............................",
                    GioiTinhThamPhan2 = dbModel.GioiTinhThamPhan2,
                    HoiThamNhanDan = dbModel.HoiThamNhanDan ?? "..............................",
                    GioiTinhHoiThamNhanDan = dbModel.GioiTinhHoiThamNhanDan,
                    HoiThamNhanDan2 = dbModel.HoiThamNhanDan2 ?? "..............................",
                    GioiTinhHoiThamNhanDan2 = dbModel.GioiTinhHoiThamNhanDan2,
                    HoiThamNhanDan3 = dbModel.HoiThamNhanDan3 ?? "..............................",
                    GioiTinhHoiThamNhanDan3 = dbModel.GioiTinhHoiThamNhanDan3,
                    ChanhAn = dbModel.ChanhAn,
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    NgayThuLy = dbModel.NgayThuLy,
                    NgayPhanCong = dbModel.NgayPhanCong,
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat ?? "..............................",
                    DanhNguoiCoQLNVLienQuan = danhSachNguoiLQ,
                    DanhSachNguyenDon = danhSachNguyenDon,
                    DanhSachBiDon = danhSachBiDon,
                    DanhSachThamPhanDuKhuyet = danhSachThamPhanDuKhuyet,
                    DanhSachHoiThamNhanDanDuKhuyet = danhSachHoiThamNhanDanDuKhuyet,
                    HoiDong = dbModel.HoiDong,
                    GiaiDoan = dbModel.GiaiDoan,
                    ThamPhanKhac = dbModel.ThamPhanKhac,
                    GioiTinhThamPhanKhac = dbModel.GioiTinhThamPhanKhac,
                    DanhSachBiCao = danhSachBiCao,
                    LoaiChanhAn = dbModel.LoaiChanhAn ?? "",
                    ThuKy = dbModel.ThuKy,
                    GioiTinhThuKy = dbModel.GioiTinhThuKy,
                    DanhSachThuKyDuKhuyet = danhSachThuKyDuKhuyet,
                    NoiDungDon = replacetag(dbModel.NoiDungDon),
                    KhangCao = khangcao,
                    DanhSachDuongSu = danhSachDuongSu,
                    BienPhapXuLyHanhChinh = dbModel.BienPhapXuLyHanhChinh,
                    VienKiemSatKhangNghi = dbModel.VienKiemSatKhangNghi,
                    ThuLyAD = AddZero(dbModel.ThuLyAD),
                    NgayNopDonTaiToaAn = dbModel.NgayNopDonTaiToaAn,
                    ToiDanh=toi,
                    DieuLuat=luat
                };
            }
        }
        public MauInQuyetDinhGiaHanCBXXViewModel ChiTietMauInGiaHanCBXX(int hoSoVuAnId, int giaiDoan)
        {
            var dbModel = MauInDataAccess.ChiTietMauInGiaHanCBXX(hoSoVuAnId, giaiDoan);
            var dsduongsu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId);
            List<DuongSuModel> danhSachBiCao = new List<DuongSuModel>();
            string toi = "", luat = "";
            if (dbModel == null)
                return null;
            else
            {
                foreach (var duongSu in dsduongsu)
                {
                    if (duongSu.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAN) || duongSu.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAO))
                    {
                        danhSachBiCao.Add(duongSu);
                    }

                }
                var toiDanhTruyTo = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
                foreach (var item in toiDanhTruyTo)
                {
                    toi += item.ToiDanh + ", ";
                    luat += item.Dieu + " " + item.DieuLuatApDung + " - ";
                }
                if (toi != "" &&  toi.LastIndexOf(", ")!=-1)
                    toi = toi.Remove(toi.LastIndexOf(","), 2);
                if (luat != "" &&  luat .LastIndexOf(" - ")!=-1)
                    luat = luat.Remove(luat.LastIndexOf(" - "), 3);
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                if (dbModel.ToaAnSoTham == null) dbModel.ToaAnSoTham = "..................";
                return new MauInQuyetDinhGiaHanCBXXViewModel()
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    ToaAnSoTham = dbModel.ToaAnSoTham.Replace("TAND ", "") ?? "...............",
                    MaToaAn = dbModel.MaToaAn,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    MaHoSo = dbModel.MaHoSo,
                    NhomAn = dbModel.NhomAn,
                    ChanhAn = dbModel.ChanhAn,
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    NgayThuLy = dbModel.NgayThuLy,
                    DanhSachBiCao = danhSachBiCao,
                    LoaiChanhAn = dbModel.LoaiChanhAn ?? "",
                    SoQuyetDinh = AddZero(dbModel.SoQuyetDinh),
                    NgayRaQuyetDinh = dbModel.NgayRaQuyetDinh,
                    ThoiHanGiaHan = dbModel.ThoiHanGiaHan,
                    VienKiemSatTruyTo = dbModel.VienKiemSatTruyTo,
                    ToiDanh = toi,
                    DieuLuat = luat,
                    Giaidoan=giaiDoan
                };
            }
        }
        public MauInQuyetDinhTamHoanViewModel ChiTietMauInTamHoan(int hoSoVuAnId, int giaiDoan)
        {
            var dbModel = MauInDataAccess.ChiTietMauInTamHoan(hoSoVuAnId, giaiDoan);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId);
            List<DuongSuModel> danhSachBiCao = new List<DuongSuModel>();
            string toi = "", luat = "";
            if (dbModel == null)
                return null;
            else
            {
                foreach (var duongSu in danhSachDuongSu)
                {
                    if (duongSu.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAN) || duongSu.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAO))
                    {
                        danhSachBiCao.Add(duongSu);
                    }
                }
                var toiDanhTruyTo = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
                foreach (var item in toiDanhTruyTo)
                {
                    toi += item.ToiDanh + ", ";
                    luat += item.Dieu + " " + item.DieuLuatApDung + " - ";
                }
                if (toi != "" &&  toi.LastIndexOf(", ")!=-1)
                    toi = toi.Remove(toi.LastIndexOf(","), 2);
                if (luat != "" &&  luat .LastIndexOf(" - ")!=-1)
                    luat = luat.Remove(luat.LastIndexOf(" - "), 3);
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                return new MauInQuyetDinhTamHoanViewModel()
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    MaToaAn = dbModel.MaToaAn,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    MaHoSo = dbModel.MaHoSo,
                    NhomAn = dbModel.NhomAn,
                    GiaiDoan = dbModel.GiaiDoan,
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    NgayThuLy = dbModel.NgayThuLy,
                    DanhSachBiCao = danhSachBiCao,
                    SoQuyetDinh = AddZero(dbModel.SoQuyetDinh),
                    NgayRaQuyetDinh = dbModel.NgayRaQuyetDinh,
                    LyDo = dbModel.LyDo,
                    ToaAnSoTham = dbModel.ToaAnSoTham != null ? dbModel.ToaAnSoTham.Replace("TAND ", "") ?? "..............." : dbModel.ToaAnSoTham,
                    HoiDong = dbModel.HoiDong,
                    ThamPhan = dbModel.ThamPhan,
                    GioiTinhThamPhan = dbModel.GioiTinhThamPhan,
                    ThamPhan1 = dbModel.ThamPhan1,
                    GioiTinhThamPhan1 = dbModel.GioiTinhThamPhan1,
                    ThamPhan2 = dbModel.ThamPhan2,
                    GioiTinhThamPhan2 = dbModel.GioiTinhThamPhan2,
                    ThamPhanKhac = dbModel.ThamPhanKhac,
                    GioiTinhThamPhanKhac = dbModel.GioiTinhThamPhanKhac,
                    HoiThamNhanDan = dbModel.HoiThamNhanDan,
                    GioiTinhHoiThamNhanDan = dbModel.GioiTinhHoiThamNhanDan,
                    HoiThamNhanDan2 = dbModel.GioiTinhHoiThamNhanDan2,
                    GioiTinhHoiThamNhanDan2 = dbModel.GioiTinhHoiThamNhanDan2,
                    HoiThamNhanDan3 = dbModel.HoiThamNhanDan3,
                    GioiTinhHoiThamNhanDan3 = dbModel.GioiTinhHoiThamNhanDan3,
                    ThuKy = dbModel.ThuKy,
                    GioiTinhThuKy = dbModel.GioiTinhThuKy,
                    KiemSatVien = dbModel.KiemSatVien,
                    GioiTinhKiemSatVien = dbModel.GioiTinhKiemSatVien,
                    ThoiHanGiaHan = dbModel.ThoiHanGiaHan,
                    ChanhAn = dbModel.ChanhAn,
                    LoaiChanhAn = dbModel.LoaiChanhAn ?? "",
                    LanThu = dbModel.LanThu,
                    DiaChiToaAn = dbModel.DiaChiToaAn,
                    DiaDiemMoPhienToa = dbModel.DiaDiemMoPhienToa.Replace("TAND ", "").First().ToString() + dbModel.DiaDiemMoPhienToa.Replace("TAND ", "").Remove(0, 1) ?? "...............",
                    ThoiGianMoPhienToa = dbModel.ThoiGianMoPhienToa,
                    ToiDanh = toi,
                    DieuLuat = luat
                };
            }
        }
        public MauInLenhTrichXuatViewModel ChiTietMauInLenhTrichXuat(int hoSoVuAnId, int giaiDoan)
        {
            var dbModel = MauInDataAccess.ChiTietMauInLenhTrichXuat(hoSoVuAnId, giaiDoan);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId);
            List<DuongSuModel> danhSachBiCao = new List<DuongSuModel>();
            if (dbModel == null)
                return null;
            else
            {
                foreach (var duongSu in danhSachDuongSu)
                {
                    if (duongSu.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAN) || duongSu.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAO) && (duongSu.TinhTrangGiamGiu.Equals("Đang tạm giam")))
                    {
                        danhSachBiCao.Add(duongSu);
                    }
                }
                return new MauInLenhTrichXuatViewModel()
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    MaToaAn = dbModel.MaToaAn,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    MaHoSo = dbModel.MaHoSo,
                    NhomAn = dbModel.NhomAn,
                    GiaiDoan = dbModel.GiaiDoan,
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    NgayThuLy = dbModel.NgayThuLy,
                    DanhSachBiCao = danhSachBiCao,
                    NgayRaQuyetDinhXetXu = dbModel.NgayRaQuyetDinhXetXu,
                    ThamPhan = dbModel.ThamPhan,
                    DiaChiToaAn = dbModel.DiaChiToaAn,
                    VienKiemSatTruyTo = dbModel.VienKiemSatTruyTo,
                    ThoiGianMoPhienToa = dbModel.ThoiGianMoPhienToa,

                };
            }
        }
        public MauInQDTamGiamViewModel ChiTietMauInTamGiam_4_5_9(int hoSoVuAnId, int giaiDoan, int congDoan, int mauInSo)
        {
            var dbModel = MauInDataAccess.ChiTietMauInTamGiam_4_5_9(hoSoVuAnId, giaiDoan);
            var hinhPhat = MauInDataAccess.DanhSachBiCaoHinhPhat(hoSoVuAnId);
            if (dbModel == null)
                return null;
            else
            {
                if (dbModel.ToaAnSoTham == null) dbModel.ToaAnSoTham = "..................";
                List<DuongSuModel> dsBicao = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId).Where(x => x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAN || x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO).ToList();
                foreach (var item in dsBicao)
                {
                    string s = "";
                    var phat = hinhPhat.Where(x => x.DuongSuID == item.DuongSuID);
                    if (phat != null && phat.Count() > 1)
                    {
                        foreach (var p in phat)
                            s += p.HinhPhat + ", ";
                        s = s.Remove(s.LastIndexOf(","), 1);
                        item.HinhPhat = s;
                    }
                    else if (phat != null && phat.Count() == 1)
                        item.HinhPhat = phat.FirstOrDefault().HinhPhat;
                }
                MauInQDTamGiamViewModel mauIn = new MauInQDTamGiamViewModel()
                {
                    MaHoSo = dbModel.MaHoSo,
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    ToaAnSoTham = dbModel.ToaAnSoTham.Replace("TAND ", "") ?? "...............",
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    NgayThuLy = dbModel.NgayThuLy,
                    SoQuyetDinh = AddZero(dbModel.SoQuyetDinh),
                    NgayQuyetDinh = dbModel.NgayQuyetDinh,
                    NhomAn = dbModel.NhomAn,
                    ChanhAn = dbModel.ChanhAn,
                    ThoiHanGiaHan = dbModel.ThoiHanGiaHan,
                    ThoiHanGiaiQuyet = dbModel.ThoiHanGiaiQuyet,
                    LoaiChanhAn = dbModel.LoaiChanhAn ?? "",
                    VienKiemSatTruyTo = dbModel.VienKiemSatTruyTo,
                    DanhSachBiCao = dsBicao
                };
                if (congDoan == CongDoan.ChuanBiXetXu.GetHashCode())
                {
                    mauIn.NgayTamGiam = dbModel.NgayThuLy.AddDays(int.Parse(dbModel.ThoiHanGiaiQuyet));
                    mauIn.ThoiHanTamGiamBangChu = So_chu(double.Parse(dbModel.ThoiHanGiaHan)).Replace(" đồng", "");
                    if (mauInSo == 5 && dbModel.ThoiHanGiaHan != null)
                    {
                        mauIn.NgayTamGiam = mauIn.NgayTamGiam.AddDays(int.Parse(dbModel.ThoiHanGiaHan));
                    }
                }
                else
                {
                    mauIn.ThoiHanTamGiamBangChu = So_chu(double.Parse(dbModel.ThoiHanGiaiQuyet)).Replace(" đồng", "");
                }

                return mauIn;
            }

        }
        public MauInQuyetDinhDinhChiViewModel ChiTietMauInQuyetDinhDinhChi(int hoSoVuAnId, string loai)
        {
            var dbModel = MauInDataAccess.ChiTietMauInQuyetDinhDinhChi(hoSoVuAnId, loai);
            var danhsachduongSu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId);
            if (dbModel == null)
                return null;
            else
            {
                return new MauInQuyetDinhDinhChiViewModel
                {
                    MaHoSo = dbModel.MaHoSo,
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    ThamPhan = dbModel.ThamPhan,
                    SoQuyetDinh = dbModel.SoQuyetDinh,
                    NgayRaQuyetDinh = dbModel.NgayRaQuyetDinh,
                    ThuLyAD = dbModel.ThuLyAD,
                    NhomAn = dbModel.NhomAn,
                    MaToaAn = dbModel.MaToaAn,
                    BienPhapXuLyHanhChinh = dbModel.BienPhapXuLyHanhChinh,
                    NgayNopDonTaiToaAn = dbModel.NgayNopDonTaiToaAn,
                    LyDo = dbModel.LyDo,
                    DanhSachDuongSu = danhsachduongSu.ToList()
                };
            }
        }
        public MauInGiayTrieuTapViewModel DuLieuMauInGiayTrieuTap(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.ChiTietMauInGiayTrieuTap(hoSoVuAnId);
            List<DuongSuModel> danhSachDuongSu = MauInDataAccess.DanhSachDuongSuMauInGiayTrieuTap(hoSoVuAnId).ToList();
            List<DuongSuModel> listDuongSu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId).ToList();
            MauInGiayTrieuTapViewModel mauInGiayTrieuTap = new MauInGiayTrieuTapViewModel();
            string toidanh = "";
            if (dbModel == null)
                return null;
            else
            {
                var tenNguyenDon = "";
                var tenBiDon = "";
                if (dbModel.NhomAn != "HS" && dbModel.NhomAn != "AD")
                    foreach (var duongSu in listDuongSu)
                    {
                        if (dbModel.LoaiQuanHe.Equals(ViewText.LABEL_LOAIQUANHE_TRANHCHAP))
                        {
                            if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON) | duongSu.TuCachThamGiaToTung.Equals("Người khởi kiện"))
                            {
                                if (duongSu.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    tenNguyenDon += duongSu.HoTenDuongSu + ", ";
                                else
                                    tenNguyenDon += duongSu.TenCoQuanToChuc + ", ";
                            }
                            else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_BIDON) | duongSu.TuCachThamGiaToTung.Equals("Người bị kiện"))
                            {
                                if (duongSu.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    tenBiDon += duongSu.HoTenDuongSu + ", ";
                                else
                                    tenBiDon += duongSu.TenCoQuanToChuc + ", ";
                            }
                        }
                        else if (dbModel.LoaiQuanHe.Equals(ViewText.LABEL_LOAIQUANHE_YEUCAU))
                        {
                            if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU))
                            {
                                tenNguyenDon += duongSu.HoTenDuongSu + ", ";
                            }
                            else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN))
                            {
                                tenBiDon += duongSu.HoTenDuongSu + ", ";
                            }
                        }
                    }
                else if (dbModel.NhomAn == "HS")
                {
                    var toiDanhTruyTo = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
                    foreach (var item in toiDanhTruyTo)
                    {
                        toidanh += item.ToiDanh + ", ";
                    }
                    var hinhPhat = MauInDataAccess.DanhSachBiCaoHinhPhat(hoSoVuAnId);
                    if (toidanh != "")
                        toidanh = toidanh.Remove(toidanh.LastIndexOf(","), 1);
                }
                if (!tenNguyenDon.Equals(""))
                    tenNguyenDon = tenNguyenDon.Remove(tenNguyenDon.LastIndexOf(','), 1);

                if (!tenBiDon.Equals(""))
                    tenBiDon = tenBiDon.Remove(tenBiDon.LastIndexOf(','), 2); //remove ', ' bc it is at the end of sentence.

                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                if (dbModel.ToaAnSoTham == null) dbModel.ToaAnSoTham = "..................";

                mauInGiayTrieuTap = new MauInGiayTrieuTapViewModel()
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    ToaAnSoTham = dbModel.ToaAnSoTham.Replace("TAND ", "") ?? "...............",
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    MaHoSo = dbModel.MaHoSo,
                    ThamPhan = dbModel.ThamPhan ?? "..................",
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat ?? "..............................",
                    ThoiGianMoPhienToa = dbModel.ThoiGianMoPhienToa,
                    DiaDiemMoPhienToa = dbModel.DiaDiemMoPhienToa,
                    DiaChiToaAn = dbModel.DiaChiToaAn,
                    NgayRaThongBao=dbModel.NgayRaThongBao,
                    NgayHienTai = DateTime.Now,
                    DanhSachDuongSu = danhSachDuongSu,
                    TenNguyenDon = tenNguyenDon,
                    TenBiDon = tenBiDon,
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    NgayThuLy = dbModel.NgayThuLy,
                    TenVuAn = dbModel.TenVuAn,
                    BienPhapXuLyHanhChinh = dbModel.BienPhapXuLyHanhChinh,
                    ToiDanh = toidanh ?? "..............................",
                    GiaiDoan=dbModel.GiaiDoan,
                    NguoiKy=dbModel.NguoiKy
                };
                return mauInGiayTrieuTap;
            }
        }
        public MauInBBGiaoNhanModel ChiTietMauInBienBanGiaoNhan(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.ChiTietMauInBienBanGiaoNhan(hoSoVuAnId);
            if (dbModel == null)
                return null;
            else
            {
                var dsduongsu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId);
                List<DuongSuModel> dsBiCao = new List<DuongSuModel>();
                string toidanh = null;
                foreach (var item in dsduongsu)
                {
                    if (item.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAN) | item.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_BICAO))
                        dsBiCao.Add(item);
                }
                var toiDanhTruyTo = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
                foreach (var item in toiDanhTruyTo)
                {
                    toidanh += item.ToiDanh + ", ";
                }
                var hinhPhat = MauInDataAccess.DanhSachBiCaoHinhPhat(hoSoVuAnId);
                if (toidanh != "")
                    toidanh = toidanh.Remove(toidanh.LastIndexOf(","), 1);
                dbModel.TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............";
                dbModel.ToiDanh = toidanh;
            }
            return dbModel;
        }
        public MauInGXNKhangCaoViewModel ChiTietGXNKhangCao(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.ChiTietGXNKhangCao(hoSoVuAnId);
            var nguoiKhangCao = MauInDataAccess.DanhSachNguoiKhangCaoGXN(hoSoVuAnId);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId);
            if (dbModel == null)
                return null;
            else
            {
                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                if (dbModel.ToaAnSoTham == null) dbModel.ToaAnSoTham = "..................";
                return new MauInGXNKhangCaoViewModel()
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "...............",
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    MaHoSo = dbModel.MaHoSo,
                    ThamPhan = dbModel.ThamPhan ?? "..................",
                    SoBanAn = AddZero(dbModel.SoBanAn),
                    SoQuyetDinh = AddZero(dbModel.SoQuyetDinh),
                    NgayRaBanAn = dbModel.NgayRaBanAn,
                    NgayRaQuyetDinh = dbModel.NgayRaQuyetDinh,
                    NguoiKhangCao = nguoiKhangCao,
                    HinhThucGoiDon = dbModel.HinhThucGoiDon,
                    DanhSachDuongSu = danhSachDuongSu.ToList(),
                    ToaAnSoTham = dbModel.ToaAnSoTham.Replace("TAND ", "") ?? "",
                    NhomAn=dbModel.NhomAn,
                    LoaiQuanHe=dbModel.LoaiQuanHe
                };
            }
        }
        public MauInBiaHoSoNhanDonViewModel ChiTietMauInBiaHoSoNhanDon(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.ChiTietMauInBiaHoSoNhanDon(hoSoVuAnId);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId);

            List<DuongSuModel> danhSachNguyenDon = new List<DuongSuModel>();
            List<DuongSuModel> danhSachBiDon = new List<DuongSuModel>();
            List<DuongSuModel> danhSachNguoiLienQuanTranhChap = new List<DuongSuModel>();
            if (dbModel == null)
                return null;
            else
            {
                foreach (var duongSu in danhSachDuongSu)
                {
                    if (dbModel.LoaiQuanHe.Equals(ViewText.LABEL_LOAIQUANHE_TRANHCHAP))
                    {
                        if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON) || duongSu.TuCachThamGiaToTung.Equals("Người khởi kiện"))
                        {
                            danhSachNguyenDon.Add(duongSu);
                        }
                        else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_BIDON) || duongSu.TuCachThamGiaToTung.Equals("Người bị kiện"))
                        {
                            danhSachBiDon.Add(duongSu);
                        }
                        else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN))
                        {
                            danhSachNguoiLienQuanTranhChap.Add(duongSu);
                        }
                    }
                    else if (dbModel.LoaiQuanHe.Equals(ViewText.LABEL_LOAIQUANHE_YEUCAU))
                    {
                        if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU))
                        {
                            danhSachNguyenDon.Add(duongSu);
                        }
                        else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN))
                        {
                            danhSachBiDon.Add(duongSu);
                        }
                    }
                }

                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                MauInBiaHoSoNhanDonViewModel mauInBiaHoSo = new MauInBiaHoSoNhanDonViewModel
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "..............................",
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    MaHoSo = dbModel.MaHoSo,
                    NhomAn = dbModel.NhomAn,
                    ThamPhanChuToa = dbModel.ThamPhanChuToa ?? "..............................",
                    GioiTinhThamPhanChuToa = dbModel.GioiTinhThamPhanChuToa,
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat ?? "..............................",
                    NgayNhanDon = dbModel.NgayNhanDon,
                    DanhSachNguyenDon = danhSachNguyenDon,
                    DanhSachBiDon = danhSachBiDon,
                    DanhSachNguoiLienQuanTranhChap = danhSachNguoiLienQuanTranhChap,
                    NoiDungDon = replacetag(dbModel.NoiDungDon)
                };

                return mauInBiaHoSo;
            }
        }
        public MauInBiaHoSoViewModel DuLieuMauInBiaHoSo(int hoSoVuAnId)
        {
            var dbModel = MauInDataAccess.ChiTietMauInBiaHoSo(hoSoVuAnId);
            var danhSachDuongSu = MauInDataAccess.DanhSachDuongSuTheoHoSoVuAnID(hoSoVuAnId);

            List<DuongSuModel> danhSachNguyenDon = new List<DuongSuModel>();
            List<DuongSuModel> danhSachBiDon = new List<DuongSuModel>();
            List<DuongSuModel> danhSachNguoiLienQuanTranhChap = new List<DuongSuModel>();
            List<DuongSuModel> danhSachBiCao = MauInDataAccess.DanhSachBiCaoHinhPhat(hoSoVuAnId);
            var danhSachNguoiKhangCao = MauInDataAccess.DanhSachNguoiKhangCaoGXN(hoSoVuAnId).ToList();
            if (dbModel == null)
                return null;
            else
            {
                
                foreach (var duongSu in danhSachDuongSu)
                {
                    if (dbModel.LoaiQuanHe == ViewText.LABEL_LOAIQUANHE_TRANHCHAP)
                    {
                        if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON) || duongSu.TuCachThamGiaToTung.Equals("Người khởi kiện"))
                        {
                            danhSachNguyenDon.Add(duongSu);
                        }
                        else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_BIDON) || duongSu.TuCachThamGiaToTung.Equals("Người bị kiện"))
                        {
                            danhSachBiDon.Add(duongSu);
                        }
                        else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN))
                        {
                            danhSachNguoiLienQuanTranhChap.Add(duongSu);
                        }
                    }
                    else if (dbModel.LoaiQuanHe == ViewText.LABEL_LOAIQUANHE_YEUCAU)
                    {
                        if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU))
                        {
                            danhSachNguyenDon.Add(duongSu);
                        }
                        else if (duongSu.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN))
                        {
                            danhSachBiDon.Add(duongSu);
                        }
                    }
                }

                if (dbModel.TenToaAn == null) dbModel.TenToaAn = "..................";
                if (dbModel.ToaAnSoTham == null) dbModel.ToaAnSoTham = "..................";
                MauInBiaHoSoViewModel mauInBiaHoSo = new MauInBiaHoSoViewModel
                {
                    TenToaAn = dbModel.TenToaAn.Replace("TAND ", "") ?? "..............................",
                    ToaAnSoTham = dbModel.ToaAnSoTham.Replace("TAND ", "") ?? "..............................",
                    SoHoSo = AddZero(dbModel.SoHoSo),
                    MaHoSo = dbModel.MaHoSo,
                    NhomAn = dbModel.NhomAn,
                    ThamPhanChuToa = dbModel.ThamPhanChuToa ?? "..............................",
                    GioiTinhThamPhanChuToa = dbModel.GioiTinhThamPhanChuToa,
                    ThamPhan1 = dbModel.ThamPhan1 ?? "..............................",
                    GioiTinhThamPhan1 = dbModel.GioiTinhThamPhan1,
                    GioiTinhThamPhan2 = dbModel.GioiTinhThamPhan2,
                    ThamPhan2 = dbModel.ThamPhan2 ?? "..............................",
                    HoiThamNhanDan1 = dbModel.HoiThamNhanDan1 ?? "..............................",
                    GioiTinhHoiThamNhanDan1 = dbModel.GioiTinhHoiThamNhanDan1,
                    HoiThamNhanDan2 = dbModel.HoiThamNhanDan2 ?? "..............................",
                    GioiTinhHoiThamNhanDan2 = dbModel.GioiTinhHoiThamNhanDan2,
                    HoiThamNhanDan3 = dbModel.HoiThamNhanDan2 ?? "..............................",
                    GioiTinhHoiThamNhanDan3 = dbModel.GioiTinhHoiThamNhanDan3,
                    SoThuLy = AddZero(dbModel.SoThuLy),
                    NgayThuLy = dbModel.NgayThuLy,
                    NgayRaQuyetDinhXetXu = dbModel.NgayRaQuyetDinhXetXu,
                    LoaiQuanHe = dbModel.LoaiQuanHe,
                    QuanHePhapLuat = dbModel.QuanHePhapLuat ?? "..............................",
                    KiemSatVien = dbModel.KiemSatVien,
                    GioiTinhKiemSatVien = dbModel.GioiTinhKiemSatVien,
                    ThuKy = dbModel.ThuKy,
                    GioiTinhThuKy = dbModel.GioiTinhThuKy,
                    DanhSachNguyenDon = danhSachNguyenDon,
                    DanhSachBiDon = danhSachBiDon,
                    DanhSachNguoiLienQuanTranhChap = danhSachNguoiLienQuanTranhChap,
                    NgayRaQuyetDinh = dbModel.NgayRaQuyetDinh,
                    SoQuyetDinh = AddZero(dbModel.SoQuyetDinh),
                    DanhSachDuongSuAD = danhSachDuongSu.ToList(),
                    SoBiCan = dbModel.SoBiCan,
                    SoCaoTrang = AddZero(dbModel.SoCaoTrang),
                    NgayCaoTrang = dbModel.NgayCaoTrang,
                    DanhSachBiCao = danhSachBiCao,
                    ThamPhanKhac = dbModel.ThamPhanKhac,
                    GioiTinhThamPhanKhac = dbModel.GioiTinhThamPhanKhac,
                    BienPhapXuLyHanhChinh = dbModel.BienPhapXuLyHanhChinh,
                    TenCoQuanDeNghi = dbModel.TenCoQuanDeNghi,
                    HoiDong = dbModel.HoiDong,
                    SoBanAn = AddZero(dbModel.SoBanAn),
                    NgayRaBanAn = dbModel.NgayRaBanAn,
                    NoiDungBanAn = replacetag(dbModel.NoiDungBanAn),
                    NoiDungQuyetDinh = replacetag(dbModel.NoiDungQuyetDinh),
                    DanhSachNguoiKhangCao = danhSachNguoiKhangCao,
                    NgayKhangNghi = dbModel.NgayKhangNghi,
                    SoKhangNghi = dbModel.SoKhangNghi,
                    ThuLyAD = AddZero(dbModel.ThuLyAD),
                    NgayNopDonTaiToaAn = dbModel.NgayNopDonTaiToaAn,
                    SoBanAnPT = AddZero(dbModel.SoBanAnPT),
                    NgayRaBanAnPT = dbModel.NgayRaBanAnPT,
                    SoQuyetDinhPT = AddZero(dbModel.SoQuyetDinhPT),
                    NgayRaQuyetDinhPT = dbModel.NgayRaQuyetDinhPT,
                    VienKiemSatKhangNghi = dbModel.VienKiemSatKhangNghi,
                    HieuLuc = dbModel.HieuLuc,
                    NoiDungBanAnPT = replacetag(dbModel.NoiDungBanAnPT),
                    NoiDungQuyetDinhPT = replacetag(dbModel.NoiDungQuyetDinhPT),
                    GiaiDoan=dbModel.GiaiDoan,
                    NoiDungKhangNghi=dbModel.NoiDungKhangNghi
                };

                return mauInBiaHoSo;
            }
        }
        #endregion

        #region Danh Sach Mau In Page
        public IEnumerable<Lib.TACM.MauIn.Model.NhomAnModel> DanhSachNhomAnTheoToaAn(int toaAnId)
        {
            var dbModel = MauInDataAccess.DanhSachNhomAnTheoToaAn(toaAnId);
            List<Lib.TACM.MauIn.Model.NhomAnModel> dsNhomAn = dbModel.Select(s => new Lib.TACM.MauIn.Model.NhomAnModel()
            {
                NhomAnID = s.NhomAnID,
                ToaAnID = s.ToaAnID,
                MaNhomAn = s.MaNhomAn,
                TenNhomAn = s.TenNhomAn
            }).ToList();
            return dsNhomAn;
        }

        public IEnumerable<MauInModel> DanhSachMauInTheoGiaiDoanVaNhomAn(int? giaiDoan, string nhomAn)
        {
            var dbModel = MauInDataAccess.DanhSachMauInTheoGiaiDoanVaNhomAn(giaiDoan, nhomAn);
            List<MauInModel> dsMauIn = dbModel.Select(s => new MauInModel()
            {
                MauInID = s.MauInID,
                NhomAn = s.NhomAn,
                GiaiDoan = s.GiaiDoan,
                CongDoanHoSo = s.CongDoanHoSo,
                SoMauIn = s.SoMauIn,
                TenMauIn = s.TenMauIn,
                DuongDanFileMau = s.DuongDanFileMau,
                DinhDangHTML = s.DinhDangHTML,
                TrangThai = s.TrangThai,
                NguoiTao = s.NguoiTao,
                NgayTao = s.NgayTao,
                GhiChu = s.GhiChu
            }).ToList();
            return dsMauIn;
        }

        public IEnumerable<MauInModel> DanhSachMauInSearchByKeyword(string keyword)
        {
            var danhSachMauIn = MauInDataAccess.DanhSachMauInSearchByKeyword(keyword);
            return danhSachMauIn;
        }
        #endregion

        #region Doc chinh thuc

        public string[] MauInGiayXacNhanKhangCaoDoc(MauInGXNKhangCaoViewModel mauInObject, string filePath, string templatePath)
        {
            string filePathTemp = filePath.Replace(".docx", "_temp_.docx");
            string filePathTemp2 = filePath.Replace(".docx", "_temp2_.docx");
            try
            {
                foreach (var item in mauInObject.NguoiKhangCao)
                {
                    if (item == mauInObject.NguoiKhangCao.FirstOrDefault() || mauInObject.NguoiKhangCao.Count() == 1)
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            document.ApplyTemplate(templatePath);
                            document.ReplaceText("{SoKhangCao}", item.KhangCaoID.ToString());
                            document.ReplaceText("{NamNop}", item.NgayNop.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if(mauInObject.NhomAn==Setting.MANHOMAN_HANHCHINH)
                            {
                                if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                                {
                                    document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                                }
                                else
                                {
                                    var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                    tempToaAnTinhParagraph.Remove(false);
                                }
                            }
                            else
                            {
                                if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                                {
                                    document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                                }
                                else
                                {
                                    var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                    tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                                    tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false).FontSize(12).Bold().Alignment = Alignment.center;
                                }
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayNopDon}", NgayThangNam(item.NgayNop));
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{NguoiKhangCao}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{NguoiKhangCao}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{DiaChiNguoiKhangCao}", item.NoiTamTru ?? item.NoiDKHKTT);
                            document.ReplaceText("{NgayDe}", NgayThangNam(item.NgayDe));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);
                            document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                            if (mauInObject.HinhThucGoiDon == "Trực tiếp")
                            {
                                document.ReplaceText("{HinhThucNop}", "nộp trực tiếp");
                            }
                            else if (mauInObject.HinhThucGoiDon == "Gián tiếp")
                            {
                                document.ReplaceText("{HinhThucNop}", "do tổ chức dịch vụ bưu chính chuyển đến");
                            }
                            else
                            {
                                document.ReplaceText("{HinhThucNop}", "gửi qua " + (mauInObject.HinhThucGoiDon != null ? mauInObject.HinhThucGoiDon.ToLower() : ".................."));
                            }
                            document.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAn != null ? "bản án" : "quyết định");
                            document.ReplaceText("{SoBAQD}", mauInObject.SoBanAn != null ? mauInObject.SoBanAn : mauInObject.SoQuyetDinh);
                            document.ReplaceText("{NamBAQD}", mauInObject.SoBanAn != null ? mauInObject.NgayRaBanAn.Year.ToString() : mauInObject.NgayRaQuyetDinh.Year.ToString());
                            document.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAn != null ? NgayThangNam(mauInObject.NgayRaBanAn) : NgayThangNam(mauInObject.NgayRaQuyetDinh));
                            document.SaveAs(filePath);
                        }
                    }
                    else
                    {
                        using (DocX temp = DocX.Create(filePathTemp))
                        {
                            temp.ApplyTemplate(templatePath);

                            temp.ReplaceText("{SoKhangCao}", item.KhangCaoID.ToString());
                            temp.ReplaceText("{NamNop}", item.NgayNop.Year.ToString());
                            temp.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.NhomAn == Setting.MANHOMAN_HANHCHINH)
                            {
                                if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                                {
                                    temp.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                                }
                                else
                                {
                                    var tempToaAnTinhParagraph = GetParagraph(temp, "{ToaAnTinh}");
                                    tempToaAnTinhParagraph.Remove(false);
                                }
                            }
                            else
                            {
                                if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                                {
                                    temp.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                                }
                                else
                                {
                                    var tempToaAnTinhParagraph = GetParagraph(temp, "{ToaAnTinh}");
                                    tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                                    tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false).FontSize(12).Bold().Alignment = Alignment.center;
                                }
                            }
                            temp.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            temp.ReplaceText("{NgayNopDon}", NgayThangNam(item.NgayNop));
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                temp.ReplaceText("{NguoiKhangCao}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                temp.ReplaceText("{NguoiKhangCao}", item.TenCoQuanToChuc);
                            }
                            temp.ReplaceText("{DiaChiNguoiKhangCao}", item.NoiTamTru ?? item.NoiDKHKTT);
                            temp.ReplaceText("{NgayDe}", NgayThangNam(item.NgayDe));
                            temp.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            temp.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);
                            temp.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                            if (mauInObject.HinhThucGoiDon == "Trực tiếp")
                            {
                                temp.ReplaceText("{HinhThucNop}", "nộp trực tiếp");
                            }
                            else if (mauInObject.HinhThucGoiDon == "Gián tiếp")
                            {
                                temp.ReplaceText("{HinhThucNop}", "do tổ chức dịch vụ bưu chính chuyển đến");
                            }
                            else
                            {
                                temp.ReplaceText("{HinhThucNop}", "gửi qua " + (mauInObject.HinhThucGoiDon != null ? mauInObject.HinhThucGoiDon.ToLower() : ".................."));
                            }
                            temp.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAn != null ? "bản án" : "quyết định");
                            temp.ReplaceText("{SoBAQD}", mauInObject.SoBanAn != null ? mauInObject.SoBanAn : mauInObject.SoQuyetDinh);
                            temp.ReplaceText("{NamBAQD}", mauInObject.SoBanAn != null ? mauInObject.NgayRaBanAn.Year.ToString() : mauInObject.NgayRaQuyetDinh.Year.ToString());
                            temp.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAn != null ? NgayThangNam(mauInObject.NgayRaBanAn) : NgayThangNam(mauInObject.NgayRaQuyetDinh));
                            temp.SaveAs(filePathTemp);

                            File.Copy(filePath, filePathTemp2);

                            List<Source> sources = new List<Source>();

                            sources.Add(new Source(new WmlDocument(filePathTemp2), true));

                            sources.Add(new Source(new WmlDocument(filePathTemp), true));

                            DocumentBuilder.BuildDocument(sources, filePath);


                            File.Delete(filePathTemp2);
                            File.Delete(filePathTemp);
                        }
                    }
                }


                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInGiayXacNhanKhangCaoDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        //HC
        public string[] MauInHC02Doc(MauInSo24ViewModel mauInObject, string filePath, string templatePath)
        {
            string nguoikhoikien = "";
            try
            {
                if (mauInObject.DanhSachDuongSuViewModel.Any())
                    foreach (var item in mauInObject.DanhSachDuongSuViewModel)
                    {
                        if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                        {
                            nguoikhoikien += (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ", ";
                        }
                        else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                        {
                            nguoikhoikien += item.TenCoQuanToChuc + ", ";
                        }
                    }
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    Xceed.Words.NET.Paragraph danhsachduongsu = GetParagraph(document, "{DanhSachDuongSu}");
                    if (mauInObject.DanhSachDuongSuViewModel.Any())
                        foreach (var item in mauInObject.DanhSachDuongSuViewModel)
                        {
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    danhsachduongsu.ReplaceText("{DanhSachDuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    danhsachduongsu.ReplaceText("{DanhSachDuongSu}", item.TenCoQuanToChuc + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 3.5f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = danhsachduongsu.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 3.5f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 3.5f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = danhsachduongsu.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    p2.IndentationFirstLine = 3.5f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.LastOrDefault()))
                            {
                                var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachDuongSuViewModel.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachDuongSuViewModel.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 3.5f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                            }
                        }


                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo.ToString());
                    document.ReplaceText("{NamNopHS}", mauInObject.NgayNopDonTaiToaAn.Year.ToString());
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayNopDon}", NgayThangNam(mauInObject.NgayNopDonTaiToaAn));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{NgayLamDon}", NgayThangNam(mauInObject.NgayLamDon));
                    document.ReplaceText("{NguoiKhoiKien}", nguoikhoikien);
                    if (mauInObject.HinhThucGoiDon == "Trực tiếp")
                    {
                        document.ReplaceText("{HinhThucNop}", "nộp trực tiếp");
                    }
                    else if (mauInObject.HinhThucGoiDon == "Gián tiếp")
                    {
                        document.ReplaceText("{HinhThucNop}", "do tổ chức dịch vụ bưu chính chuyển đến");
                    }
                    else
                    {
                        document.ReplaceText("{HinhThucNop}", "gửi qua " + (mauInObject.HinhThucGoiDon != null ? mauInObject.HinhThucGoiDon.ToLower() : ".................."));
                    }
                    var noidungdon = StripHtmlTags(mauInObject.NoiDungDon);
                    Xceed.Words.NET.Paragraph NoiDungDon = GetParagraph(document, "{NoiDungDon}");
                    foreach (var item in noidungdon)
                    {
                        if (noidungdon.Count == 1 || item == noidungdon.FirstOrDefault())
                            NoiDungDon.ReplaceText("{NoiDungDon}", item);
                        else
                        {
                            NoiDungDon.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                        }
                    }
                    
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.HoTenNguoiKyXacNhanDaNhanDon);

                    if (mauInObject.ChucVuNguoiKyXacNhan.ToLower() == "chánh án" || mauInObject.ChucDanhNguoiKyXacNhan.ToLower() == "thẩm phán")
                    {
                        document.ReplaceText("{NguoiKy}", mauInObject.ChucDanhNguoiKyXacNhan.ToUpper());
                    }
                    else
                    {
                        var pChucDanhNguoiKyXacNhan = GetParagraph(document, "{NguoiKy}");
                        pChucDanhNguoiKyXacNhan.ReplaceText("{NguoiKy}", "TL.CHÁNH ÁN");
                        pChucDanhNguoiKyXacNhan.AppendLine("KT.CHÁNH VĂN PHÒNG").Bold().Font("Times New Roman").FontSize(13);
                        pChucDanhNguoiKyXacNhan.AppendLine("PHÓ CHÁNH VĂN PHÒNG").Bold().Font("Times New Roman").FontSize(13);
                    }

                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauInHC02Doc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        public string[] MauInHC04Doc(MauInSo29ViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo.ToString());
                    document.ReplaceText("{NamRaThongBao}", mauInObject.NgayRaThongBao.Year.ToString());
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{NguoiDuNop}", (mauInObject.NguoiDuNop.DuongSuLa == Setting.DUONGSULA_CANHAN ? ThemOngBa(mauInObject.NguoiDuNop.GioiTinh, mauInObject.NguoiDuNop.HoTenDuongSu) : mauInObject.NguoiDuNop.TenCoQuanToChuc));
                    document.ReplaceText("{NguoiDuNopThuong}", (mauInObject.NguoiDuNop.DuongSuLa == Setting.DUONGSULA_CANHAN ? ThemOngBa_ChuThuong(mauInObject.NguoiDuNop.GioiTinh, mauInObject.NguoiDuNop.HoTenDuongSu) : mauInObject.NguoiDuNop.TenCoQuanToChuc));
                    document.ReplaceText("{DiaChiNguoiDuDop}", mauInObject.NguoiDuNop.NoiTamTru ?? mauInObject.NguoiDuNop.NoiDKHKTT);
                    document.ReplaceText("{CoQuanThiHanhAnThu}", mauInObject.CoQuanThiHanhAnThu);
                    document.ReplaceText("{DiaChiCoQuanThiHanhAnThu}", mauInObject.DiaChiCoQuanThiHanhAnThu);
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);


                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauInHC04Doc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
            
        }

        public string[] MauInHC06Doc(MauInSo30ViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);
                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph danhsachduongsu = GetParagraph(document, "{DanhSachDuongSu}");
                    Xceed.Words.NET.Paragraph NguoiKhoiKien = GetParagraph(document, "{NguoiKhoiKien}");
                    Xceed.Words.NET.Paragraph NguoiBiKien = GetParagraph(document, "{NguoiBiKien}");
                    Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                    if (mauInObject.DanhSachDuongSuViewModel != null || mauInObject.DanhSachDuongSuViewModel.Any())
                    {
                        foreach (var item in mauInObject.DanhSachDuongSuViewModel)
                        {
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    danhsachduongsu.ReplaceText("{DanhSachDuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    danhsachduongsu.ReplaceText("{DanhSachDuongSu}", item.TenCoQuanToChuc + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 3.5f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = danhsachduongsu.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 3.5f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 3.5f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = danhsachduongsu.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    p2.IndentationFirstLine = 3.5f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.LastOrDefault()))
                            {
                                var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachDuongSuViewModel.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachDuongSuViewModel.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 3.5f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                            }
                        }

                        var kk = mauInObject.DanhSachDuongSuViewModel.Where(x => x.TuCachThamGiaToTung.Equals("Người khởi kiện"));
                        if (kk != null && kk.Any())
                        {
                            foreach (var item in kk)
                            {
                                if (kk.Count() == 1 || item.Equals(kk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiKhoiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiKhoiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (kk.Count() == 1 || item.Equals(kk.LastOrDefault()))
                                {
                                    var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (kk.FirstOrDefault().NoiTamTru ?? kk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var bk = mauInObject.DanhSachDuongSuViewModel.Where(x => x.TuCachThamGiaToTung.Equals("Người bị kiện"));
                        if (bk != null && bk.Any())
                        {
                            foreach (var item in bk)
                            {
                                if (bk.Count() == 1 || item.Equals(bk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiBiKien.ReplaceText("{NguoiBiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiBiKien.ReplaceText("{NguoiBiKien}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiBiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiBiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (bk.Count() == 1 || item.Equals(bk.LastOrDefault()))
                                {
                                    var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (bk.FirstOrDefault().NoiTamTru ?? bk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var lq = mauInObject.DanhSachDuongSuViewModel.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN));
                        if (lq != null && lq.Any())
                        {
                            foreach (var item in lq)
                            {
                                if (lq.Count() == 1 || item.Equals(lq.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (lq.Count() == 1 || item.Equals(lq.LastOrDefault()))
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (lq.FirstOrDefault().NoiTamTru ?? lq.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }
                        else
                        {
                            NguoiLienQuan.ReplaceText("{NguoiLienQuan}", "Không.");
                        }
                    }
                    else
                    {
                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", "Ông (Bà) ..........................................");
                        NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguoiKhoiKien.IndentationFirstLine = 1.0f;

                        NguoiBiKien.ReplaceText("{NguoiBiKien}", "Ông (Bà) ..........................................");
                        NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguoiBiKien.IndentationFirstLine = 1.0f;

                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", "Không.");
                    }
                    //replace    
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy).Replace("ngày", "Ngày"));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{NhomAn}", mauInObject.NhomAn);
                    document.ReplaceText("{KhieuKien}", mauInObject.QuanHePhapLuat);

                    var noidungdon = StripHtmlTags(mauInObject.NoiDungYeuCau);
                    Xceed.Words.NET.Paragraph NoiDungDon = GetParagraph(document, "{NoiDungDon}");
                    foreach (var item in noidungdon)
                    {
                        if (noidungdon.Count == 1 || item == noidungdon.FirstOrDefault())
                            NoiDungDon.ReplaceText("{NoiDungDon}", item);
                        else
                        {
                            NoiDungDon.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                        }
                    }
                    document.ReplaceText("{ThuTuc}", mauInObject.ThuLyTheoThuTuc.ToLower());
                    if(String.IsNullOrEmpty(mauInObject.TaiLieuChungTuKemTheo))
                        document.ReplaceText("{TaiLieuChungTuKemTheo}", "Không");
                    else
                    {
                        var tailieu = StripHtmlTags(mauInObject.TaiLieuChungTuKemTheo);
                        Xceed.Words.NET.Paragraph TaiLieuChungTuKemTheo = GetParagraph(document, "{TaiLieuChungTuKemTheo}");
                        foreach (var item in tailieu)
                        {
                            if (tailieu.Count == 1 || item == tailieu.FirstOrDefault())
                                TaiLieuChungTuKemTheo.ReplaceText("{TaiLieuChungTuKemTheo}", item);
                            else
                            {
                                TaiLieuChungTuKemTheo.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                            }
                        }
                    }                                    
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauInHC06Doc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHCQuyetDinhPCTPHTDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguoiKhoiKien = GetParagraph(document, "{NguoiKhoiKien}");
                    Xceed.Words.NET.Paragraph NguoiBiKien = GetParagraph(document, "{NguoiBiKien}");

                    string thamphandukhuyet = "", hoithamdukhuyet = "";
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {


                        var kk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals("Người khởi kiện"));
                        if (kk != null && kk.Any())
                        {
                            foreach (var item in kk)
                            {
                                if (kk.Count() == 1 || item.Equals(kk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiKhoiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiKhoiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (kk.Count() == 1 || item.Equals(kk.LastOrDefault()))
                                {
                                    var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (kk.FirstOrDefault().NoiTamTru ?? kk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var bk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals("Người bị kiện"));
                        if (bk != null && bk.Any())
                        {
                            foreach (var item in bk)
                            {
                                if (bk.Count() == 1 || item.Equals(bk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiBiKien.ReplaceText("{NguoiBiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiBiKien.ReplaceText("{NguoiBiKien}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiBiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiBiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (bk.Count() == 1 || item.Equals(bk.LastOrDefault()))
                                {
                                    var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (bk.FirstOrDefault().NoiTamTru ?? bk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }
                        if (mauInObject.GiaiDoan == 1)
                        {
                            Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                            var lq = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN));
                            if (lq != null && lq.Any())
                            {
                                foreach (var item in lq)
                                {
                                    if (lq.Count() == 1 || item.Equals(lq.FirstOrDefault()))
                                    {
                                        if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                        {
                                            NguoiLienQuan.ReplaceText("{NguoiLienQuan}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                        }
                                        else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                        {
                                            NguoiLienQuan.ReplaceText("{NguoiLienQuan}", item.TenCoQuanToChuc + ".");
                                        }
                                    }
                                    else
                                    {
                                        if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                        {
                                            var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                            p1.IndentationFirstLine = 1.0f;
                                            //p1.SpacingBefore(6);
                                            p1.SpacingAfter(6);
                                            var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                            p2.IndentationFirstLine = 1.0f;
                                            //p2.SpacingBefore(6);
                                            p2.SpacingAfter(6);

                                        }
                                        else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                        {
                                            var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                            p1.IndentationFirstLine = 1.0f;
                                            //p1.SpacingBefore(6);
                                            p1.SpacingAfter(6);
                                            var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                            p2.IndentationFirstLine = 1.0f;
                                            //p2.SpacingBefore(6);
                                            p2.SpacingAfter(6);
                                        }
                                    }
                                    if (lq.Count() == 1 || item.Equals(lq.LastOrDefault()))
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (lq.FirstOrDefault().NoiTamTru ?? lq.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                    }
                                }
                            }
                            else
                            {
                                NguoiLienQuan.ReplaceText("{NguoiLienQuan}", "Không.");
                            }
                        }

                    }
                    else
                    {
                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", "Ông (Bà) ..........................................");
                        NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguoiKhoiKien.IndentationFirstLine = 1.0f;

                        NguoiBiKien.ReplaceText("{NguoiBiKien}", "Ông (Bà) ..........................................");
                        NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguoiBiKien.IndentationFirstLine = 1.0f;
                        if (mauInObject.GiaiDoan == 1)
                            document.ReplaceText("{NguoiLienQuan}", "Không.");
                    }

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }
                    if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet == null || !mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Any())
                        hoithamdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachHoiThamNhanDanDuKhuyet)
                        {
                            if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachHoiThamNhanDanDuKhuyet.LastOrDefault())
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet) + ", ";
                            else
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet);
                        }
                    }


                    //replace               



                    document.ReplaceText("{HoiThamNhanDan1}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan, mauInObject.HoiThamNhanDan));
                    document.ReplaceText("{HoiThamNhanDan2}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2));
                    document.ReplaceText("{HoiThamDuKhuyet}", hoithamdukhuyet);
                    if (mauInObject.HoiDong == 2 && mauInObject.GiaiDoan == 1)
                    {
                        document.ReplaceText("{ThamPhanKhac}", ThemOngBa(mauInObject.GioiTinhThamPhanKhac, mauInObject.ThamPhanKhac) + ", chức danh Thẩm phán.");
                        document.ReplaceText("{HoiThamNhanDan3}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3) + ", chức danh Hội thẩm.");
                    }
                    else if (mauInObject.GiaiDoan == 1)
                    {
                        var tempParagraph1 = GetParagraph(document, "{ThamPhanKhac}");
                        var tempParagraph2 = GetParagraph(document, "{HoiThamNhanDan3}");
                        tempParagraph1.Remove(false);
                        tempParagraph2.Remove(false);
                    }
                    var nguoiky = GetParagraph(document, "{NguoiKy}");
                    if (mauInObject.LoaiChanhAn.ToLower() == "chánh án")
                    {
                        nguoiky.ReplaceText("{NguoiKy}", mauInObject.LoaiChanhAn.ToUpper());
                    }
                    else
                    {
                        nguoiky.ReplaceText("{NguoiKy}", "KT. CHÁNH ÁN");
                        nguoiky.AppendLine("PHÓ CHÁNH ÁN").Bold().Font("Times New Roman").FontSize(13);
                    }

                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhan, mauInObject.ThamPhan));
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NhomAn}", mauInObject.NhomAn);
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else if (mauInObject.GiaiDoan == 1)
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamPhanCong}", NgayThangNam(mauInObject.NgayPhanCong));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{KhieuKien}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1));
                    document.ReplaceText("{ThamPhan2}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan2));
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ChanhAn);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauInHCQuyetDinhPCTPHTDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHCQuyetDinhDuaVuAnXetXuSoThamDoc(MauInSo47ViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguoiKhoiKien = GetParagraph(document, "{NguoiKhoiKien}");
                    Xceed.Words.NET.Paragraph NguoiBiKien = GetParagraph(document, "{NguoiBiKien}");
                    Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                    string thamphandukhuyet = "", hoithamdukhuyet = "", thukydukhuyet = "", ksvdukhuyet = "";
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {


                        var kk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals("Người khởi kiện"));
                        if (kk != null && kk.Any())
                        {
                            foreach (var item in kk)
                            {
                                if (kk.Count() == 1 || item.Equals(kk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiKhoiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiKhoiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (kk.Count() == 1 || item.Equals(kk.LastOrDefault()))
                                {
                                    var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (kk.FirstOrDefault().NoiTamTru ?? kk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var bk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals("Người bị kiện"));
                        if (bk != null && bk.Any())
                        {
                            foreach (var item in bk)
                            {
                                if (bk.Count() == 1 || item.Equals(bk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiBiKien.ReplaceText("{NguoiBiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiBiKien.ReplaceText("{NguoiBiKien}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiBiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiBiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (bk.Count() == 1 || item.Equals(bk.LastOrDefault()))
                                {
                                    var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (bk.FirstOrDefault().NoiTamTru ?? bk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var lq = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN));
                        if (lq != null && lq.Any())
                        {
                            foreach (var item in lq)
                            {
                                if (lq.Count() == 1 || item.Equals(lq.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (lq.Count() == 1 || item.Equals(lq.LastOrDefault()))
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (lq.FirstOrDefault().NoiTamTru ?? lq.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }
                        else
                        {
                            NguoiLienQuan.ReplaceText("{NguoiLienQuan}", "Không.");
                        }
                    }
                    else
                    {
                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", "Ông (Bà) ..........................................");
                        NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguoiKhoiKien.IndentationFirstLine = 1.0f;

                        NguoiBiKien.ReplaceText("{NguoiBiKien}", "Ông (Bà) ..........................................");
                        NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguoiBiKien.IndentationFirstLine = 1.0f;

                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", "Không.");
                    }

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet == null || !mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Any())
                        hoithamdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachHoiThamNhanDanDuKhuyet)
                        {
                            if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachHoiThamNhanDanDuKhuyet.LastOrDefault())
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet) + ", ";
                            else
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachThuKyDuKhuyet == null || !mauInObject.DanhSachThuKyDuKhuyet.Any())
                        thukydukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThuKyDuKhuyet)
                        {
                            if (mauInObject.DanhSachThuKyDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThuKyDuKhuyet.LastOrDefault())
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet) + ", ";
                            else
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachKiemSatVienDuKhuyet == null || !mauInObject.DanhSachKiemSatVienDuKhuyet.Any())
                        ksvdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachKiemSatVienDuKhuyet)
                        {
                            if (mauInObject.DanhSachKiemSatVienDuKhuyet.Count() > 1 && item != mauInObject.DanhSachKiemSatVienDuKhuyet.LastOrDefault())
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet) + ", ";
                            else
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet);
                        }
                    }
                    //replace               



                    document.ReplaceText("{HoiThamNhanDan1}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan, mauInObject.HoiThamNhanDan));
                    document.ReplaceText("{HoiThamNhanDan2}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2));
                    document.ReplaceText("{HoiThamDuKhuyet}", hoithamdukhuyet);
                    if (mauInObject.HoiDong == 2)
                    {
                        document.ReplaceText("{ThamPhanKhac}", ThemOngBa(mauInObject.GioiTinhThamPhanKhac, mauInObject.ThamPhanKhac) + ", chức danh Thẩm phán.");
                        document.ReplaceText("{HoiThamNhanDan3}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3) + ", chức danh Hội thẩm.");
                    }
                    else
                    {
                        var tempParagraph1 = GetParagraph(document, "{ThamPhanKhac}");
                        if (mauInObject.ThuTuc != Setting.THUTUC_RUTGON)
                        {
                            var tempParagraph2 = GetParagraph(document, "{HoiThamNhanDan3}");
                            tempParagraph2.Remove(false);
                        }                            
                        tempParagraph1.Remove(false);
                        
                    }
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{HoiThamDuKhuyet}", hoithamdukhuyet);
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{ThuKyDuKhuyet}", thukydukhuyet);
                    document.ReplaceText("{KiemSatVien}", ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien));
                    document.ReplaceText("{KiemSatVienDuKhuyet}", ksvdukhuyet);
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                    document.ReplaceText("{NamXetXu}", mauInObject.NgayRaQuyetDinhXetXu.Year.ToString());
                    document.ReplaceText("{NgayXetXu}", NgayThangNam(mauInObject.NgayRaQuyetDinhXetXu));
                    document.ReplaceText("{KhieuKien}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{DiaDiemMoPhienToa}", mauInObject.DiaDiemMoPhienToa);
                    document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                    document.ReplaceText("{VuAnDuocXetXu}", mauInObject.VuAnDuocXetXu.ToLower());
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhanChuToa);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauInHCQuyetDinhDuaVuAnXetXuSoThamDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHCGiayTrieuTapDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath)
        {
            string filePathTemp = filePath.Replace(".docx", "_temp_.docx");
            string filePathTemp2 = filePath.Replace(".docx", "_temp2_.docx");
            try
            {
                string duongsudauvu = mauInObject.TenVuAn;
                if (duongsudauvu.Count(x => x == '-') > 1)
                    duongsudauvu = duongsudauvu.Remove(duongsudauvu.LastIndexOf(" - "));
                foreach (var item in mauInObject.DanhSachDuongSu)
                {
                    if (item == mauInObject.DanhSachDuongSu.FirstOrDefault() || mauInObject.DanhSachDuongSu.Count == 1)
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            document.ApplyTemplate(templatePath);
                            document.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            document.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            document.ReplaceText("{TuCachThamGiaToTung}", item.TuCachThamGiaToTung);
                            document.ReplaceText("{KhieuKien}", mauInObject.QuanHePhapLuat);
                            document.ReplaceText("{NguoiKhoiKien}", mauInObject.TenNguyenDon);
                            document.ReplaceText("{NguoiBiKien}", mauInObject.TenBiDon);
                            document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            document.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                            document.ReplaceText("{NamXetXu}", mauInObject.ThoiGianMoPhienToa.Year.ToString());
                            document.ReplaceText("{TenVuAn}", duongsudauvu);
                            document.ReplaceText("{TenTP}", mauInObject.ThamPhan.Substring(mauInObject.ThamPhan.LastIndexOf(" ")+1));

                            document.SaveAs(filePath);
                        }
                    }
                    else
                    {
                        using (DocX temp = DocX.Create(filePathTemp))
                        {
                            temp.ApplyTemplate(templatePath);

                            temp.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            temp.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            temp.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                temp.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(temp, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            temp.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            temp.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                temp.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                temp.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                            }
                            temp.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            temp.ReplaceText("{TuCachThamGiaToTung}", item.TuCachThamGiaToTung);
                            temp.ReplaceText("{KhieuKien}", mauInObject.QuanHePhapLuat);
                            temp.ReplaceText("{NguoiKhoiKien}", mauInObject.TenNguyenDon);
                            temp.ReplaceText("{NguoiBiKien}", mauInObject.TenBiDon);
                            temp.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            temp.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            temp.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            temp.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            temp.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                            temp.ReplaceText("{NamXetXu}", mauInObject.ThoiGianMoPhienToa.Year.ToString());
                            temp.ReplaceText("{TenVuAn}", duongsudauvu);
                            temp.ReplaceText("{TenTP}", mauInObject.ThamPhan.Substring(mauInObject.ThamPhan.LastIndexOf(" ") + 1));
                            temp.SaveAs(filePathTemp);

                            File.Copy(filePath, filePathTemp2);

                            List<Source> sources = new List<Source>();

                            sources.Add(new Source(new WmlDocument(filePathTemp2), true));

                            sources.Add(new Source(new WmlDocument(filePathTemp), true));

                            DocumentBuilder.BuildDocument(sources, filePath);


                            File.Delete(filePathTemp2);
                            File.Delete(filePathTemp);
                        }
                    }
                }


                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauInGiayTrieuTapDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
                
        public string[] MauInHC31Doc(MauInSo61ViewModel mauInObject, string filePath, string templatePath)
        {  
            try
            {
                string filePathTemp = filePath.Replace(".docx", "_temp_.docx");
                string filePathTemp2 = filePath.Replace(".docx", "_temp2_.docx");
                // Create a new document.
                foreach (var item in mauInObject.DanhSachNguoiKhangCao)
                {
                    if(mauInObject.DanhSachNguoiKhangCao.Count==1 || item == mauInObject.DanhSachNguoiKhangCao.FirstOrDefault())
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            // Apply a template to the document based on a path.
                            document.ApplyTemplate(templatePath);

                            Formatting format = new Formatting
                            {
                                FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                                Size = 14
                            };
                            
                            //replace
                            document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                            document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo.ToString());
                            document.ReplaceText("{NamRaThongBao}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{NguoiKhangCao}", ThemOngBa_ChuThuong(item.GioiTinh, item.HoTenDuongSu));
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{NguoiKhangCao}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{CoQuanThiHanhAnThu}", mauInObject.CoQuanThiHanhAnThu);
                            document.ReplaceText("{DiaChiCoQuanThiHanhAnThu}", mauInObject.DiaChiCoQuanThiHanhAnThu);
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);


                            // Save this document to disk.
                            document.SaveAs(filePath);
                            //}
                        }
                    }
                    else
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            // Apply a template to the document based on a path.
                            document.ApplyTemplate(templatePath);

                            Formatting format = new Formatting
                            {
                                FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                                Size = 14
                            };

                            //replace
                            document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                            document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo.ToString());
                            document.ReplaceText("{NamRaThongBao}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{NguoiKhangCao}", ThemOngBa_ChuThuong(item.GioiTinh, item.HoTenDuongSu));
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{NguoiKhangCao}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{CoQuanThiHanhAnThu}", mauInObject.CoQuanThiHanhAnThu);
                            document.ReplaceText("{DiaChiCoQuanThiHanhAnThu}", mauInObject.DiaChiCoQuanThiHanhAnThu);
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);


                            // Save this document to disk.
                            document.SaveAs(filePathTemp);

                            File.Copy(filePath, filePathTemp2);

                            List<Source> sources = new List<Source>();

                            sources.Add(new Source(new WmlDocument(filePathTemp2), true));

                            sources.Add(new Source(new WmlDocument(filePathTemp), true));

                            DocumentBuilder.BuildDocument(sources, filePath);


                            File.Delete(filePathTemp2);
                            File.Delete(filePathTemp);
                            //}
                        }
                    }
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauInHC31Doc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHC35Doc(MauInSo65ViewModel mauInObject, string filePath, string templatePath)
        {
            string tailieu = "", noidungkc="";
            try
            {
                foreach (var item in mauInObject.NguoiKhangCao)
                {
                    tailieu += item.TaiLieuChungTuKemTheo;
                }
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);
                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph DanhSachDuongSu = GetParagraph(document, "{DanhSachDuongSu}");
                    Xceed.Words.NET.Paragraph NguoiKhangCaoKhangNghi = GetParagraph(document, "{NguoiKhangCaoKhangNghi}");
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {
                        foreach (var item in mauInObject.DanhSachDuongSu)
                        {
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    DanhSachDuongSu.ReplaceText("{DanhSachDuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    DanhSachDuongSu.ReplaceText("{DanhSachDuongSu}", item.TenCoQuanToChuc + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = DanhSachDuongSu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 3.5f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = DanhSachDuongSu.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 3.5f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = DanhSachDuongSu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 3.5f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = DanhSachDuongSu.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    p2.IndentationFirstLine = 3.5f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.LastOrDefault()))
                            {
                                var p1 = DanhSachDuongSu.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachDuongSu.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachDuongSu.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 3.5f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                            }
                        }
                    }
                    else
                    {
                        DanhSachDuongSu.ReplaceText("{DanhSachDuongSu}", "Ông (Bà) ..........................................");
                        DanhSachDuongSu.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        DanhSachDuongSu.IndentationFirstLine = 1.0f;
                    }
                    if (mauInObject.VienKiemSatKhangNghi != null)
                    {
                        NguoiKhangCaoKhangNghi.ReplaceText("{NguoiKhangCaoKhangNghi}", mauInObject.VienKiemSatKhangNghi + ".");
                        noidungkc += mauInObject.NoiDungKhangNghi;
                    }
                    else
                    {
                        NguoiKhangCaoKhangNghi.ReplaceText("{NguoiKhangCaoKhangNghi}", "");
                    }
                    if (mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any())
                    {
                        foreach (var item in mauInObject.NguoiKhangCao)
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                            noidungkc += item.NoidungKhangCao;
                        }
                    }


                    //replace    
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy).Replace("ngày", "Ngày"));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{ToaAnST}", mauInObject.ToaAnSoTham);
                    document.ReplaceText("{KhieuKien}", mauInObject.QuanHePhapLuat);
                    if(String.IsNullOrEmpty(noidungkc))
                        document.ReplaceText("{NoiDungKhangCaoKhangNghi}", "");
                    else
                    {
                        var noiDungKhangCaoKhangNghi = StripHtmlTags(noidungkc);
                        Xceed.Words.NET.Paragraph NoiDungKhangCaoKhangNghi = GetParagraph(document, "{NoiDungKhangCaoKhangNghi}");
                        foreach (var item in noiDungKhangCaoKhangNghi)
                        {
                            if (noiDungKhangCaoKhangNghi.Count == 1 || item == noiDungKhangCaoKhangNghi.FirstOrDefault())
                                NoiDungKhangCaoKhangNghi.ReplaceText("{NoiDungKhangCaoKhangNghi}", item);
                            else
                            {
                                NoiDungKhangCaoKhangNghi.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                            }
                        }
                    }


                    if (String.IsNullOrEmpty(tailieu))
                        document.ReplaceText("{TaiLieuChungTuKemTheo}", "Không.");
                    else
                    {
                        var taiLieuChungTuKemTheo = StripHtmlTags(tailieu);
                        Xceed.Words.NET.Paragraph TaiLieuChungTuKemTheo = GetParagraph(document, "{TaiLieuChungTuKemTheo}");
                        foreach (var item in taiLieuChungTuKemTheo)
                        {
                            if (taiLieuChungTuKemTheo.Count == 1 || item == taiLieuChungTuKemTheo.FirstOrDefault())
                                TaiLieuChungTuKemTheo.ReplaceText("{TaiLieuChungTuKemTheo}", item);
                            else
                            {
                                TaiLieuChungTuKemTheo.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                            }
                        }
                    }

                    
                    document.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAn != null ? "bản án" : "quyết định");
                    document.ReplaceText("{SoBAQD}", mauInObject.SoBanAn != null ? mauInObject.SoBanAn : mauInObject.SoQuyetDinh);
                    document.ReplaceText("{NamBAQD}", mauInObject.SoBanAn != null ? mauInObject.NgayTuyenAn.Year.ToString() : mauInObject.NgayRaQuyetDinh.Year.ToString());
                    document.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAn != null ? NgayThangNam(mauInObject.NgayTuyenAn) : NgayThangNam(mauInObject.NgayRaQuyetDinh));
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauInHC35Doc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        
        public string[] MauInHCQuyetDinhDuaVuAnXetXuPhucThamDoc(MauInSo47PTViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguoiKhoiKien = GetParagraph(document, "{NguoiKhoiKien}");
                    Xceed.Words.NET.Paragraph NguoiBiKien = GetParagraph(document, "{NguoiBiKien}");
                    Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                    Xceed.Words.NET.Paragraph NguoiKhangCaoKhangNghi = GetParagraph(document, "{NguoiKhangCaoKhangNghi}");
                    string thamphandukhuyet = "", thukydukhuyet = "", ksvdukhuyet = "";
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {
                        var kk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals("Người khởi kiện"));
                        if (kk != null && kk.Any())
                        {
                            foreach (var item in kk)
                            {
                                if (kk.Count() == 1 || item.Equals(kk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiKhoiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiKhoiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (kk.Count() == 1 || item.Equals(kk.LastOrDefault()))
                                {
                                    var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (kk.FirstOrDefault().NoiTamTru ?? kk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var bk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals("Người bị kiện"));
                        if (bk != null && bk.Any())
                        {
                            foreach (var item in bk)
                            {
                                if (bk.Count() == 1 || item.Equals(bk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiBiKien.ReplaceText("{NguoiBiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiBiKien.ReplaceText("{NguoiBiKien}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiBiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiBiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (bk.Count() == 1 || item.Equals(bk.LastOrDefault()))
                                {
                                    var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (bk.FirstOrDefault().NoiTamTru ?? bk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var lq = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN));
                        if (lq != null && lq.Any())
                        {
                            foreach (var item in lq)
                            {
                                if (lq.Count() == 1 || item.Equals(lq.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (lq.Count() == 1 || item.Equals(lq.LastOrDefault()))
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (lq.FirstOrDefault().NoiTamTru ?? lq.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }
                        else
                        {
                            NguoiLienQuan.ReplaceText("{NguoiLienQuan}", "Không.");
                        }
                    }
                    else
                    {
                        NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", "Ông (Bà) ..........................................");
                        NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguoiKhoiKien.IndentationFirstLine = 1.0f;

                        NguoiBiKien.ReplaceText("{NguoiBiKien}", "Ông (Bà) ..........................................");
                        NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguoiBiKien.IndentationFirstLine = 1.0f;

                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", "Không.");
                    }

                    if (mauInObject.VienKiemSatKhangNghi != null && (mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any()))
                    {
                        document.ReplaceText("{KhangCao/KhangNghi}", "kháng cáo, kháng nghị");
                    }
                    else if ((mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any()) && mauInObject.VienKiemSatKhangNghi == null)
                    {
                        document.ReplaceText("{KhangCao/KhangNghi}", "kháng cáo");
                    }
                    else
                    {
                        document.ReplaceText("{KhangCao/KhangNghi}", "kháng nghị");
                    }

                    if (mauInObject.VienKiemSatKhangNghi != null)
                    {
                        NguoiKhangCaoKhangNghi.ReplaceText("{NguoiKhangCaoKhangNghi}", mauInObject.VienKiemSatKhangNghi + ".");
                    }
                    else
                    {
                        NguoiKhangCaoKhangNghi.ReplaceText("{NguoiKhangCaoKhangNghi}", "");
                    }
                    if (mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any())
                    {
                        foreach (var item in mauInObject.NguoiKhangCao)
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                        }
                    }

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachThuKyDuKhuyet == null || !mauInObject.DanhSachThuKyDuKhuyet.Any())
                        thukydukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThuKyDuKhuyet)
                        {
                            if (mauInObject.DanhSachThuKyDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThuKyDuKhuyet.LastOrDefault())
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet) + ", ";
                            else
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachKiemSatVienDuKhuyet == null || !mauInObject.DanhSachKiemSatVienDuKhuyet.Any())
                        ksvdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachKiemSatVienDuKhuyet)
                        {
                            if (mauInObject.DanhSachKiemSatVienDuKhuyet.Count() > 1 && item != mauInObject.DanhSachKiemSatVienDuKhuyet.LastOrDefault())
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet) + ", ";
                            else
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet);
                        }
                    }

                    //replace               

                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1));
                    document.ReplaceText("{ThamPhan2}", ThemOngBa(mauInObject.GioiTinhThamPhan2, mauInObject.ThamPhan2));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{ThuKyDuKhuyet}", thukydukhuyet);
                    document.ReplaceText("{KiemSatVien}", ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien));
                    document.ReplaceText("{KiemSatVienDuKhuyet}", ksvdukhuyet);
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                    document.ReplaceText("{NamXetXu}", mauInObject.NgayRaQuyetDinhXetXu.Year.ToString());
                    document.ReplaceText("{NgayXetXu}", NgayThangNam(mauInObject.NgayRaQuyetDinhXetXu));
                    document.ReplaceText("{KhieuKien}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{DiaDiemMoPhienToa}", mauInObject.DiaDiemMoPhienToa);
                    document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                    document.ReplaceText("{VuAnDuocXetXu}", mauInObject.VuAnDuocXetXu.ToLower());
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhanChuToa);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauInHCQuyetDinhDuaVuAnXetXuPhucThamDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHCBiaHoSoNhanDonDoc(MauInBiaHoSoNhanDonViewModel  mauInObject, string filePath, string templatePath)
        {
            
            try
            {
                
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    Xceed.Words.NET.Paragraph NguoiKhoiKien = GetParagraph(document, "{NguoiKhoiKien}");
                    Xceed.Words.NET.Paragraph NguoiBiKien = GetParagraph(document, "{NguoiBiKien}");
                    Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                    Xceed.Words.NET.Paragraph NguoiLienQuan2 = GetParagraph(document, "{NguoiLienQuan2}");
                    foreach (var item in mauInObject.DanhSachNguyenDon)
                    {
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (item.SoDienThoai == null)
                            item.SoDienThoai = "...................";
                        if (mauInObject.DanhSachNguyenDon.Count() == 1 || item.Equals(mauInObject.DanhSachNguyenDon.FirstOrDefault()))
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", item.TenCoQuanToChuc + ".");
                            }
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                NguoiKhoiKien.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                NguoiKhoiKien.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count()>4 ? "Ngày sinh: " : "Năm sinh: ")+item.NgayThangNamSinh, false, format);                                
                                var p2 = NguoiKhoiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                //p2.SpacingBefore(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                NguoiKhoiKien.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                var p2 = NguoiKhoiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                //p2.SpacingBefore(6);
                            }
                        }
                        if (mauInObject.DanhSachNguyenDon.Count() == 1 || item.Equals(mauInObject.DanhSachNguyenDon.LastOrDefault()))
                        {
                            var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachNguyenDon.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachNguyenDon.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                            p1.SpacingAfter(6);
                            NguoiKhoiKien.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                            if (mauInObject.DanhSachNguyenDon.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguoiKhoiKien.InsertParagraphAfterSelf((mauInObject.DanhSachNguyenDon.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + mauInObject.DanhSachNguyenDon.FirstOrDefault().NgayThangNamSinh, false, format);
                            }                            
                        }
                    }

                    foreach (var item in mauInObject.DanhSachBiDon)
                    {
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (item.SoDienThoai == null)
                            item.SoDienThoai = "...................";
                        if (mauInObject.DanhSachBiDon.Count() == 1 || item.Equals(mauInObject.DanhSachBiDon.FirstOrDefault()))
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguoiBiKien.ReplaceText("{NguoiBiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                NguoiBiKien.ReplaceText("{NguoiBiKien}", item.TenCoQuanToChuc + ".");
                            }
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                NguoiBiKien.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                NguoiBiKien.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh+".", false, format);
                                var p2 = NguoiBiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                //p2.SpacingBefore(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                NguoiBiKien.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                var p2 = NguoiBiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                //p2.SpacingBefore(6);
                            }
                        }
                        if (mauInObject.DanhSachBiDon.Count() == 1 || item.Equals(mauInObject.DanhSachBiDon.LastOrDefault()))
                        {
                            var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachBiDon.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachBiDon.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                            p1.SpacingAfter(6);
                            NguoiBiKien.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                            if (mauInObject.DanhSachBiDon.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguoiBiKien.InsertParagraphAfterSelf((mauInObject.DanhSachBiDon.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + mauInObject.DanhSachBiDon.FirstOrDefault().NgayThangNamSinh + ".", false, format);
                            }                            
                        }
                    }

                    
                    if(mauInObject.DanhSachNguoiLienQuanTranhChap!=null && mauInObject.DanhSachNguoiLienQuanTranhChap.Any())
                    {
                        var lq = mauInObject.DanhSachNguoiLienQuanTranhChap.ToList();
                        var lq2 = new List<DuongSuModel>();
                        if (lq.Count > 1)
                            for (int i = 0; i < lq.Count; i++)
                            {
                                if (i % 2 != 0)
                                {
                                    lq2.Add(lq[i]);
                                    lq.Remove(lq[i]);
                                }
                            }

                        foreach (var item in lq)
                        {
                            if (item.NgayThangNamSinh == null)
                                item.NgayThangNamSinh = "..........";
                            if (item.SoDienThoai == null)
                                item.SoDienThoai = "...................";
                            if (lq.Count() == 1 || item.Equals(lq.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    NguoiLienQuan.ReplaceText("{NguoiLienQuan}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    NguoiLienQuan.ReplaceText("{NguoiLienQuan}", item.TenCoQuanToChuc + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.SpacingAfter(6);
                                    NguoiLienQuan.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                    NguoiLienQuan.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    //p2.SpacingBefore(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.SpacingAfter(6);
                                    NguoiLienQuan.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    //p2.SpacingBefore(6);
                                }
                            }
                            if (lq.Count() == 1 || item.Equals(lq.LastOrDefault()))
                            {
                                var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (lq.FirstOrDefault().NoiTamTru ?? lq.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                NguoiLienQuan.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                if (lq.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    NguoiLienQuan.InsertParagraphAfterSelf((lq.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + lq.FirstOrDefault().NgayThangNamSinh, false, format);
                                }
                            }
                        }

                        if (lq2.Count > 0)
                            foreach (var item in lq2)
                            {
                                if (item.NgayThangNamSinh == null)
                                    item.NgayThangNamSinh = "..........";
                                if (item.SoDienThoai == null)
                                    item.SoDienThoai = "...................";
                                if (lq2.Count() == 1 || item.Equals(lq2.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiLienQuan2.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.SpacingAfter(6);
                                        NguoiLienQuan2.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                        NguoiLienQuan2.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                        var p2 = NguoiLienQuan2.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        //p2.SpacingBefore(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiLienQuan2.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.SpacingAfter(6);
                                        NguoiLienQuan2.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                        var p2 = NguoiLienQuan2.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        //p2.SpacingBefore(6);
                                    }
                                }
                                if (lq2.Count() == 1 || item.Equals(lq2.LastOrDefault()))
                                {
                                    var p1 = NguoiLienQuan2.InsertParagraphAfterSelf("Địa chỉ: " + (lq2.FirstOrDefault().NoiTamTru ?? lq2.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.SpacingAfter(6);
                                    NguoiLienQuan2.InsertParagraphAfterSelf("Số điện thoại: " + item.SoDienThoai + ".", false, format);
                                    if (lq2.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiLienQuan2.InsertParagraphAfterSelf((lq2.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + lq2.FirstOrDefault().NgayThangNamSinh, false, format);
                                    }
                                }
                            }
                        else
                            NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", "");
                    }
                    else
                    {
                        NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", "");
                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", "");
                    }

                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{KhieuKien}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{NgayNhanDon}", NgayThangNam(mauInObject.NgayNhanDon).Replace("ngày","Ngày"));
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa,mauInObject.ThamPhanChuToa));


                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHCBiaHoSoNhanDonDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        public string[] MauInHCBiaHoSoDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath)
        {

            try
            {
                string hoitham = ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan1, mauInObject.HoiThamNhanDan1) + ", " + ThemOngBa_ChuThuong(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2);
                if (mauInObject.HoiDong==2)
                    hoitham += ", " + ThemOngBa_ChuThuong(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3);
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    Xceed.Words.NET.Paragraph NguoiKhoiKien = GetParagraph(document, "{NguoiKhoiKien}");
                    Xceed.Words.NET.Paragraph NguoiBiKien = GetParagraph(document, "{NguoiBiKien}");
                    Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                    Xceed.Words.NET.Paragraph NguoiLienQuan2 = GetParagraph(document, "{NguoiLienQuan2}");
                    foreach (var item in mauInObject.DanhSachNguyenDon)
                    {
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (mauInObject.DanhSachNguyenDon.Count() == 1 || item.Equals(mauInObject.DanhSachNguyenDon.FirstOrDefault()))
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                NguoiKhoiKien.ReplaceText("{NguoiKhoiKien}", item.TenCoQuanToChuc + ".");
                            }
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                NguoiKhoiKien.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                var p2 = NguoiKhoiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                //p2.SpacingBefore(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhoiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                //p2.SpacingBefore(6);
                            }
                        }
                        if (mauInObject.DanhSachNguyenDon.Count() == 1 || item.Equals(mauInObject.DanhSachNguyenDon.LastOrDefault()))
                        {
                            var p1 = NguoiKhoiKien.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachNguyenDon.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachNguyenDon.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                            p1.SpacingAfter(6);
                            if (mauInObject.DanhSachNguyenDon.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguoiKhoiKien.InsertParagraphAfterSelf((mauInObject.DanhSachNguyenDon.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + mauInObject.DanhSachNguyenDon.FirstOrDefault().NgayThangNamSinh, false, format);
                            }
                        }
                    }

                    foreach (var item in mauInObject.DanhSachBiDon)
                    {
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (mauInObject.DanhSachBiDon.Count() == 1 || item.Equals(mauInObject.DanhSachBiDon.FirstOrDefault()))
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguoiBiKien.ReplaceText("{NguoiBiKien}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                NguoiBiKien.ReplaceText("{NguoiBiKien}", item.TenCoQuanToChuc + ".");
                            }
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                NguoiBiKien.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                var p2 = NguoiBiKien.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                //p2.SpacingBefore(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiKien.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                //p2.SpacingBefore(6);
                            }
                        }
                        if (mauInObject.DanhSachBiDon.Count() == 1 || item.Equals(mauInObject.DanhSachBiDon.LastOrDefault()))
                        {
                            var p1 = NguoiBiKien.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachBiDon.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachBiDon.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                            p1.SpacingAfter(6);
                            if (mauInObject.DanhSachBiDon.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguoiBiKien.InsertParagraphAfterSelf((mauInObject.DanhSachBiDon.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + mauInObject.DanhSachBiDon.FirstOrDefault().NgayThangNamSinh, false, format);
                            }
                        }
                    }


                    if (mauInObject.DanhSachNguoiLienQuanTranhChap != null && mauInObject.DanhSachNguoiLienQuanTranhChap.Any())
                    {
                        var lq = mauInObject.DanhSachNguoiLienQuanTranhChap.ToList();
                        var lq2 = new List<DuongSuModel>();
                        if (lq.Count > 1)
                            for (int i = 0; i < lq.Count; i++)
                            {
                                if (i % 2 != 0)
                                {
                                    lq2.Add(lq[i]);
                                    lq.Remove(lq[i]);
                                }
                            }

                        foreach (var item in lq)
                        {
                            if (item.NgayThangNamSinh == null)
                                item.NgayThangNamSinh = "..........";
                            if (lq.Count() == 1 || item.Equals(lq.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    NguoiLienQuan.ReplaceText("{NguoiLienQuan}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    NguoiLienQuan.ReplaceText("{NguoiLienQuan}", item.TenCoQuanToChuc + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.SpacingAfter(6);
                                    NguoiLienQuan.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    //p2.SpacingBefore(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.SpacingAfter(6);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    //p2.SpacingBefore(6);
                                }
                            }
                            if (lq.Count() == 1 || item.Equals(lq.LastOrDefault()))
                            {
                                var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (lq.FirstOrDefault().NoiTamTru ?? lq.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                if (lq.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    NguoiLienQuan.InsertParagraphAfterSelf((lq.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + lq.FirstOrDefault().NgayThangNamSinh, false, format);
                                }
                            }
                        }

                        if (lq2.Count > 0)
                            foreach (var item in lq2)
                            {
                                if (item.NgayThangNamSinh == null)
                                    item.NgayThangNamSinh = "..........";
                                
                                if (lq2.Count() == 1 || item.Equals(lq2.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiLienQuan2.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.SpacingAfter(6);
                                        NguoiLienQuan2.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                        var p2 = NguoiLienQuan2.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        //p2.SpacingBefore(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiLienQuan2.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan2.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        //p2.SpacingBefore(6);
                                    }
                                }
                                if (lq2.Count() == 1 || item.Equals(lq2.LastOrDefault()))
                                {
                                    var p1 = NguoiLienQuan2.InsertParagraphAfterSelf("Địa chỉ: " + (lq2.FirstOrDefault().NoiTamTru ?? lq2.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.SpacingAfter(6);
                                    if (lq2.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiLienQuan2.InsertParagraphAfterSelf((lq2.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + lq2.FirstOrDefault().NgayThangNamSinh, false, format);
                                    }
                                }
                            }
                        else
                            NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", "");
                    }
                    else
                    {
                        NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", "");
                        NguoiLienQuan.ReplaceText("{NguoiLienQuan}", "");
                    }

                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{KhieuKien}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{HoiTham}", hoitham);
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{KiemSatVien}", ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien));

                    if(mauInObject.GiaiDoan!=2)
                    {
                        document.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAn != null ? "Bản án" : "Quyết định");
                        document.ReplaceText("{SoBAQD}", mauInObject.SoBanAn != null ? mauInObject.SoBanAn : mauInObject.SoQuyetDinh);
                        document.ReplaceText("{NamBAQD}", mauInObject.SoBanAn != null ? mauInObject.NgayRaBanAn.Year.ToString() : mauInObject.NgayRaQuyetDinh.Year.ToString());
                        document.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAn != null ? NgayThangNam(mauInObject.NgayRaBanAn) : NgayThangNam(mauInObject.NgayRaQuyetDinh));
                    }
                    else
                    {
                        document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1));
                        document.ReplaceText("{ThamPhan2}", ThemOngBa(mauInObject.GioiTinhThamPhan2, mauInObject.ThamPhan2));
                        document.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAnPT != null ? "Bản án" : "Quyết định");
                        document.ReplaceText("{SoBAQD}", mauInObject.SoBanAnPT != null ? mauInObject.SoBanAnPT : mauInObject.SoQuyetDinhPT);
                        document.ReplaceText("{NamBAQD}", mauInObject.SoBanAnPT != null ? mauInObject.NgayRaBanAnPT.Year.ToString() : mauInObject.NgayRaQuyetDinhPT.Year.ToString());
                        document.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAnPT != null ? NgayThangNam(mauInObject.NgayRaBanAnPT) : NgayThangNam(mauInObject.NgayRaQuyetDinhPT));
                    }
                    if (mauInObject.DanhSachNguoiKhangCao != null && mauInObject.DanhSachNguoiKhangCao.Any())
                        document.ReplaceText("{NgayKhangCao}", NgayThangNam(mauInObject.DanhSachNguoiKhangCao.LastOrDefault().NgayNop).Replace("ngày", "Ngày"));
                    else
                        document.ReplaceText("{NgayKhangCao}", "Ngày......tháng.......năm..........");
                    if (mauInObject.VienKiemSatKhangNghi!=null)
                        document.ReplaceText("{NgayKhangNghi}", NgayThangNam(mauInObject.NgayKhangNghi).Replace("ngày", "Ngày"));
                    else
                        document.ReplaceText("{NgayKhangNghi}", "Ngày......tháng.......năm..........");
                    if(mauInObject.VienKiemSatKhangNghi == null && (mauInObject.DanhSachNguoiKhangCao == null || !mauInObject.DanhSachNguoiKhangCao.Any()))
                        document.ReplaceText("{NgayHieuLuc}",NgayThangNam(mauInObject.HieuLuc));
                    else
                        document.ReplaceText("{NgayHieuLuc}", "ngày......tháng.......năm..........");
                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHCBiaHoSoDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        //HS
        public string[] MauInHSQuyetDinhPCTPDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    string thamphandukhuyet = "";
                    

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }

                    var bicaodauvu = mauInObject.DanhSachBiCao.FirstOrDefault();
                    //replace  
                    
                    if (mauInObject.HoiDong == 2 && mauInObject.GiaiDoan == 1)
                    {
                        document.ReplaceText("{ThamPhanKhac}", ThemOngBa(mauInObject.GioiTinhThamPhanKhac, mauInObject.ThamPhanKhac) + " – Thẩm phán.");
                    }
                    else
                    {
                        var tempParagraph1 = GetParagraph(document, "{ThamPhanKhac}");
                        tempParagraph1.Remove(false);
                    }
                    var nguoiky = GetParagraph(document, "{NguoiKy}");
                    if (mauInObject.LoaiChanhAn.ToLower() == "chánh án")
                    {
                        nguoiky.ReplaceText("{NguoiKy}", mauInObject.LoaiChanhAn.ToUpper());
                    }
                    else
                    {
                        nguoiky.ReplaceText("{NguoiKy}", "KT. CHÁNH ÁN");
                        nguoiky.AppendLine("PHÓ CHÁNH ÁN").Bold().Font("Times New Roman").FontSize(13);
                    }

                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhan, mauInObject.ThamPhan));
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else if (mauInObject.GiaiDoan == 1)
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamPhanCong}", NgayThangNam(mauInObject.NgayPhanCong));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{DieuLuat}", mauInObject.DieuLuat);
                    if(mauInObject.GiaiDoan==2)
                    {
                        document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1) + " – Thẩm phán.");
                        document.ReplaceText("{ThamPhan2}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan2) + " – Thẩm phán.");
                        document.ReplaceText("{TenToaAnST}", mauInObject.ToaAnSoTham);
                    }
                    else
                    {
                        document.ReplaceText("{TenToaAnST}", mauInObject.TenToaAn);
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ThamPhan1}");
                        tempToaAnTinhParagraph.Remove(false);
                        var tempToaAnTinhParagraph2 = GetParagraph(document, "{ThamPhan2}");
                        tempToaAnTinhParagraph2.Remove(false);
                    }
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ChanhAn);
                    document.ReplaceText("{GiaiDoan}", mauInObject.GiaiDoan==1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower());
                    document.ReplaceText("{ST/PT}", mauInObject.GiaiDoan == 1 ? "ST" : "PT");
                    document.ReplaceText("{BiCan/BiCao}", mauInObject.GiaiDoan == 1 ? Setting.HS_TUCACHTOTUNG_BICAN : Setting.HS_TUCACHTOTUNG_BICAO);
                    document.ReplaceText("{VKS/TA}", mauInObject.GiaiDoan == 1 ? "Viện kiểm sát" : "Toà án");
                    document.ReplaceText("{TruyTo/XetXu}", mauInObject.GiaiDoan == 1 ? "truy tố" : "xét xử");
                    document.ReplaceText("{ToiDanh}", mauInObject.ToiDanh);
                    document.ReplaceText("{BiCaoDauVu}", bicaodauvu.DuongSuLa==Setting.DUONGSULA_CANHAN ? bicaodauvu.HoTenDuongSu : bicaodauvu.TenCoQuanToChuc);
                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHSQuyetDinhPCTPDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHSQuyetDinhPCHTTKDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    string hoithamdukhuyet = "", thukydukhuyet = "", hoitham = "";
                    hoitham = ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan, mauInObject.HoiThamNhanDan) + ", " + ThemOngBa_ChuThuong(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2);
                    if(mauInObject.HoiDong==2)
                        hoitham+= ", " + ThemOngBa_ChuThuong(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3);
                    if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet == null || !mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Any())
                        hoithamdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachHoiThamNhanDanDuKhuyet)
                        {
                            if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachHoiThamNhanDanDuKhuyet.LastOrDefault())
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet) + ", ";
                            else
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachThuKyDuKhuyet == null || !mauInObject.DanhSachThuKyDuKhuyet.Any())
                        thukydukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThuKyDuKhuyet)
                        {
                            if (mauInObject.DanhSachThuKyDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThuKyDuKhuyet.LastOrDefault())
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet) + ", ";
                            else
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet);
                        }
                    }

                    var bicaodauvu = mauInObject.DanhSachBiCao.FirstOrDefault();
                    //replace  
                    
                    var nguoiky = GetParagraph(document, "{NguoiKy}");
                    if (mauInObject.LoaiChanhAn.ToLower() == "chánh án")
                    {
                        nguoiky.ReplaceText("{NguoiKy}", mauInObject.LoaiChanhAn.ToUpper());
                    }
                    else
                    {
                        nguoiky.ReplaceText("{NguoiKy}", "KT. CHÁNH ÁN");
                        nguoiky.AppendLine("PHÓ CHÁNH ÁN").Bold().Font("Times New Roman").FontSize(13);
                    }

                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{HoiTham}", hoitham);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    document.ReplaceText("{DieuLuat}", mauInObject.DieuLuat);
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else if (mauInObject.GiaiDoan == 1)
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamPhanCong}", NgayThangNam(mauInObject.NgayPhanCong));
                    document.ReplaceText("{ThuKyDuKhuyet}", thukydukhuyet);
                    document.ReplaceText("{HoiThamDuKhuyet}", hoithamdukhuyet);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    if (mauInObject.GiaiDoan == 2)
                    {
                        document.ReplaceText("{TenToaAnST}", mauInObject.ToaAnSoTham);
                    }
                    else
                    {
                        document.ReplaceText("{TenToaAnST}", mauInObject.TenToaAn);
                    }
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ChanhAn);
                    document.ReplaceText("{GiaiDoan}", mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower());
                    document.ReplaceText("{ST/PT}", mauInObject.GiaiDoan == 1 ? "ST" : "PT");
                    document.ReplaceText("{BiCan/BiCao}", mauInObject.GiaiDoan == 1 ? Setting.HS_TUCACHTOTUNG_BICAN : Setting.HS_TUCACHTOTUNG_BICAO);
                    document.ReplaceText("{VKS/TA}", mauInObject.GiaiDoan == 1 ? "Viện kiểm sát" : "Toà án");
                    document.ReplaceText("{TruyTo/XetXu}", mauInObject.GiaiDoan == 1 ? "truy tố" : "xét xử");
                    document.ReplaceText("{ToiDanh}", mauInObject.ToiDanh);
                    document.ReplaceText("{BiCaoDauVu}", bicaodauvu.DuongSuLa == Setting.DUONGSULA_CANHAN ? bicaodauvu.HoTenDuongSu : bicaodauvu.TenCoQuanToChuc);
                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHSQuyetDinhPCTKHTDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHSQuyetDinhDuaVuAnXetXuSoThamDoc(MauInSo47ViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph BiCao = GetParagraph(document, "{BiCao}");
                    string thamphandukhuyet = "", hoithamdukhuyet = "", thukydukhuyet = "", ksvdukhuyet = "",nguoithamgiattk = "";
                    var nguoithamgiatotungkhac= mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung != Setting.HS_TUCACHTOTUNG_BICAN && x.TuCachThamGiaToTung != Setting.HS_TUCACHTOTUNG_BICAO);
                    if(nguoithamgiatotungkhac!=null && nguoithamgiatotungkhac.Any())
                    {
                        foreach(var item in nguoithamgiatotungkhac)
                        {
                            if (nguoithamgiatotungkhac.Count() > 1 && item != nguoithamgiatotungkhac.LastOrDefault())
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    nguoithamgiattk += ThemOngBa_ChuThuong(item.GioiTinh, item.HoTenDuongSu);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    nguoithamgiattk += item.TenCoQuanToChuc + "; người đại diện: " + item.HoTenDuongSu + ", ";
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    nguoithamgiattk += ThemOngBa_ChuThuong(item.GioiTinh, item.HoTenDuongSu);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    nguoithamgiattk += item.TenCoQuanToChuc + "; người đại diện: " + item.HoTenDuongSu ;
                                }
                            }
                        }
                    }
                    else
                    {
                        nguoithamgiattk = "Không";
                    }
                    var danhsachbicao = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAN || x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO);
                    if (danhsachbicao != null && danhsachbicao.Any())
                    {
                        foreach (var item in danhsachbicao)
                        {
                            if (danhsachbicao.Count() == 1 || item.Equals(danhsachbicao.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                { 
                                    BiCao.ReplaceText("{BiCao}","Họ tên: " + item.HoTenDuongSu +".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    BiCao.ReplaceText("{BiCao}", item.TenCoQuanToChuc +"; người đại diện: "+item.HoTenDuongSu+ ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    if (item.NgayThangNamSinh == null)
                                        item.NgayThangNamSinh = "..............";
                                    if (item.NoiSinh == null)
                                        item.NoiSinh = "..............";
                                    if (item.NgheNghiep == null)
                                        item.NgheNghiep = "..............";
                                    var p1 = BiCao.InsertParagraphAfterSelf("Nghề nghiệp: " + item.NgheNghiep +".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = BiCao.InsertParagraphAfterSelf("Nơi cư trú: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                    var p3 = BiCao.InsertParagraphAfterSelf("Nơi sinh: " + item.NoiSinh +  ".", false, format);
                                    p3.IndentationFirstLine = 1.0f;
                                    p3.SpacingBefore(6);
                                    p3.SpacingAfter(6);
                                    var p4 = BiCao.InsertParagraphAfterSelf("Ngày tháng năm sinh: " + item.NgayThangNamSinh + ".", false, format);
                                    p4.IndentationFirstLine = 1.0f;
                                    p4.SpacingBefore(6);
                                    p4.SpacingAfter(6);
                                    var p5 = BiCao.InsertParagraphAfterSelf("Họ tên: " + item.HoTenDuongSu + ".", false, format);
                                    p5.IndentationFirstLine = 1.0f;
                                    p5.SpacingBefore(6);
                                    p5.SpacingAfter(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = BiCao.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = BiCao.InsertParagraphAfterSelf(item.TenCoQuanToChuc + "; người đại diện: " + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }
                            if (danhsachbicao.Count() == 1 || item.Equals(danhsachbicao.LastOrDefault()))
                            {
                                if(item.DuongSuLa==Setting.DUONGSULA_CANHAN)
                                {
                                    if (item.NgayThangNamSinh == null)
                                        item.NgayThangNamSinh = "..............";
                                    if (item.NoiSinh == null)
                                        item.NoiSinh = "..............";
                                    if (item.NgheNghiep == null)
                                        item.NgheNghiep = "..............";
                                    var p1 = BiCao.InsertParagraphAfterSelf("Nghề nghiệp: " + item.NgheNghiep + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = BiCao.InsertParagraphAfterSelf("Nơi cư trú: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                    var p3 = BiCao.InsertParagraphAfterSelf("Nơi sinh: " + item.NoiSinh + ".", false, format);
                                    p3.IndentationFirstLine = 1.0f;
                                    p3.SpacingBefore(6);
                                    p3.SpacingAfter(6);
                                    var p4 = BiCao.InsertParagraphAfterSelf("Ngày tháng năm sinh: " + item.NgayThangNamSinh + ".", false, format);
                                    p4.IndentationFirstLine = 1.0f;
                                    p4.SpacingBefore(6);
                                    p4.SpacingAfter(6);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p2 = BiCao.InsertParagraphAfterSelf(item.TenCoQuanToChuc + "; người đại diện: " + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }
                        }
                    }
                    else
                    {
                        BiCao.ReplaceText("{BiCao}", "Họ tên:..................");
                        var p1 = BiCao.InsertParagraphAfterSelf("Nghề nghiệp: ..................", false, format);
                        p1.IndentationFirstLine = 1.0f;
                        //p1.SpacingBefore(6);
                        p1.SpacingAfter(6);
                        var p2 = BiCao.InsertParagraphAfterSelf("Nơi cư trú: ..................", false, format);
                        p2.IndentationFirstLine = 1.0f;
                        //p2.SpacingBefore(6);
                        p2.SpacingAfter(6);
                        var p3 = BiCao.InsertParagraphAfterSelf("Nơi sinh: ..................", false, format);
                        p3.IndentationFirstLine = 1.0f;
                        p3.SpacingBefore(6);
                        p3.SpacingAfter(6);
                        var p4 = BiCao.InsertParagraphAfterSelf("Ngày tháng năm sinh: ..................", false, format);
                        p4.IndentationFirstLine = 1.0f;
                    }

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet == null || !mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Any())
                        hoithamdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachHoiThamNhanDanDuKhuyet)
                        {
                            if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachHoiThamNhanDanDuKhuyet.LastOrDefault())
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet) + ", ";
                            else
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachThuKyDuKhuyet == null || !mauInObject.DanhSachThuKyDuKhuyet.Any())
                        thukydukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThuKyDuKhuyet)
                        {
                            if (mauInObject.DanhSachThuKyDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThuKyDuKhuyet.LastOrDefault())
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet) + ", ";
                            else
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachKiemSatVienDuKhuyet == null || !mauInObject.DanhSachKiemSatVienDuKhuyet.Any())
                        ksvdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachKiemSatVienDuKhuyet)
                        {
                            if (mauInObject.DanhSachKiemSatVienDuKhuyet.Count() > 1 && item != mauInObject.DanhSachKiemSatVienDuKhuyet.LastOrDefault())
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet) + ", ";
                            else
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet);
                        }
                    }
                    //replace 
                    string hoitham = ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan, mauInObject.HoiThamNhanDan) + ", " + ThemOngBa_ChuThuong(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2);
                    
                    if (mauInObject.HoiDong == 2)
                    {
                        document.ReplaceText("{ThamPhanKhac}", ThemOngBa(mauInObject.GioiTinhThamPhanKhac, mauInObject.ThamPhanKhac) );
                        hoitham+=", "+ ThemOngBa_ChuThuong(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3);
                    }
                    else
                    {
                        var tempParagraph1 = GetParagraph(document, "{ThamPhanKhac}");
                        tempParagraph1.Remove(false);
                    }
                    document.ReplaceText("{HoiTham}", hoitham);
                    document.ReplaceText("{HoiThamDuKhuyet}", hoithamdukhuyet);
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{ThuKyDuKhuyet}", thukydukhuyet);
                    document.ReplaceText("{KiemSatVien}", ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien));
                    document.ReplaceText("{KiemSatVienDuKhuyet}", ksvdukhuyet);
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                    document.ReplaceText("{NamXetXu}", mauInObject.NgayRaQuyetDinhXetXu.Year.ToString());
                    document.ReplaceText("{NgayXetXu}", NgayThangNam(mauInObject.NgayRaQuyetDinhXetXu));
                    document.ReplaceText("{ToiDanh}", mauInObject.ToiDanh);
                    document.ReplaceText("{DieuLuat}", mauInObject.DieuLuat);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{DiaDiemMoPhienToa}", mauInObject.DiaDiemMoPhienToa);
                    document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                    document.ReplaceText("{VuAnDuocXetXu}", mauInObject.VuAnDuocXetXu.ToLower());
                    document.ReplaceText("{NguoiThamGiaToTung}", nguoithamgiattk);
                    if (String.IsNullOrEmpty(mauInObject.VatChung))
                        document.ReplaceText("{VatChung}", "Không.");
                    else
                    {
                        var vatchung = StripHtmlTags(mauInObject.VatChung);
                        Xceed.Words.NET.Paragraph VatChung = GetParagraph(document, "{VatChung}");
                        foreach (var item in vatchung)
                        {
                            if (vatchung.Count == 1 || item == vatchung.FirstOrDefault())
                                VatChung.ReplaceText("{VatChung}", item);
                            else
                            {
                                VatChung.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                            }
                        }
                    }                    
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhanChuToa);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHSQuyetDinhDuaVuAnXetXuSoThamDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHSGiayTrieuTapNguoiThamGiaTTDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath)
        {
            string filePathTemp = filePath.Replace(".docx", "_temp_.docx");
            string filePathTemp2 = filePath.Replace(".docx", "_temp2_.docx");
            try
            {
                var DSNguoiThamGiaToTung = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung != Setting.HS_TUCACHTOTUNG_BICAN && x.TuCachThamGiaToTung != Setting.HS_TUCACHTOTUNG_BICAO);
                foreach (var item in DSNguoiThamGiaToTung)
                {
                    string dieuluat = "";
                    switch (item.TuCachThamGiaToTung)
                    {
                        case "Bị hại": dieuluat = ", Điều 62"; break;
                        case "Nguyên đơn dân sự": dieuluat = ", Điều 63"; break;
                        case "Bị đơn dân sự": dieuluat = ", Điều 64"; break;
                        case "Người có quyền lợi, nghĩa vụ liên quan đến vụ án": dieuluat = "Điều 65"; break;
                        case "Người làm chứng": dieuluat = ", Điều 66"; break;
                        case "Người giám định": dieuluat = ", Điều 68"; break;
                        case "Người định giá tài sản": dieuluat = ", Điều 69"; break;
                        case "Người phiên dịch, người dịch thuật": dieuluat = ", Điều 70"; break;
                        case "Người bào chữa": dieuluat = ", Điều 73"; break;
                        case "Người bảo vệ quyền và lợi ích hợp pháp": dieuluat = ", Điều 84"; break;
                        default:
                            {
                                var ds = mauInObject.DanhSachDuongSu.Where(x => x.DuongSuID == item.NguoiDaiDienCua).FirstOrDefault();
                                if (ds != null)
                                {
                                    switch (ds.TuCachThamGiaToTung)
                                    {
                                        case "Bị hại": dieuluat = ", Điều 62"; break;
                                        case "Nguyên đơn dân sự": dieuluat = ", Điều 63"; break;
                                        case "Bị đơn dân sự": dieuluat = ", Điều 64"; break;
                                        case "Người có quyền lợi, nghĩa vụ liên quan đến vụ án": dieuluat = ", Điều 65"; break;

                                    }
                                    int nam = 0;
                                    if (ds.NgayThangNamSinh!=null && ds.NgayThangNamSinh.Count() > 4)
                                    {
                                        nam = DateTime.Parse(ds.NgayThangNamSinh).Year;
                                    }
                                    else if (ds.NgayThangNamSinh != null) { nam = int.Parse(ds.NgayThangNamSinh); }
                                    if (ds.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO && item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        dieuluat = ", Điều 435";
                                    }
                                    if (ds.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO && (DateTime.Now.Year - nam) < 18)
                                    {
                                        dieuluat = ", Điều 423";
                                    }
                                }

                            }
                            break;

                    };
                    if (item == DSNguoiThamGiaToTung.FirstOrDefault() || DSNguoiThamGiaToTung.Count() == 1)
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            document.ApplyTemplate(templatePath);
                            document.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            document.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                           
                            if(item.NguoiBaoChuaLa == "Luật sư")
                            {
                                var tinh = SettingDataService.SettingDataItemXML("city_province", "").Where(x => x.Value == item.DoanLuatSu).FirstOrDefault();
                                document.ReplaceText("{DuongSu}", "Luật sư " + item.HoTenDuongSu + " - Văn phòng " + item.VanPhongLuatSu + " - Đoàn luật sư: " + tinh.Text);
                                document.ReplaceText("{DiaChiDuongSu}", item.VanPhongLuatSu);
                                document.ReplaceText("{NoiCuTru/DiaChi}", "Địa chỉ:");
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    document.ReplaceText("{NoiCuTru/DiaChi}", "Nơi cư trú");
                                    document.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + " - ngày tháng năm sinh: "+item.NgayThangNamSinh);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    document.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                                    document.ReplaceText("{NoiCuTru/DiaChi}", "Địa chỉ");
                                }
                                document.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            }                            
                            document.ReplaceText("{TuCachThamGiaToTung}", item.TuCachThamGiaToTung);
                            document.ReplaceText("{TenVuAn}", mauInObject.TenVuAn);
                            document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaDiemMoPhienToa ?? mauInObject.DiaChiToaAn);
                            document.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            document.ReplaceText("{VKS/TA}", (mauInObject.GiaiDoan == 1 ? "VKS" : "TA"));
                            document.ReplaceText("{TruyTo/XetXu}", (mauInObject.GiaiDoan == 1 ? "truy tố" : "xét xử"));
                            document.ReplaceText("{TenToaAnST}", (mauInObject.GiaiDoan == 1 ? mauInObject.TenToaAn : mauInObject.ToaAnSoTham));
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                            document.ReplaceText("{NamXetXu}", mauInObject.ThoiGianMoPhienToa.Year.ToString());
                            document.ReplaceText("{Dieu}", dieuluat);
                            document.ReplaceText("{ToiDanh}", mauInObject.ToiDanh);
                            document.ReplaceText("{TenVuAn}", mauInObject.TenVuAn);
                            document.ReplaceText("{TenTP}", mauInObject.ThamPhan.Substring(mauInObject.ThamPhan.LastIndexOf(" ") + 1));
                            document.SaveAs(filePath);
                        }
                    }
                    else
                    {
                        using (DocX temp = DocX.Create(filePathTemp))
                        {
                            temp.ApplyTemplate(templatePath);

                            temp.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            temp.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            temp.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                temp.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(temp, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            temp.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            temp.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            if (item.NguoiBaoChuaLa == "Luật sư")
                            {
                                var tinh = SettingDataService.SettingDataItemXML("city_province", "").Where(x=>x.Value==item.DoanLuatSu).FirstOrDefault();
                                temp.ReplaceText("{DuongSu}", "Luật sư " + item.HoTenDuongSu + " - Văn phòng " + item.VanPhongLuatSu + " - Đoàn luật sư: " + tinh.Text);
                                temp.ReplaceText("{DiaChiDuongSu}", item.VanPhongLuatSu);
                                temp.ReplaceText("{NoiCuTru/DiaChi}", "Địa chỉ");
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    temp.ReplaceText("{NoiCuTru/DiaChi}", "Nơi cư trú");
                                    temp.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + " - ngày tháng năm sinh: " + item.NgayThangNamSinh);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    temp.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                                    temp.ReplaceText("{NoiCuTru/DiaChi}", "Địa chỉ");
                                }
                                temp.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            }
                            temp.ReplaceText("{TuCachThamGiaToTung}", item.TuCachThamGiaToTung);
                            temp.ReplaceText("{TenVuAn}", mauInObject.TenVuAn);
                            temp.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            temp.ReplaceText("{DiaChiToaAn}", mauInObject.DiaDiemMoPhienToa ?? mauInObject.DiaChiToaAn);
                            temp.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            temp.ReplaceText("{VKS/TA}", (mauInObject.GiaiDoan == 1 ? "VKS" : "TA"));
                            temp.ReplaceText("{TruyTo/XetXu}", (mauInObject.GiaiDoan == 1 ? "truy tố" : "xét xử"));
                            temp.ReplaceText("{TenToaAnST}", (mauInObject.GiaiDoan == 1 ? mauInObject.TenToaAn : mauInObject.ToaAnSoTham));
                            temp.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            temp.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                            temp.ReplaceText("{NamXetXu}", mauInObject.ThoiGianMoPhienToa.Year.ToString());
                            temp.ReplaceText("{Dieu}", dieuluat);                            ;
                            temp.ReplaceText("{ToiDanh}", mauInObject.ToiDanh);
                            temp.SaveAs(filePathTemp);
                            temp.ReplaceText("{TenVuAn}", mauInObject.TenVuAn);
                            temp.ReplaceText("{TenTP}", mauInObject.ThamPhan.Substring(mauInObject.ThamPhan.LastIndexOf(" ") + 1));

                            File.Copy(filePath, filePathTemp2);

                            List<Source> sources = new List<Source>();

                            sources.Add(new Source(new WmlDocument(filePathTemp2), true));

                            sources.Add(new Source(new WmlDocument(filePathTemp), true));

                            DocumentBuilder.BuildDocument(sources, filePath);


                            File.Delete(filePathTemp2);
                            File.Delete(filePathTemp);
                        }
                    }
                }


                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHSGiayTrieuTapDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHSGiayTrieuTapBiCaoDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath)
        {
            string filePathTemp = filePath.Replace(".docx", "_temp_.docx");
            string filePathTemp2 = filePath.Replace(".docx", "_temp2_.docx");
            try
            {
                var DSBiCao = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAN || x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO && (x.TinhTrangGiamGiu == Setting.HS_TINHTRANGGIAMGIU_TAMGIAM || x.TinhTrangGiamGiu == Setting.HS_TINHTRANGGIAMGIU_TAINGOAI));
                foreach (var item in DSBiCao)
                {
                    string tinhtranggiam = "";
                    if (item.TinhTrangGiamGiu == Setting.HS_TINHTRANGGIAMGIU_TAMGIAM)
                        tinhtranggiam = "TẠM GIAM";
                    else if(item.TinhTrangGiamGiu == Setting.HS_TINHTRANGGIAMGIU_TAINGOAI)
                        tinhtranggiam = "TẠI NGOẠI";
                    if (item.NgayThangNamSinh == null)
                        item.NgayThangNamSinh = "..............";
                    if (item.NoiSinh == null)
                        item.NoiSinh = "..............";
                    if (item.NgheNghiep == null)
                        item.NgheNghiep = "..............";
                    if (item.HoTenCha == null)
                        item.HoTenCha = "..............";
                    if (item.HoTenMe == null)
                        item.HoTenMe = "..............";
                    if (item == DSBiCao.FirstOrDefault() || DSBiCao.Count() == 1)
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            document.ApplyTemplate(templatePath);
                            document.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            document.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            document.ReplaceText("{TamGiam/TaiNgoai}", tinhtranggiam);
                            document.ReplaceText("{BiCao}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ", ngày tháng năm sinh: " + item.NgayThangNamSinh+", nơi cư trú: "+(item.NoiTamTru ?? item.NoiDKHKTT)+", nghề nghiệp: "+item.NgheNghiep+", con ông: "+item.HoTenCha+", con bà: "+item.HoTenMe);
                            document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                            document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                            document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                            document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            document.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            document.ReplaceText("{ST/PT}", (mauInObject.GiaiDoan == 1 ? "ST" : "PT"));
                            document.ReplaceText("{TenToaAnST}", (mauInObject.GiaiDoan == 1 ? mauInObject.TenToaAn : mauInObject.ToaAnSoTham));
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            document.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            document.ReplaceText("{TuCachThamGiaToTung}", item.TuCachThamGiaToTung);
                            document.ReplaceText("{TenVuAn}", mauInObject.TenVuAn);
                            document.ReplaceText("{TenTP}", mauInObject.ThamPhan.Substring(mauInObject.ThamPhan.LastIndexOf(" ") + 1));
                            document.SaveAs(filePath);
                        }
                    }
                    else
                    {
                        using (DocX temp = DocX.Create(filePathTemp))
                        {
                            temp.ApplyTemplate(templatePath);

                            temp.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            temp.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            temp.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                temp.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(temp, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            temp.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            temp.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            temp.ReplaceText("{TamGiam/TaiNgoai}", tinhtranggiam);
                            temp.ReplaceText("{BiCao}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ", ngày tháng năm sinh: " + item.NgayThangNamSinh + ", nơi cư trú: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ", nghề nghiệp: " + item.NgheNghiep + ", con ông: " + item.HoTenCha + ", con bà: " + item.HoTenMe);
                            temp.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                            temp.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                            temp.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                            temp.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            temp.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            temp.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            temp.ReplaceText("{ST/PT}", (mauInObject.GiaiDoan == 1 ? "ST" : "PT"));
                            temp.ReplaceText("{TenToaAnST}", (mauInObject.GiaiDoan == 1 ? mauInObject.TenToaAn : mauInObject.ToaAnSoTham));
                            temp.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            temp.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            temp.ReplaceText("{TuCachThamGiaToTung}", item.TuCachThamGiaToTung);
                            temp.ReplaceText("{TenVuAn}", mauInObject.TenVuAn);
                            temp.ReplaceText("{TenTP}", mauInObject.ThamPhan.Substring(mauInObject.ThamPhan.LastIndexOf(" ") + 1));

                            temp.SaveAs(filePathTemp);

                            File.Copy(filePath, filePathTemp2);

                            List<Source> sources = new List<Source>();

                            sources.Add(new Source(new WmlDocument(filePathTemp2), true));

                            sources.Add(new Source(new WmlDocument(filePathTemp), true));

                            DocumentBuilder.BuildDocument(sources, filePath);


                            File.Delete(filePathTemp2);
                            File.Delete(filePathTemp);
                        }
                    }
                }


                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHSGiayTrieuTapBiCaoDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHSQuyetDinhDuaVuAnXetXuPhucThamDoc(MauInSo47PTViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    var NguoiKhangCaoKhangNghi = GetParagraph(document, "{NguoiKhangCaoKhangNghi}");
                    string thamphandukhuyet = "", thukydukhuyet = "", ksvdukhuyet = "", nguoithamgiattk = "", dsbicao = "";
                    var nguoithamgiatotungkhac = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung != Setting.HS_TUCACHTOTUNG_BICAN && x.TuCachThamGiaToTung != Setting.HS_TUCACHTOTUNG_BICAO);
                    if (nguoithamgiatotungkhac != null && nguoithamgiatotungkhac.Any())
                    {
                        foreach (var item in nguoithamgiatotungkhac)
                        {
                            if (nguoithamgiatotungkhac.Count() > 1 && item != nguoithamgiatotungkhac.LastOrDefault())
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    nguoithamgiattk += ThemOngBa_ChuThuong(item.GioiTinh,item.HoTenDuongSu) + ", ";
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    nguoithamgiattk += item.TenCoQuanToChuc + "; người đại diện: " + item.HoTenDuongSu + ", ";
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    nguoithamgiattk += ThemOngBa_ChuThuong(item.GioiTinh, item.HoTenDuongSu);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    nguoithamgiattk += item.TenCoQuanToChuc + "; người đại diện: " + item.HoTenDuongSu ;
                                }
                            }
                        }
                    }
                    else
                    {
                        nguoithamgiattk = "Không";
                    }
                    var danhsachbicao = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAN || x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO);
                    foreach (var item in danhsachbicao)
                    {
                        if (danhsachbicao.Count() > 1 && item != danhsachbicao.LastOrDefault())
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                dsbicao += "Họ tên: " + item.HoTenDuongSu;
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                dsbicao += item.TenCoQuanToChuc + "; người đại diện: " + item.HoTenDuongSu;
                            }
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                dsbicao += "Họ tên: " + item.HoTenDuongSu + ", ";
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                dsbicao += item.TenCoQuanToChuc + "; người đại diện: " + item.HoTenDuongSu + ", ";
                            }
                        }
                    }

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }

                    
                    if (mauInObject.DanhSachThuKyDuKhuyet == null || !mauInObject.DanhSachThuKyDuKhuyet.Any())
                        thukydukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThuKyDuKhuyet)
                        {
                            if (mauInObject.DanhSachThuKyDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThuKyDuKhuyet.LastOrDefault())
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet) + ", ";
                            else
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachKiemSatVienDuKhuyet == null || !mauInObject.DanhSachKiemSatVienDuKhuyet.Any())
                        ksvdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachKiemSatVienDuKhuyet)
                        {
                            if (mauInObject.DanhSachKiemSatVienDuKhuyet.Count() > 1 && item != mauInObject.DanhSachKiemSatVienDuKhuyet.LastOrDefault())
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet) + ", ";
                            else
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet);
                        }
                    }

                    if (mauInObject.VienKiemSatKhangNghi != null && (mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any()))
                    {
                        document.ReplaceText("{KhangCao/KhangNghi}", "kháng cáo, kháng nghị");
                    }
                    else if ((mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any()) && mauInObject.VienKiemSatKhangNghi == null)
                    {
                        document.ReplaceText("{KhangCao/KhangNghi}", "kháng cáo");
                    }
                    else
                    {
                        document.ReplaceText("{KhangCao/KhangNghi}", "kháng nghị");
                    }

                    if (mauInObject.VienKiemSatKhangNghi != null)
                    {
                        NguoiKhangCaoKhangNghi.ReplaceText("{NguoiKhangCaoKhangNghi}", mauInObject.VienKiemSatKhangNghi + ".");
                    }
                    else
                    {
                        NguoiKhangCaoKhangNghi.ReplaceText("{NguoiKhangCaoKhangNghi}", "");
                    }
                    if (mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any())
                    {
                        foreach (var item in mauInObject.NguoiKhangCao)
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                        }
                    }

                    //replace 

                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1));
                    document.ReplaceText("{ThamPhan2}", ThemOngBa(mauInObject.GioiTinhThamPhan2, mauInObject.ThamPhan2));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{ThuKyDuKhuyet}", thukydukhuyet);
                    document.ReplaceText("{KiemSatVien}", ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien));
                    document.ReplaceText("{KiemSatVienDuKhuyet}", ksvdukhuyet);
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{DSBiCao}", dsbicao);
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                    document.ReplaceText("{NamXetXu}", mauInObject.NgayRaQuyetDinhXetXu.Year.ToString());
                    document.ReplaceText("{NgayXetXu}", NgayThangNam(mauInObject.NgayRaQuyetDinhXetXu));
                    document.ReplaceText("{ToiDanh}", mauInObject.ToiDanh);
                    document.ReplaceText("{DieuLuat}", mauInObject.DieuLuat);
                    document.ReplaceText("{HinhPhat}", mauInObject.HinhPhat);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{ToaAnST}", mauInObject.ToaAnSoTham);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{DiaDiemMoPhienToa}", mauInObject.DiaDiemMoPhienToa);
                    document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                    document.ReplaceText("{VuAnDuocXetXu}", mauInObject.VuAnDuocXetXu.ToLower());
                    document.ReplaceText("{NguoiThamGiaToTung}", nguoithamgiattk);
                    if (String.IsNullOrEmpty(mauInObject.VatChung))
                        document.ReplaceText("{VatChung}", "Không.");
                    else
                    {
                        var vatchung = StripHtmlTags(mauInObject.VatChung);
                        Xceed.Words.NET.Paragraph VatChung = GetParagraph(document, "{VatChung}");
                        foreach (var item in vatchung)
                        {
                            if (vatchung.Count == 1 || item == vatchung.FirstOrDefault())
                                VatChung.ReplaceText("{VatChung}", item);
                            else
                            {
                                VatChung.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                            }
                        }
                    }
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhanChuToa);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHSQuyetDinhDuaVuAnXetXuPhucThamDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInHSBiaHoSoSTDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {                
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    Xceed.Words.NET.Paragraph HoiThamNhanDan3 = GetParagraph(document, "{HoiThamNhanDan3}");
                    Xceed.Words.NET.Paragraph ThamPhanKhac = GetParagraph(document, "{ThamPhanKhac}");
                    if (mauInObject.HoiDong == 2)
                    {
                        HoiThamNhanDan3.ReplaceText("{ThamPhanKhac}", ThemOngBa(mauInObject.GioiTinhThamPhanKhac, mauInObject.ThamPhanKhac));
                        HoiThamNhanDan3.ReplaceText("{HoiThamNhanDan3}", "3. " + ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3));
                    }                        
                    else
                    {
                        ThamPhanKhac.ReplaceText("{ThamPhanKhac}", "");
                        HoiThamNhanDan3.Remove(false);
                    }

                    Xceed.Words.NET.Table t = document.Tables[1];
                    int i = 1;
                    float[] f = { 7, 4.5f, 4.5f };
                    t.SetWidths(f);
                    foreach (var item in mauInObject.DanhSachBiCao)
                    {
                        t.InsertRow();

                        t.Rows[i].Cells[0].Paragraphs.First().AppendLine("Họ tên: " + item.HoTenDuongSu).Font("Times New Roman").FontSize(14);
                        t.Rows[i].Cells[0].Paragraphs.First().AppendLine("Ngày tháng năm sinh: " + item.NgayThangNamSinh).Font("Times New Roman").FontSize(14);
                        t.Rows[i].Cells[0].Paragraphs.First().AppendLine("Địa chỉ: " + item.NoiTamTru ?? item.NoiDKHKTT).Font("Times New Roman").FontSize(14);
                        t.Rows[i].Cells[0].Paragraphs.First().AppendLine("Tình trạng giam giữ: " + item.TinhTrangGiamGiu).Font("Times New Roman").FontSize(14);

                        t.Rows[i].Cells[1].Paragraphs.First().AppendLine("- Tội danh: " + item.ToiDanhTruyTo).Font("Times New Roman").FontSize(14);
                        t.Rows[i].Cells[1].Paragraphs.First().AppendLine("- Điều luật: " + item.DieuLuatTruyTo).Font("Times New Roman").FontSize(14);

                        t.Rows[i].Cells[2].Paragraphs.First().AppendLine("Áp dụng: " + item.DieuLuatApDung).Font("Times New Roman").FontSize(14);
                        t.Rows[i].Cells[2].Paragraphs.First().AppendLine("Hình phạt: " + item.HinhPhat).Font("Times New Roman").FontSize(14);
                      
                        i++;
                    }
                    t.AutoFit = AutoFit.Contents;
                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }

                    document.ReplaceText("{SoBiCao}", mauInObject.SoBiCan<10 ? "0" + mauInObject.SoBiCan : mauInObject.SoBiCan.ToString());
                    document.ReplaceText("{SoCaoTrang}", mauInObject.SoCaoTrang);
                    document.ReplaceText("{NgayCaoTrang}", NgayThangNam(mauInObject.NgayCaoTrang));
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());                    
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{HoiThamNhanDan}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan1, mauInObject.HoiThamNhanDan1));
                    document.ReplaceText("{HoiThamNhanDan2}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2));
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{KiemSatVien}", ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien));

                    document.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAn != null ? "Bản án" : "Quyết định");
                    document.ReplaceText("{SoBAQD}", mauInObject.SoBanAn != null ? mauInObject.SoBanAn : mauInObject.SoQuyetDinh);
                    document.ReplaceText("{NamBAQD}", mauInObject.SoBanAn != null ? mauInObject.NgayRaBanAn.Year.ToString() : mauInObject.NgayRaQuyetDinh.Year.ToString());
                    document.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAn != null ? NgayThangNam(mauInObject.NgayRaBanAn) : NgayThangNam(mauInObject.NgayRaQuyetDinh));

                    
                    if (mauInObject.VienKiemSatKhangNghi == null && (mauInObject.DanhSachNguoiKhangCao == null || !mauInObject.DanhSachNguoiKhangCao.Any()))
                        document.ReplaceText("{NgayHieuLuc}", NgayThangNam(mauInObject.HieuLuc));
                    else
                        document.ReplaceText("{NgayHieuLuc}", "ngày......tháng.......năm..........");
                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHSBiaHoSoSTDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        public string[] MauInHSBiaHoSoPTDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    
                    Xceed.Words.NET.Table t = document.Tables[1];
                    int i = 1;
                    float[] f = { 5, 4, 4, 4 };
                    t.SetWidths(f);
                    foreach (var item in mauInObject.DanhSachBiCao)
                    {
                        t.InsertRow();

                        t.Rows[i].Cells[0].Paragraphs.First().AppendLine("Họ tên: " + item.HoTenDuongSu).Font("Times New Roman").FontSize(14);
                        t.Rows[i].Cells[0].Paragraphs.First().AppendLine("Ngày tháng năm sinh: " + item.NgayThangNamSinh).Font("Times New Roman").FontSize(14);
                        t.Rows[i].Cells[0].Paragraphs.First().AppendLine("Địa chỉ: " + item.NoiTamTru ?? item.NoiDKHKTT).Font("Times New Roman").FontSize(14);
                        t.Rows[i].Cells[0].Paragraphs.First().AppendLine("Tình trạng giam giữ: " + item.TinhTrangGiamGiu).Font("Times New Roman").FontSize(14);
                        
                        t.Rows[i].Cells[1].Paragraphs.First().AppendLine("- Điều luật: " + item.DieuLuatApDung).Font("Times New Roman").FontSize(14);
                        t.Rows[i].Cells[1].Paragraphs.First().AppendLine("- Hình phạt: " + item.HinhPhat).Font("Times New Roman").FontSize(14);

                        i++;
                    }

                    if (mauInObject.VienKiemSatKhangNghi != null)
                    {
                        t.Rows[1].Cells[2].Paragraphs.First().AppendLine("Kháng nghị " + mauInObject.SoKhangNghi.ToString() + "/" + mauInObject.NgayKhangNghi.Year.ToString() +" "+ NgayThangNam(mauInObject.NgayKhangNghi) + " của " + mauInObject.VienKiemSatKhangNghi + ", nội dung: " + StripHtmlTags(mauInObject.NoiDungKhangNghi)).Font("Times New Roman").FontSize(14);
                    }
                    foreach (var item in mauInObject.DanhSachNguoiKhangCao)
                    {
                        
                        t.Rows[1].Cells[2].Paragraphs.First().AppendLine("Kháng cáo " + item.KhangCaoID+"/"+item.NgayNop.Year + " " + NgayThangNam(item.NgayNop) + ", nội dung: "+StripHtmlTags(item.NoiDungKhangCao)).Font("Times New Roman").FontSize(14);
                    }

                    if(mauInObject.SoBanAn!=null)
                        t.Rows[1].Cells[3].Paragraphs.First().AppendLine("Nội dung bản án: " + StripHtmlTags(mauInObject.NoiDungBanAn)).Font("Times New Roman").FontSize(14);
                    else
                        t.Rows[1].Cells[3].Paragraphs.First().AppendLine("Nội dung quyết định: " + StripHtmlTags(mauInObject.NoiDungQuyetDinh)).Font("Times New Roman").FontSize(14);


                    if (mauInObject.DanhSachBiCao.Count>1)
                    {
                        t.MergeCellsInColumn(2, 1, i - 1);
                        t.MergeCellsInColumn(3, 1, i - 1);
                    }
                    t.AutoFit = AutoFit.Contents;
                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    
                    document.ReplaceText("{SoBiCao}", mauInObject.SoBiCan < 10 ? "0" + mauInObject.SoBiCan : mauInObject.SoBiCan.ToString());
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1));
                    document.ReplaceText("{ThamPhan2}", ThemOngBa(mauInObject.GioiTinhThamPhan2, mauInObject.ThamPhan2));
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{KiemSatVien}", ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien));

                    document.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAn != null ? "Bản án" : "Quyết định");
                    document.ReplaceText("{SoBAQD}", mauInObject.SoBanAn != null ? mauInObject.SoBanAn : mauInObject.SoQuyetDinh);
                    document.ReplaceText("{NamBAQD}", mauInObject.SoBanAn != null ? mauInObject.NgayRaBanAn.Year.ToString() : mauInObject.NgayRaQuyetDinh.Year.ToString());
                    document.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAn != null ? NgayThangNam(mauInObject.NgayRaBanAn) : NgayThangNam(mauInObject.NgayRaQuyetDinh));
                    document.ReplaceText("{Ma}", mauInObject.SoBanAn != null ? "HS-ST" : "HSST-QĐ");

                    document.ReplaceText("{BanAn/QuyetDinhPT}", mauInObject.SoBanAnPT != null ? "Bản án" : "Quyết định");
                    document.ReplaceText("{SoBAQDPT}", mauInObject.SoBanAnPT != null ? mauInObject.SoBanAnPT : mauInObject.SoQuyetDinhPT);
                    document.ReplaceText("{NamBAQDPT}", mauInObject.SoBanAnPT != null ? mauInObject.NgayRaBanAnPT.Year.ToString() : mauInObject.NgayRaQuyetDinhPT.Year.ToString());
                    document.ReplaceText("{NgayRaBAQDPT}", mauInObject.SoBanAnPT != null ? NgayThangNam(mauInObject.NgayRaBanAnPT) : NgayThangNam(mauInObject.NgayRaQuyetDinhPT));
                    document.ReplaceText("{MaPT}", mauInObject.SoBanAnPT != null ? "HS-PT" : "HSPT-QĐ");

                    
                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHSBiaHoSoPTDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        public string[] MauInHSLenhTrichXuatDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath)
        {
            string filePathTemp = filePath.Replace(".docx", "_temp_.docx");
            string filePathTemp2 = filePath.Replace(".docx", "_temp2_.docx");
            try
            {
                var DSBiCao = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAN || x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO && (x.TinhTrangGiamGiu == Setting.HS_TINHTRANGGIAMGIU_TAMGIAMVUANKHAC || x.TinhTrangGiamGiu == Setting.HS_TINHTRANGGIAMGIU_CHAPHANHBANANKHAC || x.TinhTrangGiamGiu==Setting.HS_TINHTRANGGIAMGIU_TAMGIAM));
                foreach (var item in DSBiCao)                {
                    
                    if (item.NgayThangNamSinh == null)
                        item.NgayThangNamSinh = "..............";
                    if (item.NoiSinh == null)
                        item.NoiSinh = "..............";
                    if (item.NgheNghiep == null)
                        item.NgheNghiep = "..............";
                    if (item.HoTenCha == null)
                        item.HoTenCha = "..............";
                    if (item.HoTenMe == null)
                        item.HoTenMe = "..............";
                    if (item == DSBiCao.FirstOrDefault() || DSBiCao.Count() == 1)
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            document.ApplyTemplate(templatePath);
                            document.ReplaceText("{SoTrichXuat}", item.SoTrieuTap.ToString());
                            document.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            document.ReplaceText("{HoTen}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            document.ReplaceText("NgayThangNamSinh", item.NgayThangNamSinh);
                            document.ReplaceText("{NoiSinh}", item.NoiSinh);
                            document.ReplaceText("{DiaChi}", item.NoiTamTru ?? item.NoiDKHKTT);
                            document.ReplaceText("{TenCha}", item.HoTenCha);
                            document.ReplaceText("{TenMe}", item.HoTenMe);
                            document.ReplaceText("{ToiDanh}", item.ToiDanhTruyTo);
                            document.ReplaceText("{DieuLuat}", item.DieuLuatTruyTo);
                            document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                            document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                            document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                            document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            document.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            document.ReplaceText("{VKS/TA}", (mauInObject.GiaiDoan == 1 ? "Viện kiểm sát" : "Toà án"));
                            document.ReplaceText("{TruyTo/XetXu}", (mauInObject.GiaiDoan == 1 ? "truy tố" : "xét xử"));
                            document.ReplaceText("{ST/PT}", (mauInObject.GiaiDoan == 1 ? "ST" : "PT"));
                            document.ReplaceText("{TenToaAnST}", (mauInObject.GiaiDoan == 1 ? mauInObject.TenToaAn : mauInObject.ToaAnSoTham));
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);
                            if (mauInObject.TenToaAn.Contains("tỉnh"))
                                document.ReplaceText("{CanhSat}", "Phòng Cảnh sát Thi hành án hình sự và Hỗ trợ tư pháp Công an tỉnh Cà Mau");
                            else
                                document.ReplaceText("{CanhSat}", "Đội Cảnh sát Thi hành án hình sự và Hỗ trợ tư pháp Công an");
                            document.SaveAs(filePath);
                        }
                    }
                    else
                    {
                        using (DocX temp = DocX.Create(filePathTemp))
                        {
                            temp.ApplyTemplate(templatePath);

                            temp.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            temp.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            temp.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                temp.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(temp, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            temp.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            temp.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            temp.ReplaceText("{HoTen}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            temp.ReplaceText("NgayThangNamSinh", item.NgayThangNamSinh);
                            temp.ReplaceText("{NoiSinh}", item.NoiSinh);
                            temp.ReplaceText("{DiaChi}", item.NoiTamTru ?? item.NoiDKHKTT);
                            temp.ReplaceText("{TenCha}", item.HoTenCha);
                            temp.ReplaceText("{TenMe}", item.HoTenMe);
                            temp.ReplaceText("{ToiDanh}", item.ToiDanhTruyTo);
                            temp.ReplaceText("{DieuLuat}", item.DieuLuatTruyTo);
                            temp.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                            temp.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                            temp.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                            temp.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            temp.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            temp.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            temp.ReplaceText("{VKS/TA}", (mauInObject.GiaiDoan == 1 ? "Viện kiểm sát" : "Toà án"));
                            temp.ReplaceText("{TruyTo/XetXu}", (mauInObject.GiaiDoan == 1 ? "truy tố" : "xét xử"));
                            temp.ReplaceText("{ST/PT}", (mauInObject.GiaiDoan == 1 ? "ST" : "PT"));
                            temp.ReplaceText("{TenToaAnST}", (mauInObject.GiaiDoan == 1 ? mauInObject.TenToaAn : mauInObject.ToaAnSoTham));
                            temp.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);

                            if (mauInObject.TenToaAn.Contains("tỉnh"))
                                temp.ReplaceText("{CanhSat}", "Phòng Cảnh sát Thi hành án hình sự và Hỗ trợ tư pháp Công an tỉnh Cà Mau");
                            else
                                temp.ReplaceText("{CanhSat}", "Đội Cảnh sát Thi hành án hình sự và Hỗ trợ tư pháp Công an");

                            temp.SaveAs(filePathTemp);

                            File.Copy(filePath, filePathTemp2);

                            List<Source> sources = new List<Source>();

                            sources.Add(new Source(new WmlDocument(filePathTemp2), true));

                            sources.Add(new Source(new WmlDocument(filePathTemp), true));

                            DocumentBuilder.BuildDocument(sources, filePath);


                            File.Delete(filePathTemp2);
                            File.Delete(filePathTemp);
                        }
                    }
                }


                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHSGiayTrieuTapBiCaoDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }


        //DS, HN, LD, KT
        public string[] MauIn01BiaHoSoNhanDonDoc(MauInBiaHoSoNhanDonViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    Xceed.Words.NET.Paragraph NguyenDon = GetParagraph(document, "{NguyenDon}");
                    Xceed.Words.NET.Paragraph BiDon = GetParagraph(document, "{BiDon}");
                    int i = 1;
                    foreach (var item in mauInObject.DanhSachNguyenDon)
                    {
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (mauInObject.DanhSachNguyenDon.Count() == 1 || item.Equals(mauInObject.DanhSachNguyenDon.FirstOrDefault()))
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguyenDon.ReplaceText("{NguyenDon}", i +". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                NguyenDon.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh+".").Font("Times New Roman").FontSize(14);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                NguyenDon.ReplaceText("{NguyenDon}", i + ". Cơ quan: " + item.TenCoQuanToChuc + ".");
                            }
                            NguyenDon.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguyenDon.AppendLine( i + ". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                                NguyenDon.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                NguyenDon.AppendLine( i + ". Cơ quan: " + item.TenCoQuanToChuc + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                            }
                            NguyenDon.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                        }
                        i++;
                    }
                    i = 1;
                    foreach (var item in mauInObject.DanhSachBiDon)
                    {
                        
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (mauInObject.DanhSachBiDon.Count() == 1 || item.Equals(mauInObject.DanhSachBiDon.FirstOrDefault()))
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                BiDon.ReplaceText("{BiDon}", i + ". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                BiDon.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                BiDon.ReplaceText("{BiDon}", i + ". Cơ quan: " + item.TenCoQuanToChuc + ".");
                            }
                            BiDon.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                BiDon.AppendLine(i + ". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                                BiDon.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                BiDon.AppendLine(i + ". Cơ quan: " + item.TenCoQuanToChuc + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                            }
                            BiDon.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                        }
                        i++;
                    }

                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}","");
                        tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment=Alignment.center;
                    }
                    var noidungdon = StripHtmlTags(mauInObject.NoiDungDon);
                    Xceed.Words.NET.Paragraph NoiDungDon = GetParagraph(document, "{NoiDungDon}");
                    foreach (var item in noidungdon)
                    {
                        if (noidungdon.Count == 1 || item == noidungdon.FirstOrDefault())
                            NoiDungDon.ReplaceText("{NoiDungDon}", item);
                        else
                        {
                            NoiDungDon.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                        }
                    }
                    document.ReplaceText("{NgayNhanDon}", NgayThangNam(mauInObject.NgayNhanDon).Replace("ngày", "Ngày"));
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));


                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn01BiaHoSoNhanDonDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        public string[] MauIn02GiayXacNhanDaNhanDonDoc(MauInSo24ViewModel mauInObject, string filePath, string templatePath)
        {
            string nguyendon = "";
            try
            {
                if (mauInObject.DanhSachDuongSuViewModel.Any())
                    foreach (var item in mauInObject.DanhSachDuongSuViewModel)
                    {
                        if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                        {
                            nguyendon += ThemOngBa_ChuThuong(item.GioiTinh,item.HoTenDuongSu) + ", ";
                        }
                        else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                        {
                            nguyendon += item.TenCoQuanToChuc + ", ";
                        }
                    }
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    Xceed.Words.NET.Paragraph danhsachduongsu = GetParagraph(document, "{DanhSachDuongSu}");
                    if (mauInObject.DanhSachDuongSuViewModel.Any())
                        foreach (var item in mauInObject.DanhSachDuongSuViewModel)
                        {
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    danhsachduongsu.ReplaceText("{DanhSachDuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    danhsachduongsu.ReplaceText("{DanhSachDuongSu}", item.TenCoQuanToChuc + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = danhsachduongsu.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = danhsachduongsu.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.LastOrDefault()))
                            {
                                var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachDuongSuViewModel.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachDuongSuViewModel.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                            }
                        }


                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo.ToString());
                    document.ReplaceText("{NamNopHS}", mauInObject.NgayNopDonTaiToaAn.Year.ToString());
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                        tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayNopDon}", NgayThangNam(mauInObject.NgayNopDonTaiToaAn));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{NgayLamDon}", NgayThangNam(mauInObject.NgayLamDon));
                    document.ReplaceText("{NguyenDon}", nguyendon);
                    if (mauInObject.HinhThucGoiDon == "Trực tiếp")
                    {
                        document.ReplaceText("{HinhThucNop}", "nộp trực tiếp");
                    }
                    else if (mauInObject.HinhThucGoiDon == "Gián tiếp")
                    {
                        document.ReplaceText("{HinhThucNop}", "do tổ chức dịch vụ bưu chính chuyển đến");
                    }
                    else
                    {
                        document.ReplaceText("{HinhThucNop}", "gửi qua " + (mauInObject.HinhThucGoiDon != null ? mauInObject.HinhThucGoiDon.ToLower() : ".................."));
                    }

                    var noidungdon = StripHtmlTags(mauInObject.NoiDungDon);
                    Xceed.Words.NET.Paragraph NoiDungDon = GetParagraph(document, "{NoiDungDon}");
                    foreach (var item in noidungdon)
                    {
                        if (noidungdon.Count == 1 || item == noidungdon.FirstOrDefault())
                            NoiDungDon.ReplaceText("{NoiDungDon}", item);
                        else
                        {
                            NoiDungDon.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                        }
                    }
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.HoTenNguoiKyXacNhanDaNhanDon);

                    if (mauInObject.ChucVuNguoiKyXacNhan.ToLower() == "chánh án" || mauInObject.ChucDanhNguoiKyXacNhan.ToLower() == "thẩm phán")
                    {
                        document.ReplaceText("{NguoiKy}", mauInObject.ChucDanhNguoiKyXacNhan.ToUpper());
                    }
                    else
                    {
                        var pChucDanhNguoiKyXacNhan = GetParagraph(document, "{NguoiKy}");
                        pChucDanhNguoiKyXacNhan.ReplaceText("{NguoiKy}", "TL.CHÁNH ÁN");
                        pChucDanhNguoiKyXacNhan.AppendLine("KT.CHÁNH VĂN PHÒNG").Bold().Font("Times New Roman").FontSize(13);
                        pChucDanhNguoiKyXacNhan.AppendLine("PHÓ CHÁNH VĂN PHÒNG").Bold().Font("Times New Roman").FontSize(13);
                    }

                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn02GiayXacNhanDaNhanDonDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        public string[] MauIn03QuyetDinhPCTPGiaiQuyetDonDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    string nguyendon = "";
                    foreach (var item in mauInObject.DanhSachNguyenDon)
                    {
                        if (mauInObject.DanhSachNguyenDon.Count > 1 && item != mauInObject.DanhSachNguyenDon.LastOrDefault())
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                nguyendon += ThemOngBa_ChuThuong(item.GioiTinh, item.HoTenDuongSu) + "; Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + "; ";
                            }
                            else
                            {
                                nguyendon += item.TenCoQuanToChuc + "; Địa chỉ: " + item.NoiDKHKTT + "; ";
                            }
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                nguyendon += ThemOngBa_ChuThuong(item.GioiTinh, item.HoTenDuongSu) + "; Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT);
                            }
                            else
                            {
                                nguyendon += item.TenCoQuanToChuc + "; Địa chỉ: " + item.NoiDKHKTT;
                            }
                        }
                    }

                    //replace               

                    var nguoiky = GetParagraph(document, "{NguoiKy}");
                    if (mauInObject.LoaiChanhAn.ToLower() == "chánh án")
                    {
                        nguoiky.ReplaceText("{NguoiKy}", mauInObject.LoaiChanhAn.ToUpper());
                    }
                    else
                    {
                        nguoiky.ReplaceText("{NguoiKy}", "KT. CHÁNH ÁN");
                        nguoiky.AppendLine("PHÓ CHÁNH ÁN").Bold().Font("Times New Roman").FontSize(13);
                    }

                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhan, mauInObject.ThamPhan));
                    document.ReplaceText("{NamNopHS}", mauInObject.NgayNopDonTaiToaAn.Year.ToString());
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo.ToString());
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                        tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamPhanCong}", NgayThangNam(mauInObject.NgayPhanCong));
                    document.ReplaceText("{NguyenDon}", nguyendon);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ChanhAn);
                    var noidungdon = StripHtmlTags(mauInObject.NoiDungDon);
                    Xceed.Words.NET.Paragraph NoiDungDon = GetParagraph(document, "{NoiDungDon}");
                    foreach (var item in noidungdon)
                    {
                        if (noidungdon.Count == 1 || item == noidungdon.FirstOrDefault())
                            NoiDungDon.ReplaceText("{NoiDungDon}", item);
                        else
                        {
                            NoiDungDon.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                        }
                    }

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("MauIn03QuyetDinhPCTPGiaiQuyetDonDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauIn04ThongBaoNopTienTamUngSTDoc(MauInSo29ViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo.ToString());
                    document.ReplaceText("{NamRaThongBao}", mauInObject.NgayRaThongBao.Year.ToString());
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                        tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{NguoiDuNop}", (mauInObject.NguoiDuNop.DuongSuLa == Setting.DUONGSULA_CANHAN ? ThemOngBa(mauInObject.NguoiDuNop.GioiTinh, mauInObject.NguoiDuNop.HoTenDuongSu) : mauInObject.NguoiDuNop.TenCoQuanToChuc));
                    document.ReplaceText("{NguoiDuNopThuong}", (mauInObject.NguoiDuNop.DuongSuLa == Setting.DUONGSULA_CANHAN ? ThemOngBa_ChuThuong(mauInObject.NguoiDuNop.GioiTinh, mauInObject.NguoiDuNop.HoTenDuongSu) : mauInObject.NguoiDuNop.TenCoQuanToChuc));
                    document.ReplaceText("{DiaChiNguoiDuDop}", mauInObject.NguoiDuNop.NoiTamTru ?? mauInObject.NguoiDuNop.NoiDKHKTT);
                    document.ReplaceText("{CoQuanThiHanhAnThu}", mauInObject.CoQuanThiHanhAnThu);
                    document.ReplaceText("{GiaTriDuNopFormatted}", mauInObject.GiaTriDuNopFormatted);
                    document.ReplaceText("{GiaTriDuNopBangChu}", mauInObject.GiaTriDuNopBangChu);
                    document.ReplaceText("{DiaChiCoQuanThiHanhAnThu}", mauInObject.DiaChiCoQuanThiHanhAnThu);
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);


                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn04ThongBaoNopTienTamUngSTDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        public string[] MauIn05ThongBaoThuLySTDoc(MauInSo30ViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);
                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph DanhSachDuongSu = GetParagraph(document, "{DanhSachDuongSu}");
                    Xceed.Words.NET.Paragraph NguyenDon = GetParagraph(document, "{NguyenDon}");
                    if (mauInObject.DanhSachDuongSuViewModel != null && mauInObject.DanhSachDuongSuViewModel.Any())
                    {
                        foreach (var item in mauInObject.DanhSachDuongSuViewModel)
                        {
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    DanhSachDuongSu.ReplaceText("{DanhSachDuongSu}", (item.GioiTinh == "Nam" ? "- Ông " : "- Bà ") + item.HoTenDuongSu + "; Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    DanhSachDuongSu.ReplaceText("{DanhSachDuongSu}", "- " + item.TenCoQuanToChuc + "; Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {                                    
                                    var p2 = DanhSachDuongSu.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "- Ông " : "- Bà ") + item.HoTenDuongSu + "; Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p2.IndentationFirstLine = 3.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p2 = DanhSachDuongSu.InsertParagraphAfterSelf("- " + item.TenCoQuanToChuc + "; Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p2.IndentationFirstLine = 3.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }                            
                        }
                    }
                    else
                    {
                        DanhSachDuongSu.ReplaceText("{DanhSachDuongSu}", "-..................;Địa chỉ:...........................");
                    }
                    var nguyendon = mauInObject.DanhSachDuongSuViewModel.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON));
                    if(mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_YEUCAU)
                        nguyendon = mauInObject.DanhSachDuongSuViewModel.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU));
                    if (nguyendon !=null && nguyendon.Any())
                    {
                        foreach (var item in nguyendon)
                        {
                            if (nguyendon.Count() == 1 || item.Equals(nguyendon.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    NguyenDon.ReplaceText("{NguyenDon}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    NguyenDon.ReplaceText("{NguyenDon}", item.TenCoQuanToChuc + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    p1.SpacingAfter(6);
                                    var p2 = NguyenDon.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    p1.SpacingAfter(6);
                                    var p2 = NguyenDon.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                }
                            }
                            if (nguyendon.Count() == 1 || item.Equals(nguyendon.LastOrDefault()))
                            {
                                var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (nguyendon.FirstOrDefault().NoiTamTru ?? nguyendon.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                p1.SpacingAfter(6);
                            }
                        }
                    }
                    else
                    {
                        NguyenDon.ReplaceText("{NguyenDon}", "Ông (Bà) ..........................................");
                        NguyenDon.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguyenDon.IndentationFirstLine = 1.0f;
                    }
                    //replace    
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                        tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy).Replace("ngày", "Ngày"));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{NhomAn}", mauInObject.NhomAn);
                    document.ReplaceText("{QuanHePhapLuat}", mauInObject.QuanHePhapLuat);
                    var noidungdon = StripHtmlTags(mauInObject.NoiDungYeuCau);
                    Xceed.Words.NET.Paragraph NoiDungDon = GetParagraph(document, "{NoiDungDon}");
                    foreach (var item in noidungdon)
                    {
                        if (noidungdon.Count == 1 || item == noidungdon.FirstOrDefault())
                            NoiDungDon.ReplaceText("{NoiDungDon}", item);
                        else
                        {
                            NoiDungDon.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                        }
                    }
                    document.ReplaceText("{ThuTuc}", mauInObject.ThuLyTheoThuTuc.ToLower());
                    if (String.IsNullOrEmpty(mauInObject.TaiLieuChungTuKemTheo))
                        document.ReplaceText("{TaiLieuChungTuKemTheo}", "Không");
                    else
                    {
                        var tailieu = StripHtmlTags(mauInObject.TaiLieuChungTuKemTheo);
                        Xceed.Words.NET.Paragraph TaiLieuChungTuKemTheo = GetParagraph(document, "{TaiLieuChungTuKemTheo}");
                        foreach (var item in tailieu)
                        {
                            if (tailieu.Count == 1 || item == tailieu.FirstOrDefault())
                                TaiLieuChungTuKemTheo.ReplaceText("{TaiLieuChungTuKemTheo}", item);
                            else
                            {
                                TaiLieuChungTuKemTheo.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                            }
                        }
                    }
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn05ThongBaoThuLySTDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauIn0613QuyetDinhPCTPDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath, string pathChuKy)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguyenDon = GetParagraph(document, "{NguyenDon}");
                    Xceed.Words.NET.Paragraph BiDon = GetParagraph(document, "{BiDon}");

                    string thamphandukhuyet = "", hoithamdukhuyet = "";
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {
                        var kk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON));
                        if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                            kk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU));
                        if (kk != null && kk.Any())
                        {
                            foreach (var item in kk)
                            {
                                if (kk.Count() == 1 || item.Equals(kk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguyenDon.ReplaceText("{NguyenDon}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguyenDon.ReplaceText("{NguyenDon}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguyenDon.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguyenDon.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (kk.Count() == 1 || item.Equals(kk.LastOrDefault()))
                                {
                                    var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (kk.FirstOrDefault().NoiTamTru ?? kk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var bk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_BIDON));
                        if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                            bk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN));
                        if (bk != null && bk.Any())
                        {
                            foreach (var item in bk)
                            {
                                if (bk.Count() == 1 || item.Equals(bk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        BiDon.ReplaceText("{BiDon}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        BiDon.ReplaceText("{BiDon}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = BiDon.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = BiDon.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (bk.Count() == 1 || item.Equals(bk.LastOrDefault()))
                                {
                                    var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (bk.FirstOrDefault().NoiTamTru ?? bk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }
                        else if(mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_YEUCAU)
                        {
                            BiDon.ReplaceText("{BiDon}", "Không.");
                        }
                    }
                    else
                    {
                        NguyenDon.ReplaceText("{NguyenDon}", "Ông (Bà) ..........................................");
                        NguyenDon.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguyenDon.IndentationFirstLine = 1.0f;

                        BiDon.ReplaceText("{BiDon}", "Ông (Bà) ..........................................");
                        BiDon.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        BiDon.IndentationFirstLine = 1.0f;
                    }

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() == 1 || item == mauInObject.DanhSachThamPhanDuKhuyet.FirstOrDefault())
                                thamphandukhuyet += ThemOngBa(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else if (mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }
                    if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet == null || !mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Any())
                        hoithamdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachHoiThamNhanDanDuKhuyet)
                        {
                            if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Count() == 1 || item == mauInObject.DanhSachHoiThamNhanDanDuKhuyet.FirstOrDefault())
                                hoithamdukhuyet += ThemOngBa(item.GioiTinh, item.HoiThamNhanDanDuKhuyet) + ", ";
                            else if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachHoiThamNhanDanDuKhuyet.LastOrDefault())
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet) + ", ";
                            else
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet);
                        }
                    }
                    
                    //replace 
                    document.ReplaceText("{HoiThamNhanDan1}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan, mauInObject.HoiThamNhanDan));
                    document.ReplaceText("{HoiThamNhanDan2}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2));
                    document.ReplaceText("{HoiThamDuKhuyet}", hoithamdukhuyet);
                    if (mauInObject.HoiDong == 2)
                    {
                        document.ReplaceText("{ThamPhanKhac}", "Thẩm phán: "+ ThemOngBa(mauInObject.GioiTinhThamPhanKhac, mauInObject.ThamPhanKhac) + ".");
                        document.ReplaceText("{HoiThamNhanDan3}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3) + ".");
                    }
                    else if(mauInObject.GiaiDoan==1 && mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_TRANHCHAP)
                    {
                        var tempParagraph1 = GetParagraph(document, "{ThamPhanKhac}");
                        var tempParagraph2 = GetParagraph(document, "{HoiThamNhanDan3}");
                        tempParagraph1.Remove(false);
                        tempParagraph2.Remove(false);
                    }
                    var nguoiky = GetParagraph(document, "{NguoiKy}");
                    if (mauInObject.LoaiChanhAn.ToLower() == "chánh án")
                    {
                        nguoiky.ReplaceText("{NguoiKy}", mauInObject.LoaiChanhAn.ToUpper());
                    }
                    else
                    {
                        nguoiky.ReplaceText("{NguoiKy}", "KT. CHÁNH ÁN");
                        nguoiky.AppendLine("PHÓ CHÁNH ÁN").Bold().Font("Times New Roman").FontSize(13);
                    }

                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhan, mauInObject.ThamPhan));
                    document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1));
                    document.ReplaceText("{ThamPhan2}", ThemOngBa(mauInObject.GioiTinhThamPhan2, mauInObject.ThamPhan2));
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NhomAn}", mauInObject.NhomAn);
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                    }
                    else if(mauInObject.GiaiDoan==1)
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                        tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamPhanCong}", NgayThangNam(mauInObject.NgayPhanCong));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{QuanHePhapLuat}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ChanhAn);

                    document.SaveAs(filePath);
                }
                if (mauInObject.ChanhAn.ToLower() == Setting.CHANH_AN_TINH.ToLower())
                {
                    InsertImageSpire(filePath, pathChuKy);
                }
                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn0613QuyetDinhPCTPDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauIn08QuyetDinhDuaVuAnXetXuSoThamDoc(MauInSo47ViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguyenDon = GetParagraph(document, "{NguyenDon}");
                    Xceed.Words.NET.Paragraph BiDon = GetParagraph(document, "{BiDon}");
                    Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                    Xceed.Words.NET.Paragraph NguoiThamGiaToTung = GetParagraph(document, "{NguoiThamGiaToTung}");
                    string thamphandukhuyet = "", hoithamdukhuyet = "", thukydukhuyet = "", ksvdukhuyet = "";
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {
                        var kk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON));
                        if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                            kk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU));
                        if (kk != null && kk.Any())
                        {
                            foreach (var item in kk)
                            {
                                if (kk.Count() == 1 || item.Equals(kk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguyenDon.ReplaceText("{NguyenDon}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguyenDon.ReplaceText("{NguyenDon}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguyenDon.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguyenDon.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (kk.Count() == 1 || item.Equals(kk.LastOrDefault()))
                                {
                                    var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (kk.FirstOrDefault().NoiTamTru ?? kk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var bk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_BIDON));
                        if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                            bk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN));
                        if (bk != null && bk.Any())
                        {
                            foreach (var item in bk)
                            {
                                if (bk.Count() == 1 || item.Equals(bk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        BiDon.ReplaceText("{BiDon}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        BiDon.ReplaceText("{BiDon}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = BiDon.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = BiDon.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (bk.Count() == 1 || item.Equals(bk.LastOrDefault()))
                                {
                                    var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (bk.FirstOrDefault().NoiTamTru ?? bk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }
                        else if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                        {
                            BiDon.Remove(false);
                        }
                        if(mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_TRANHCHAP)
                        {
                            var lq = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN));
                            if (lq != null && lq.Any())
                            {
                                NguoiLienQuan.ReplaceText("{NguoiLienQuan}", ViewText.LABEL_NGUOILIENQUAN + ":");
                                foreach (var item in lq)
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                            }
                            else
                            {
                                NguoiLienQuan.Remove(false);
                            }
                        }

                        var ntg = mauInObject.DanhSachDuongSu.Where(x=> !x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON) && !x.TuCachThamGiaToTung.Equals(ViewText.LABEL_BIDON) && !x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN) && !x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU));
                        if(ntg!=null && ntg.Any())
                        {
                            NguoiThamGiaToTung.ReplaceText("{NguoiThamGiaToTung}", "");
                            foreach (var item in ntg)
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }
                        }
                        else
                        {
                            NguoiThamGiaToTung.ReplaceText("{NguoiThamGiaToTung}", "Không.");
                        }
                    }
                    else
                    {
                        NguyenDon.ReplaceText("{NguoiKhoiKien}", "Ông (Bà) ..........................................");
                        NguyenDon.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguyenDon.IndentationFirstLine = 1.0f;

                        BiDon.ReplaceText("{NguoiBiKien}", "Ông (Bà) ..........................................");
                        BiDon.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        BiDon.IndentationFirstLine = 1.0f;

                        NguoiLienQuan.Remove(false);
                    }

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() == 1 || item == mauInObject.DanhSachThamPhanDuKhuyet.FirstOrDefault())
                                thamphandukhuyet += ThemOngBa(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else if(mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet == null || !mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Any())
                        hoithamdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachHoiThamNhanDanDuKhuyet)
                        {
                            if (mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Count() == 1 || item == mauInObject.DanhSachHoiThamNhanDanDuKhuyet.FirstOrDefault())
                                hoithamdukhuyet += ThemOngBa(item.GioiTinh, item.HoiThamNhanDanDuKhuyet) + ", ";
                            else if(mauInObject.DanhSachHoiThamNhanDanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachHoiThamNhanDanDuKhuyet.LastOrDefault())
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet) + ", ";
                            else
                                hoithamdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.HoiThamNhanDanDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachThuKyDuKhuyet == null || !mauInObject.DanhSachThuKyDuKhuyet.Any())
                        thukydukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThuKyDuKhuyet)
                        {
                            if (mauInObject.DanhSachThuKyDuKhuyet.Count() == 1 || item == mauInObject.DanhSachThuKyDuKhuyet.FirstOrDefault())
                                thukydukhuyet += ThemOngBa(item.GioiTinh, item.ThuKyDuKhuyet) + ", ";
                            else if (mauInObject.DanhSachThuKyDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThuKyDuKhuyet.LastOrDefault())
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet) + ", ";
                            else
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachKiemSatVienDuKhuyet == null || !mauInObject.DanhSachKiemSatVienDuKhuyet.Any())
                        ksvdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachKiemSatVienDuKhuyet)
                        {
                            if (mauInObject.DanhSachKiemSatVienDuKhuyet.Count() == 1 || item == mauInObject.DanhSachKiemSatVienDuKhuyet.FirstOrDefault())
                                ksvdukhuyet += ThemOngBa(item.GioiTinh, item.KiemSatVienDuKhuyet) + ", ";
                            else if (mauInObject.DanhSachKiemSatVienDuKhuyet.Count() > 1 && item != mauInObject.DanhSachKiemSatVienDuKhuyet.LastOrDefault())
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet) + ", ";
                            else
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet);
                        }
                    }
                    //replace               



                    document.ReplaceText("{HoiThamNhanDan1}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan, mauInObject.HoiThamNhanDan));
                    document.ReplaceText("{HoiThamNhanDan2}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2));
                    document.ReplaceText("{HoiThamDuKhuyet}", hoithamdukhuyet);
                    if (mauInObject.HoiDong == 2)
                    {
                        document.ReplaceText("{ThamPhanKhac}", ThemOngBa(mauInObject.GioiTinhThamPhanKhac, mauInObject.ThamPhanKhac) + ", chức danh Thẩm phán.");
                        document.ReplaceText("{HoiThamNhanDan3}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3) + ", chức danh Hội thẩm.");
                    }
                    else if(mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_TRANHCHAP)
                    {
                        var tempParagraph1 = GetParagraph(document, "{ThamPhanKhac}");
                        var tempParagraph2 = GetParagraph(document, "{HoiThamNhanDan3}");
                        tempParagraph1.Remove(false);
                        tempParagraph2.Remove(false);
                    }
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{HoiThamDuKhuyet}", hoithamdukhuyet);
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{ThuKyDuKhuyet}", thukydukhuyet);
                    document.ReplaceText("{KiemSatVien}", ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien));
                    document.ReplaceText("{KiemSatVienDuKhuyet}", ksvdukhuyet);
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                        tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                    document.ReplaceText("{NamXetXu}", mauInObject.NgayRaQuyetDinhXetXu.Year.ToString());
                    document.ReplaceText("{NgayXetXu}", NgayThangNam(mauInObject.NgayRaQuyetDinhXetXu));
                    document.ReplaceText("{QuanHePhapLuat}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{DiaDiemMoPhienToa}", mauInObject.DiaDiemMoPhienToa);
                    document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                    document.ReplaceText("{VuAnDuocXetXu}", mauInObject.VuAnDuocXetXu.ToLower());
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhanChuToa);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInHCQuyetDinhDuaVuAnXetXuSoThamDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauIn09GiayTrieuTapDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath)
        {
            string filePathTemp = filePath.Replace(".docx", "_temp_.docx");
            string filePathTemp2 = filePath.Replace(".docx", "_temp2_.docx");
            try
            {
                Formatting format = new Formatting
                {
                    FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                    Size = 14
                };
                string duongsudauvu = mauInObject.TenVuAn;
                if (duongsudauvu.Count(x => x == '-') > 1)
                    duongsudauvu = duongsudauvu.Remove(duongsudauvu.LastIndexOf(" - "));
                foreach (var item in mauInObject.DanhSachDuongSu)
                {
                    if (item == mauInObject.DanhSachDuongSu.FirstOrDefault() || mauInObject.DanhSachDuongSu.Count == 1)
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            document.ApplyTemplate(templatePath);
                            document.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            document.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                                tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            document.ReplaceText("{TuCachThamGiaToTung}", item.TuCachThamGiaToTung);
                            document.ReplaceText("{QuanHePhapLuat}", mauInObject.QuanHePhapLuat);
                            document.ReplaceText("{NguyenDon}", mauInObject.TenNguyenDon);
                            if(mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_TRANHCHAP)
                                document.ReplaceText("{BiDon}", mauInObject.TenBiDon);
                            else
                            {
                                if(mauInObject.TenBiDon=="")
                                {
                                    document.ReplaceText("{Giua/Cua}", "của");
                                    document.ReplaceText("{BiDon}", mauInObject.TenBiDon);
                                    document.ReplaceText("{VoiNguoiLienQuan}", ".");
                                }
                                else
                                {
                                    document.ReplaceText("{Giua/Cua}", "giữa");
                                    document.ReplaceText("{BiDon}", mauInObject.TenBiDon);
                                    document.ReplaceText("{VoiNguoiLienQuan}", " với " +ViewText.LABEL_NGUOILIENQUAN.ToLower());
                                }
                            }
                            document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            document.ReplaceText("{GiaiDoan}", mauInObject.GiaiDoan==1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower());
                            document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            document.ReplaceText("{ThamPhan}", mauInObject.ThamPhan);
                            document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                            document.ReplaceText("{NamXetXu}", mauInObject.ThoiGianMoPhienToa.Year.ToString());
                            document.ReplaceText("{TenVuAn}", duongsudauvu);

                            document.SaveAs(filePath);
                        }
                    }
                    else
                    {
                        using (DocX temp = DocX.Create(filePathTemp))
                        {
                            temp.ApplyTemplate(templatePath);

                            temp.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            temp.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            temp.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                            {
                                temp.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(temp, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                                tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                            }
                            temp.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            temp.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                temp.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                temp.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                            }
                            temp.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            temp.ReplaceText("{TuCachThamGiaToTung}", item.TuCachThamGiaToTung);
                            temp.ReplaceText("{QuanHePhapLuat}", mauInObject.QuanHePhapLuat);
                            temp.ReplaceText("{NguyenDon}", mauInObject.TenNguyenDon);
                            if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                                temp.ReplaceText("{BiDon}", mauInObject.TenBiDon);
                            else
                            {
                                if (mauInObject.TenBiDon == "")
                                {
                                    temp.ReplaceText("{Giua/Cua}", "của");
                                    temp.ReplaceText("{BiDon}", mauInObject.TenBiDon);
                                    temp.ReplaceText("{VoiNguoiLienQuan}", ".");
                                }
                                else
                                {
                                    temp.ReplaceText("{Giua/Cua}", "giữa");
                                    temp.ReplaceText("{BiDon}", mauInObject.TenBiDon);
                                    temp.ReplaceText("{VoiNguoiLienQuan}", " với "+ViewText.LABEL_NGUOILIENQUAN.ToLower());
                                }
                            }
                            temp.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            temp.ReplaceText("{GiaiDoan}", mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower());
                            temp.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            temp.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            temp.ReplaceText("{ThamPhan}", mauInObject.ThamPhan);
                            temp.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                            temp.ReplaceText("{NamXetXu}", mauInObject.ThoiGianMoPhienToa.Year.ToString());
                            temp.ReplaceText("{TenVuAn}", duongsudauvu);
                            temp.SaveAs(filePathTemp);

                            File.Copy(filePath, filePathTemp2);

                            List<Source> sources = new List<Source>();

                            sources.Add(new Source(new WmlDocument(filePathTemp2), true));

                            sources.Add(new Source(new WmlDocument(filePathTemp), true));

                            DocumentBuilder.BuildDocument(sources, filePath);


                            File.Delete(filePathTemp2);
                            File.Delete(filePathTemp);
                        }
                    }
                }


                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn09GiayTrieuTapDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauIn0716BiaHoSoDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {                
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    Xceed.Words.NET.Paragraph NguyenDon = GetParagraph(document, "{NguyenDon}");
                    Xceed.Words.NET.Paragraph BiDon = GetParagraph(document, "{BiDon}");                    
                    int i = 1;
                    foreach (var item in mauInObject.DanhSachNguyenDon)
                    {
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (mauInObject.DanhSachNguyenDon.Count() == 1 || item.Equals(mauInObject.DanhSachNguyenDon.FirstOrDefault()))
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguyenDon.ReplaceText("{NguyenDon}", i + ". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                NguyenDon.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                NguyenDon.ReplaceText("{NguyenDon}", i + ". Cơ quan: " + item.TenCoQuanToChuc + ".");
                            }
                            NguyenDon.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguyenDon.AppendLine(i + ". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                                NguyenDon.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                NguyenDon.AppendLine(i + ". Cơ quan: " + item.TenCoQuanToChuc + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                            }
                            NguyenDon.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                        }
                        i++;
                    }
                    i = 1;
                    if(mauInObject.DanhSachBiDon!=null && mauInObject.DanhSachBiDon.Any())
                    {
                        foreach (var item in mauInObject.DanhSachBiDon)
                        {
                            if (item.NgayThangNamSinh == null)
                                item.NgayThangNamSinh = "..........";
                            if (mauInObject.DanhSachBiDon.Count() == 1 || item.Equals(mauInObject.DanhSachBiDon.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    BiDon.ReplaceText("{BiDon}", i + ". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    BiDon.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    BiDon.ReplaceText("{BiDon}", i + ". Cơ quan: " + item.TenCoQuanToChuc + ".");
                                }
                                BiDon.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    BiDon.AppendLine(i + ". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                                    BiDon.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    BiDon.AppendLine(i + ". Cơ quan: " + item.TenCoQuanToChuc + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                                }
                                BiDon.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                            }
                            i++;
                        }
                    }
                    else if(mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_YEUCAU)
                    {
                        BiDon.ReplaceText("{BiDon}","Không.");
                    }
                    i = 1;
                    if(mauInObject.DanhSachNguoiLienQuanTranhChap!=null && mauInObject.DanhSachNguoiLienQuanTranhChap.Any() && mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_TRANHCHAP)
                    {
                        Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                        foreach (var item in mauInObject.DanhSachNguoiLienQuanTranhChap)
                        {
                            if (item.NgayThangNamSinh == null)
                                item.NgayThangNamSinh = "..........";
                            if (mauInObject.DanhSachNguoiLienQuanTranhChap.Count() == 1 || item.Equals(mauInObject.DanhSachNguoiLienQuanTranhChap.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    NguoiLienQuan.ReplaceText("{NguoiLienQuan}", i + ". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    NguoiLienQuan.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    NguoiLienQuan.ReplaceText("{NguoiLienQuan}", i + ". Cơ quan: " + item.TenCoQuanToChuc + ".");
                                }
                                NguoiLienQuan.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    NguoiLienQuan.AppendLine(i + ". Họ tên: " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                                    NguoiLienQuan.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "   Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    NguoiLienQuan.AppendLine(i + ". Cơ quan: " + item.TenCoQuanToChuc + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                                }
                                NguoiLienQuan.AppendLine("   Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                            }
                            i++;
                        }
                    }                        
                    else
                    {
                        document.ReplaceText("{NguoiLienQuan}", "Không.");
                    }

                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                    }
                    else if(mauInObject.GiaiDoan==1)
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                        tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                    }
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{QuanHePhapLuat}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{HoiThamNhanDan1}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan1, mauInObject.HoiThamNhanDan1));
                    document.ReplaceText("{HoiThamNhanDan2}", ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2));
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{KiemSatVien}", mauInObject.KiemSatVien!=null ? ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien) : "Không.");

                    document.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAn != null ? "Bản án" : "Quyết định");
                    document.ReplaceText("{SoBAQD}", mauInObject.SoBanAn != null ? mauInObject.SoBanAn : mauInObject.SoQuyetDinh);
                    document.ReplaceText("{NamBAQD}", mauInObject.SoBanAn != null ? mauInObject.NgayRaBanAn.Year.ToString() : mauInObject.NgayRaQuyetDinh.Year.ToString());
                    document.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAn != null ? NgayThangNam(mauInObject.NgayRaBanAn) : NgayThangNam(mauInObject.NgayRaQuyetDinh));
                    if (mauInObject.HoiDong == 2)
                        document.ReplaceText("{HoiThamNhanDan3}", "3 " + ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3));
                    else if (mauInObject.GiaiDoan == 1 && mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_TRANHCHAP)
                    {
                        var tempParagraph = GetParagraph(document, "{HoiThamNhanDan3}");
                        tempParagraph.Remove(false);
                    }
                    if (mauInObject.GiaiDoan==2)
                    {
                        document.ReplaceText("{ToaAnST}", mauInObject.ToaAnSoTham);
                        document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1));
                        document.ReplaceText("{ThamPhan2}", ThemOngBa(mauInObject.GioiTinhThamPhan2, mauInObject.ThamPhan2));
                        document.ReplaceText("{BanAn/QuyetDinhPT}", mauInObject.SoBanAnPT != null ? "Bản án" : "Quyết định");
                        document.ReplaceText("{SoBAQDPT}", mauInObject.SoBanAnPT != null ? mauInObject.SoBanAnPT : mauInObject.SoQuyetDinhPT);
                        document.ReplaceText("{NamBAQDPT}", mauInObject.SoBanAnPT != null ? mauInObject.NgayRaBanAnPT.Year.ToString() : mauInObject.NgayRaQuyetDinhPT.Year.ToString());
                        document.ReplaceText("{NgayRaBAQDPT}", mauInObject.SoBanAnPT != null ? NgayThangNam(mauInObject.NgayRaBanAnPT) : NgayThangNam(mauInObject.NgayRaQuyetDinhPT));
                    }
                    if (mauInObject.DanhSachNguoiKhangCao != null && mauInObject.DanhSachNguoiKhangCao.Any())
                        document.ReplaceText("{NgayKhangCao}", NgayThangNam(mauInObject.DanhSachNguoiKhangCao.LastOrDefault().NgayNop).Replace("ngày", "Ngày"));
                    else
                        document.ReplaceText("{NgayKhangCao}", "Ngày......tháng.......năm..........");
                    if (mauInObject.VienKiemSatKhangNghi != null)
                        document.ReplaceText("{NgayKhangNghi}", NgayThangNam(mauInObject.NgayKhangNghi).Replace("ngày", "Ngày"));
                    else
                        document.ReplaceText("{NgayKhangNghi}", "Ngày......tháng.......năm..........");
                    if (mauInObject.VienKiemSatKhangNghi == null && (mauInObject.DanhSachNguoiKhangCao == null || !mauInObject.DanhSachNguoiKhangCao.Any()))
                        document.ReplaceText("{NgayHieuLuc}", NgayThangNam(mauInObject.HieuLuc));
                    else
                        document.ReplaceText("{NgayHieuLuc}", "ngày......tháng.......năm..........");
                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn0716BiaHoSoDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        public string[] MauIn11ThongBaoNopTienTamUngPTDoc(MauInSo61ViewModel mauInObject, string filePath, string templatePath)
        {
            
            string filePathTemp = filePath.Replace(".docx", "_temp_.docx");
            string filePathTemp2 = filePath.Replace(".docx", "_temp2_.docx");
            try
            {
                // Create a new document.

                foreach (var item in mauInObject.DanhSachNguoiKhangCao)
                {
                    if (item == mauInObject.DanhSachNguoiKhangCao.FirstOrDefault() || mauInObject.DanhSachNguoiKhangCao.Count == 1)
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            // Apply a template to the document based on a path.
                            document.ApplyTemplate(templatePath);

                            Formatting format = new Formatting
                            {
                                FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                                Size = 14
                            };

                            //replace
                            document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                            document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo.ToString());
                            document.ReplaceText("{NamRaThongBao}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                                tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{NguoiKhangCao}", ThemOngBa_ChuThuong(item.GioiTinh,item.HoTenDuongSu));
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{NguoiKhangCao}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{CoQuanThiHanhAnThu}", mauInObject.CoQuanThiHanhAnThu);
                            document.ReplaceText("{DiaChiCoQuanThiHanhAnThu}", mauInObject.DiaChiCoQuanThiHanhAnThu);
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);
                            document.ReplaceText("{GiaTriDuNopFormatted}", mauInObject.GiaTriDuNopFormatted);
                            document.ReplaceText("{GiaTriDuNopBangChu}", mauInObject.GiaTriDuNopBangChu);

                            // Save this document to disk.
                            document.SaveAs(filePath);
                            //}
                        }
                    }
                    else
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            // Apply a template to the document based on a path.
                            document.ApplyTemplate(templatePath);

                            Formatting format = new Formatting
                            {
                                FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                                Size = 14
                            };

                            //replace
                            document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                            document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo.ToString());
                            document.ReplaceText("{NamRaThongBao}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{DuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu);
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{DuongSu}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                               tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment=Alignment.center;
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                document.ReplaceText("{NguoiKhangCao}", ThemOngBa_ChuThuong(item.GioiTinh, item.HoTenDuongSu));
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                document.ReplaceText("{NguoiKhangCao}", item.TenCoQuanToChuc);
                            }
                            document.ReplaceText("{CoQuanThiHanhAnThu}", mauInObject.CoQuanThiHanhAnThu);
                            document.ReplaceText("{DiaChiCoQuanThiHanhAnThu}", mauInObject.DiaChiCoQuanThiHanhAnThu);
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);
                            document.ReplaceText("{GiaTriDuNopFormatted}", mauInObject.GiaTriDuNopFormatted);
                            document.ReplaceText("{GiaTriDuNopBangChu}", mauInObject.GiaTriDuNopBangChu);

                            document.SaveAs(filePathTemp);

                            File.Copy(filePath, filePathTemp2);

                            List<Source> sources = new List<Source>();

                            sources.Add(new Source(new WmlDocument(filePathTemp2), true));

                            sources.Add(new Source(new WmlDocument(filePathTemp), true));

                            DocumentBuilder.BuildDocument(sources, filePath);


                            File.Delete(filePathTemp2);
                            File.Delete(filePathTemp);
                        }
                    }
                }

                

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn11ThongBaoNopTienTamUngPTDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauIn12ThongBaoThuLyPTDoc(MauInSo65ViewModel mauInObject, string filePath, string templatePath)
        {
            string tailieu = "", noidungkc = "", kckn="", dkcqdkn="";
            try
            {
                foreach (var item in mauInObject.NguoiKhangCao)
                {
                    tailieu += item.TaiLieuChungTuKemTheo;
                }
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);
                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph DanhSachDuongSu = GetParagraph(document, "{DanhSachDuongSu}");
                    Xceed.Words.NET.Paragraph NguoiKhangCao = GetParagraph(document, "{NguoiKhangCao}");
                    Xceed.Words.NET.Paragraph VKSKhangNghi = GetParagraph(document, "{VKSKhangNghi}");
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {
                        foreach (var item in mauInObject.DanhSachDuongSu)
                        {
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    DanhSachDuongSu.ReplaceText("{DanhSachDuongSu}","- " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    DanhSachDuongSu.ReplaceText("{DanhSachDuongSu}", "- " + item.TenCoQuanToChuc + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = DanhSachDuongSu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 3.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = DanhSachDuongSu.InsertParagraphAfterSelf("- " + (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 3.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = DanhSachDuongSu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 3.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = DanhSachDuongSu.InsertParagraphAfterSelf("- " + item.TenCoQuanToChuc + ".", false, format);
                                    p2.IndentationFirstLine = 3.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.LastOrDefault()))
                            {
                                var p1 = DanhSachDuongSu.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachDuongSu.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachDuongSu.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 3.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                            }
                        }
                    }
                    else
                    {
                        DanhSachDuongSu.ReplaceText("{DanhSachDuongSu}", "Ông (Bà) ..........................................");
                        DanhSachDuongSu.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        DanhSachDuongSu.IndentationFirstLine = 1.0f;
                    }

                    if (mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any())
                    {
                        kckn = ViewText.LABEL_KHANGCAO.ToLower();
                        dkcqdkn = "đơn kháng cáo";
                        NguoiKhangCao.ReplaceText("{NguoiKhangCao}", "Theo đơn kháng cáo của: ");
                        foreach (var item in mauInObject.NguoiKhangCao)
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguoiKhangCao.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCao.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguoiKhangCao.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCao.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                            noidungkc += item.NoidungKhangCao;
                        }
                    }
                    else
                    {
                        NguoiKhangCao.Remove(false);
                    }

                    if (mauInObject.VienKiemSatKhangNghi != null)
                    {
                        if (kckn == "")
                            kckn = ViewText.LABEL_KHANGNGHI.ToLower();
                        else
                            kckn += ", kháng nghị";
                        if (dkcqdkn == "")
                            dkcqdkn = "quyết định kháng nghị";
                        else
                            dkcqdkn += ", quyết định kháng nghị";
                        noidungkc += mauInObject.NoiDungKhangNghi;
                        VKSKhangNghi.ReplaceText("{VKSKhangNghi}", "Theo Quyết định kháng nghị của " + mauInObject.VienKiemSatKhangNghi + ".");
                    }
                    else
                    {
                        VKSKhangNghi.Remove(false);
                    }
                    

                    //replace    
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy).Replace("ngày", "Ngày"));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{ToaAnST}", mauInObject.ToaAnSoTham);
                    document.ReplaceText("{DonKC/QDKN}", dkcqdkn);
                    document.ReplaceText("{KhangCaoKhangNghi}", kckn);
                    document.ReplaceText("{QuanHePhapLuat}", mauInObject.QuanHePhapLuat);
                    if (String.IsNullOrEmpty(noidungkc))
                        document.ReplaceText("{NoiDungKhangCaoKhangNghi}", "");
                    else
                    {
                        var noiDungKhangCaoKhangNghi = StripHtmlTags(noidungkc);
                        Xceed.Words.NET.Paragraph NoiDungKhangCaoKhangNghi = GetParagraph(document, "{NoiDungKhangCaoKhangNghi}");
                        foreach (var item in noiDungKhangCaoKhangNghi)
                        {
                            if (noiDungKhangCaoKhangNghi.Count == 1 || item == noiDungKhangCaoKhangNghi.FirstOrDefault())
                                NoiDungKhangCaoKhangNghi.ReplaceText("{NoiDungKhangCaoKhangNghi}", item);
                            else
                            {
                                NoiDungKhangCaoKhangNghi.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                            }
                        }
                    }
                    if (tailieu!=null && tailieu!="")
                    {
                        document.ReplaceText("{Da/Khong}", "đã");
                        document.ReplaceText("{SauDay}", "sau đây:");
                        var taiLieuChungTuKemTheo = StripHtmlTags(tailieu);
                        Xceed.Words.NET.Paragraph TaiLieuChungTuKemTheo = GetParagraph(document, "{TaiLieuChungTuKemTheo}");
                        foreach (var item in taiLieuChungTuKemTheo)
                        {
                            if (taiLieuChungTuKemTheo.Count == 1 || item == taiLieuChungTuKemTheo.FirstOrDefault())
                                TaiLieuChungTuKemTheo.ReplaceText("{TaiLieuChungTuKemTheo}", item);
                            else
                            {
                                TaiLieuChungTuKemTheo.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                            }
                        }
                    }
                    else
                    {
                        document.ReplaceText("{Da/Khong}", "không");
                        document.ReplaceText("{SauDay}", ".");
                        var temp = GetParagraph(document, "{TaiLieuChungTuKemTheo}");
                        temp.Remove(false);
                    }
                    document.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAn != null ? "bản án" : "quyết định");
                    document.ReplaceText("{SoBAQD}", mauInObject.SoBanAn != null ? mauInObject.SoBanAn : mauInObject.SoQuyetDinh);
                    document.ReplaceText("{NamBAQD}", mauInObject.SoBanAn != null ? mauInObject.NgayTuyenAn.Year.ToString() : mauInObject.NgayRaQuyetDinh.Year.ToString());
                    document.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAn != null ? NgayThangNam(mauInObject.NgayTuyenAn) : NgayThangNam(mauInObject.NgayRaQuyetDinh));
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn12ThongBaoThuLyPTDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauIn14QuyetDinhDuaVuAnXetXuPhucThamDoc(MauInSo47PTViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguyenDon = GetParagraph(document, "{NguyenDon}");
                    Xceed.Words.NET.Paragraph BiDon = GetParagraph(document, "{BiDon}");
                    Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                    Xceed.Words.NET.Paragraph NguoiKhangCaoKhangNghi = GetParagraph(document, "{NguoiKhangCaoKhangNghi}");
                    Xceed.Words.NET.Paragraph NguoiThamGiaToTung = GetParagraph(document, "{NguoiThamGiaToTung}");
                    string thamphandukhuyet = "", thukydukhuyet = "", ksvdukhuyet = "";
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {
                        var kk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON));
                        if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                            kk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU));
                        if (kk != null && kk.Any())
                        {
                            foreach (var item in kk)
                            {
                                if (kk.Count() == 1 || item.Equals(kk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguyenDon.ReplaceText("{NguyenDon}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguyenDon.ReplaceText("{NguyenDon}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguyenDon.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguyenDon.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (kk.Count() == 1 || item.Equals(kk.LastOrDefault()))
                                {
                                    var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (kk.FirstOrDefault().NoiTamTru ?? kk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }

                        var bk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_BIDON));
                        if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                            bk = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN));
                        if (bk != null && bk.Any())
                        {
                            foreach (var item in bk)
                            {
                                if (bk.Count() == 1 || item.Equals(bk.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        BiDon.ReplaceText("{BiDon}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        BiDon.ReplaceText("{BiDon}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = BiDon.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = BiDon.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                                if (bk.Count() == 1 || item.Equals(bk.LastOrDefault()))
                                {
                                    var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (bk.FirstOrDefault().NoiTamTru ?? bk.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                }
                            }
                        }
                        else if(mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_YEUCAU)
                        {
                            BiDon.Remove(false);
                        }
                        if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                        {
                            var lq = mauInObject.DanhSachDuongSu.Where(x => x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN));
                            if (lq != null && lq.Any())
                            {
                                NguoiLienQuan.ReplaceText("{NguoiLienQuan}", ViewText.LABEL_NGUOILIENQUAN + ":");
                                foreach (var item in lq)
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.IndentationFirstLine = 1.0f;
                                        //p1.SpacingBefore(6);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        p2.IndentationFirstLine = 1.0f;
                                        //p2.SpacingBefore(6);
                                        p2.SpacingAfter(6);
                                    }
                                }
                            }
                            else
                            {
                                NguoiLienQuan.Remove(false);
                            }
                        }
                        var ntg = mauInObject.DanhSachDuongSu.Where(x => !x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUYENDON) && !x.TuCachThamGiaToTung.Equals(ViewText.LABEL_BIDON) && !x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOILIENQUAN) && !x.TuCachThamGiaToTung.Equals(ViewText.LABEL_NGUOIYEUCAU));
                        if (ntg != null && ntg.Any())
                        {
                            NguoiThamGiaToTung.ReplaceText("{NguoiThamGiaToTung}", "");
                            foreach (var item in ntg)
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.IndentationFirstLine = 1.0f;
                                    //p1.SpacingBefore(6);
                                    p1.SpacingAfter(6);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    p2.IndentationFirstLine = 1.0f;
                                    //p2.SpacingBefore(6);
                                    p2.SpacingAfter(6);
                                }
                            }
                        }
                        else
                        {
                            NguoiThamGiaToTung.ReplaceText("{NguoiThamGiaToTung}", "Không.");
                        }
                    }
                    else
                    {
                        NguyenDon.ReplaceText("{NguyenDon}", "Ông (Bà) ..........................................");
                        NguyenDon.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        NguyenDon.IndentationFirstLine = 1.0f;

                        BiDon.ReplaceText("{BiDon}", "Ông (Bà) ..........................................");
                        BiDon.InsertParagraphAfterSelf("Địa chỉ: ..................................................", false, format);
                        BiDon.IndentationFirstLine = 1.0f;

                        NguoiLienQuan.Remove(false);
                    }

                    if (mauInObject.VienKiemSatKhangNghi != null && (mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any()))
                    {
                        document.ReplaceText("{KhangCao/KhangNghi}", "kháng cáo, kháng nghị");
                    }
                    else if ((mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any()) && mauInObject.VienKiemSatKhangNghi == null)
                    {
                        document.ReplaceText("{KhangCao/KhangNghi}", "kháng cáo");
                    }
                    else
                    {
                        document.ReplaceText("{KhangCao/KhangNghi}", "kháng nghị");
                    }

                    if (mauInObject.VienKiemSatKhangNghi != null)
                    {
                        NguoiKhangCaoKhangNghi.ReplaceText("{NguoiKhangCaoKhangNghi}", mauInObject.VienKiemSatKhangNghi + ".");
                    }
                    else
                    {
                        NguoiKhangCaoKhangNghi.ReplaceText("{NguoiKhangCaoKhangNghi}", "");
                    }
                    if (mauInObject.NguoiKhangCao != null && mauInObject.NguoiKhangCao.Any())
                    {
                        foreach (var item in mauInObject.NguoiKhangCao)
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiKhangCaoKhangNghi.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                        }
                    }

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachThuKyDuKhuyet == null || !mauInObject.DanhSachThuKyDuKhuyet.Any())
                        thukydukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThuKyDuKhuyet)
                        {
                            if (mauInObject.DanhSachThuKyDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThuKyDuKhuyet.LastOrDefault())
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet) + ", ";
                            else
                                thukydukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThuKyDuKhuyet);
                        }
                    }

                    if (mauInObject.DanhSachKiemSatVienDuKhuyet == null || !mauInObject.DanhSachKiemSatVienDuKhuyet.Any())
                        ksvdukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachKiemSatVienDuKhuyet)
                        {
                            if (mauInObject.DanhSachKiemSatVienDuKhuyet.Count() > 1 && item != mauInObject.DanhSachKiemSatVienDuKhuyet.LastOrDefault())
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet) + ", ";
                            else
                                ksvdukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.KiemSatVienDuKhuyet);
                        }
                    }

                    //replace               

                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1));
                    document.ReplaceText("{ThamPhan2}", ThemOngBa(mauInObject.GioiTinhThamPhan2, mauInObject.ThamPhan2));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{ThuKyDuKhuyet}", thukydukhuyet);
                    document.ReplaceText("{KiemSatVien}", ThemOngBa(mauInObject.GioiTinhKiemSatVien, mauInObject.KiemSatVien));
                    document.ReplaceText("{KiemSatVienDuKhuyet}", ksvdukhuyet);
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                    document.ReplaceText("{NamXetXu}", mauInObject.NgayRaQuyetDinhXetXu.Year.ToString());
                    document.ReplaceText("{NgayXetXu}", NgayThangNam(mauInObject.NgayRaQuyetDinhXetXu));
                    document.ReplaceText("{QuanHePhapLuat}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{DiaDiemMoPhienToa}", mauInObject.DiaDiemMoPhienToa);
                    document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                    document.ReplaceText("{VuAnDuocXetXu}", mauInObject.VuAnDuocXetXu.ToLower());
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhanChuToa);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn14QuyetDinhDuaVuAnXetXuPhucThamDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInKDBiaHoSoDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                string hoitham = ThemOngBa(mauInObject.GioiTinhHoiThamNhanDan1, mauInObject.HoiThamNhanDan1) + ", " + ThemOngBa_ChuThuong(mauInObject.GioiTinhHoiThamNhanDan2, mauInObject.HoiThamNhanDan2);
                if (mauInObject.HoiDong == 2)
                    hoitham += ", " + ThemOngBa_ChuThuong(mauInObject.GioiTinhHoiThamNhanDan3, mauInObject.HoiThamNhanDan3);
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    Xceed.Words.NET.Paragraph NguyenDon = GetParagraph(document, "{NguyenDon}");
                    Xceed.Words.NET.Paragraph BiDon = GetParagraph(document, "{BiDon}");
                    foreach (var item in mauInObject.DanhSachNguyenDon)
                    {
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (mauInObject.DanhSachNguyenDon.Count() == 1 || item.Equals(mauInObject.DanhSachNguyenDon.FirstOrDefault()))
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguyenDon.ReplaceText("{NguyenDon}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                NguyenDon.ReplaceText("{NguyenDon}", item.TenCoQuanToChuc + ".");
                            }
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                NguyenDon.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                var p2 = NguyenDon.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                //p2.SpacingBefore(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                var p2 = NguyenDon.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                //p2.SpacingBefore(6);
                            }
                        }
                        if (mauInObject.DanhSachNguyenDon.Count() == 1 || item.Equals(mauInObject.DanhSachNguyenDon.LastOrDefault()))
                        {
                            var p1 = NguyenDon.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachNguyenDon.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachNguyenDon.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                            p1.SpacingAfter(6);
                            if (mauInObject.DanhSachNguyenDon.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                NguyenDon.InsertParagraphAfterSelf((mauInObject.DanhSachNguyenDon.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + mauInObject.DanhSachNguyenDon.FirstOrDefault().NgayThangNamSinh, false, format);
                            }
                        }
                    }

                    foreach (var item in mauInObject.DanhSachBiDon)
                    {
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (mauInObject.DanhSachBiDon.Count() == 1 || item.Equals(mauInObject.DanhSachBiDon.FirstOrDefault()))
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                BiDon.ReplaceText("{BiDon}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                BiDon.ReplaceText("{BiDon}", item.TenCoQuanToChuc + ".");
                            }
                        }
                        else
                        {
                            if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                BiDon.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                var p2 = BiDon.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                //p2.SpacingBefore(6);

                            }
                            else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                            {
                                var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                var p2 = BiDon.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                //p2.SpacingBefore(6);
                            }
                        }
                        if (mauInObject.DanhSachBiDon.Count() == 1 || item.Equals(mauInObject.DanhSachBiDon.LastOrDefault()))
                        {
                            var p1 = BiDon.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachBiDon.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachBiDon.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                            p1.SpacingAfter(6);
                            if (mauInObject.DanhSachBiDon.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                            {
                                BiDon.InsertParagraphAfterSelf((mauInObject.DanhSachBiDon.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + mauInObject.DanhSachBiDon.FirstOrDefault().NgayThangNamSinh, false, format);
                            }
                        }
                    }

                    if (mauInObject.DanhSachNguoiLienQuanTranhChap != null && mauInObject.DanhSachNguoiLienQuanTranhChap.Any() && mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_TRANHCHAP)
                    {
                        Xceed.Words.NET.Paragraph NguoiLienQuan = GetParagraph(document, "{NguoiLienQuan}");
                        Xceed.Words.NET.Paragraph NguoiLienQuan2 = GetParagraph(document, "{NguoiLienQuan2}");
                        var lq = mauInObject.DanhSachNguoiLienQuanTranhChap.ToList();
                        var lq2 = new List<DuongSuModel>();
                        if (lq.Count > 1)
                            for (int i = 0; i < lq.Count; i++)
                            {
                                if (i % 2 != 0)
                                {
                                    lq2.Add(lq[i]);
                                    lq.Remove(lq[i]);
                                }
                            }

                        foreach (var item in lq)
                        {
                            if (item.NgayThangNamSinh == null)
                                item.NgayThangNamSinh = "..........";
                            if (lq.Count() == 1 || item.Equals(lq.FirstOrDefault()))
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    NguoiLienQuan.ReplaceText("{NguoiLienQuan}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    NguoiLienQuan.ReplaceText("{NguoiLienQuan}", item.TenCoQuanToChuc + ".");
                                }
                            }
                            else
                            {
                                if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.SpacingAfter(6);
                                    NguoiLienQuan.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                    //p2.SpacingBefore(6);

                                }
                                else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                {
                                    var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                    p1.SpacingAfter(6);
                                    var p2 = NguoiLienQuan.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                    //p2.SpacingBefore(6);
                                }
                            }
                            if (lq.Count() == 1 || item.Equals(lq.LastOrDefault()))
                            {
                                var p1 = NguoiLienQuan.InsertParagraphAfterSelf("Địa chỉ: " + (lq.FirstOrDefault().NoiTamTru ?? lq.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.SpacingAfter(6);
                                if (lq.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                                {
                                    NguoiLienQuan.InsertParagraphAfterSelf((lq.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + lq.FirstOrDefault().NgayThangNamSinh, false, format);
                                }
                            }
                        }

                        if (lq2.Count > 0)
                            foreach (var item in lq2)
                            {
                                if (item.NgayThangNamSinh == null)
                                    item.NgayThangNamSinh = "..........";

                                if (lq2.Count() == 1 || item.Equals(lq2.FirstOrDefault()))
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");
                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", item.TenCoQuanToChuc + ".");
                                    }
                                }
                                else
                                {
                                    if (item.DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        var p1 = NguoiLienQuan2.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.SpacingAfter(6);
                                        NguoiLienQuan2.InsertParagraphAfterSelf((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + item.NgayThangNamSinh, false, format);
                                        var p2 = NguoiLienQuan2.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                        //p2.SpacingBefore(6);

                                    }
                                    else if (item.DuongSuLa == Setting.DUONGSULA_TOCHUC)
                                    {
                                        var p1 = NguoiLienQuan2.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                        p1.SpacingAfter(6);
                                        var p2 = NguoiLienQuan2.InsertParagraphAfterSelf(item.TenCoQuanToChuc + ".", false, format);
                                        //p2.SpacingBefore(6);
                                    }
                                }
                                if (lq2.Count() == 1 || item.Equals(lq2.LastOrDefault()))
                                {
                                    var p1 = NguoiLienQuan2.InsertParagraphAfterSelf("Địa chỉ: " + (lq2.FirstOrDefault().NoiTamTru ?? lq2.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                    p1.SpacingAfter(6);
                                    if (lq2.FirstOrDefault().DuongSuLa == Setting.DUONGSULA_CANHAN)
                                    {
                                        NguoiLienQuan2.InsertParagraphAfterSelf((lq2.FirstOrDefault().NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "Năm sinh: ") + lq2.FirstOrDefault().NgayThangNamSinh, false, format);
                                    }
                                }
                            }
                        else
                            NguoiLienQuan2.ReplaceText("{NguoiLienQuan2}", "");
                    }
                    else
                    {
                        document.ReplaceText("{NguoiLienQuan2}", "");
                        document.ReplaceText("{NguoiLienQuan}", "");
                    }

                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("tỉnh") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", mauInObject.TenToaAn.ToUpper());
                    }
                    else if (mauInObject.GiaiDoan == 1)
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.ReplaceText("{ToaAnTinh}", "");
                        tempToaAnTinhParagraph.InsertParagraphAfterSelf(mauInObject.TenToaAn.ToUpper(), false, format).FontSize(12).Bold().Alignment = Alignment.center;
                    }
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{QuanHePhapLuat}", mauInObject.QuanHePhapLuat);
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{HoiTham}", hoitham);
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy,mauInObject.ThuKy));
                    document.ReplaceText("{BanAn/QuyetDinh}", mauInObject.SoBanAn != null ? "Bản án" : "Quyết định");
                    document.ReplaceText("{SoBAQD}", mauInObject.SoBanAn != null ? mauInObject.SoBanAn : mauInObject.SoQuyetDinh);
                    document.ReplaceText("{NamBAQD}", mauInObject.SoBanAn != null ? mauInObject.NgayRaBanAn.Year.ToString() : mauInObject.NgayRaQuyetDinh.Year.ToString());
                    document.ReplaceText("{NgayRaBAQD}", mauInObject.SoBanAn != null ? NgayThangNam(mauInObject.NgayRaBanAn) : NgayThangNam(mauInObject.NgayRaQuyetDinh));

                    if (mauInObject.GiaiDoan == 2)
                    {
                        document.ReplaceText("{ToaAnST}", mauInObject.ToaAnSoTham);
                        if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                        {
                            document.ReplaceText("{ThamPhan1}", ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1));
                            document.ReplaceText("{ThamPhan2}", ThemOngBa_ChuThuong(mauInObject.GioiTinhThamPhan2, mauInObject.ThamPhan2));
                            document.ReplaceText("{BanAn/QuyetDinhPT}", mauInObject.SoBanAnPT != null ? "Bản án" : "Quyết định");
                            document.ReplaceText("{SoBAQDPT}", mauInObject.SoBanAnPT != null ? mauInObject.SoBanAnPT : mauInObject.SoQuyetDinhPT);
                            document.ReplaceText("{NamBAQDPT}", mauInObject.SoBanAnPT != null ? mauInObject.NgayRaBanAnPT.Year.ToString() : mauInObject.NgayRaQuyetDinhPT.Year.ToString());
                            document.ReplaceText("{NgayRaBAQDPT}", mauInObject.SoBanAnPT != null ? NgayThangNam(mauInObject.NgayRaBanAnPT) : NgayThangNam(mauInObject.NgayRaQuyetDinhPT));
                        }
                        else
                        {
                            document.ReplaceText("{HoiDong}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa) + " - Thẩm phán chủ toạ, "
                                + ThemOngBa(mauInObject.GioiTinhThamPhan1, mauInObject.ThamPhan1) + " - Thẩm phán, " + ThemOngBa(mauInObject.GioiTinhThamPhan2, mauInObject.ThamPhan2) + " - Thẩm phán.");
                        }
                    }

                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInKDBiaHoSoDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }
        //AD

        public string[] MauInAD01Doc(MauInSo30ViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);
                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph danhsachduongsu = GetParagraph(document, "{DanhSachDuongSu}");
                    Xceed.Words.NET.Paragraph NguoiBiDeNghi = GetParagraph(document, "{NguoiBiDeNghi}");
                    if (mauInObject.DanhSachDuongSuViewModel != null || mauInObject.DanhSachDuongSuViewModel.Any())
                    {
                        foreach (var item in mauInObject.DanhSachDuongSuViewModel)
                        {
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.FirstOrDefault()))
                            {
                                danhsachduongsu.ReplaceText("{DanhSachDuongSu}", (item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".");                               
                            }
                            else
                            {
                                var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = danhsachduongsu.InsertParagraphAfterSelf((item.GioiTinh == "Nam" ? "Ông " : "Bà ") + item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.LastOrDefault()))
                            {
                                var p1 = danhsachduongsu.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachDuongSuViewModel.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachDuongSuViewModel.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                            }
                        }

                        foreach (var item in mauInObject.DanhSachDuongSuViewModel)
                        {
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.FirstOrDefault()))
                            {
                                NguoiBiDeNghi.ReplaceText("{NguoiBiDeNghi}", item.HoTenDuongSu + ".");
                            }
                            else
                            {
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Con ông: " + (item.HoTenCha ?? "..........") + " và bà: "+(item.HoTenMe ?? "..........") + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nơi cư trú: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                                var p3 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nghề nghiệp: " + (item.NgheNghiep ?? "..........") + ".", false, format);
                                p3.IndentationFirstLine = 1.0f;
                                p3.SpacingBefore(6);
                                p3.SpacingAfter(6);
                                var p4 = NguoiBiDeNghi.InsertParagraphAfterSelf("CMND số: " + (item.SoCMND ?? "..........") + "   Ngày cấp: "+ (item.NgayCapCMND.ToString("dd/MM/yyyy") ?? "..........") + "   Nơi cấp: "+(item.NoiCapCMND ?? "..........") + ".", false, format);
                                p4.IndentationFirstLine = 1.0f;
                                p4.SpacingBefore(6);
                                p4.SpacingAfter(6);
                                var p5 = NguoiBiDeNghi.InsertParagraphAfterSelf("Sinh ngày: " + (item.NgayThangNamSinh ?? "..........") + ".", false, format);
                                p5.IndentationFirstLine = 1.0f;
                                p5.SpacingBefore(6);
                                p5.SpacingAfter(6);
                                var p6 = NguoiBiDeNghi.InsertParagraphAfterSelf( item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                            if (mauInObject.DanhSachDuongSuViewModel.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuViewModel.LastOrDefault()))
                            {
                                var f = mauInObject.DanhSachDuongSuViewModel.FirstOrDefault();
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Con ông: " + (f.HoTenCha ?? "..........") + " và bà: " + (f.HoTenMe ?? "..........") + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nơi cư trú: " + (f.NoiTamTru ?? f.NoiDKHKTT) + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                                var p3 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nghề nghiệp: " + (f.NgheNghiep ?? "..........") + ".", false, format);
                                p3.IndentationFirstLine = 1.0f;
                                p3.SpacingBefore(6);
                                p3.SpacingAfter(6);
                                var p4 = NguoiBiDeNghi.InsertParagraphAfterSelf("CMND số: " + (f.SoCMND ?? "..........") + "   Ngày cấp: " + (f.NgayCapCMND.ToString("dd/MM/yyyy") ?? "..........") + "   Nơi cấp: " + (f.NoiCapCMND ?? "..........") + ".", false, format);
                                p4.IndentationFirstLine = 1.0f;
                                p4.SpacingBefore(6);
                                p4.SpacingAfter(6);
                                var p5 = NguoiBiDeNghi.InsertParagraphAfterSelf("Sinh ngày: " + (f.NgayThangNamSinh ?? "..........") + ".", false, format);
                                p5.IndentationFirstLine = 1.0f;
                                p5.SpacingBefore(6);
                                p5.SpacingAfter(6);                                
                            }
                        }
                    }
                    
                    //replace    
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoThuLy}", mauInObject.ThuLyAD);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayNopDonTaiToaAn.Year.ToString());
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayNopDonTaiToaAn));
                    document.ReplaceText("{NgayNopDon}", NgayThangNam(mauInObject.NgayNopDonTaiToaAn).Replace("ngày", "Ngày"));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{TenCoQuanDeNghi}", mauInObject.TenCoQuanDeNghi);
                    document.ReplaceText("{BienPhapXuLyHanhChinh}", mauInObject.BienPhapXuLyHanhChinh);
                    document.ReplaceText("{NgayNopDonTaiTA}", NgayThangNam(mauInObject.NgayNopDonTaiToaAn));
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInAD01Doc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInADQuyetDinhPCTPDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguoiBiDeNghi = GetParagraph(document, "{NguoiBiDeNghi}");

                    string thamphandukhuyet = "";
                    if (mauInObject.DanhSachDuongSu != null && mauInObject.DanhSachDuongSu.Any())
                    {
                        foreach (var item in mauInObject.DanhSachDuongSu)
                        {
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.FirstOrDefault()))
                            {
                                NguoiBiDeNghi.ReplaceText("{NguoiBiDeNghi}", "Họ tên: " + item.HoTenDuongSu + ".");
                            }
                            else
                            {
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiDeNghi.InsertParagraphAfterSelf("Họ tên: " + item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.LastOrDefault()))
                            {
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Địa chỉ: " + (mauInObject.DanhSachDuongSu.FirstOrDefault().NoiTamTru ?? mauInObject.DanhSachDuongSu.FirstOrDefault().NoiDKHKTT) + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                            }
                        }
                    }

                    if (mauInObject.DanhSachThamPhanDuKhuyet == null || !mauInObject.DanhSachThamPhanDuKhuyet.Any())
                        thamphandukhuyet = "Không";
                    else
                    {
                        foreach (var item in mauInObject.DanhSachThamPhanDuKhuyet)
                        {
                            if (mauInObject.DanhSachThamPhanDuKhuyet.Count() > 1 && item != mauInObject.DanhSachThamPhanDuKhuyet.LastOrDefault())
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet) + ", ";
                            else
                                thamphandukhuyet += ThemOngBa_ChuThuong(item.GioiTinh, item.ThamPhanDuKhuyet);
                        }
                    }
                    
                    //replace 
                    var nguoiky = GetParagraph(document, "{NguoiKy}");
                    if (mauInObject.LoaiChanhAn.ToLower() == "chánh án")
                    {
                        nguoiky.ReplaceText("{NguoiKy}", mauInObject.LoaiChanhAn.ToUpper());
                    }
                    else
                    {
                        nguoiky.ReplaceText("{NguoiKy}", "KT. CHÁNH ÁN");
                        nguoiky.AppendLine("PHÓ CHÁNH ÁN").Bold().Font("Times New Roman").FontSize(13);
                    }

                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhan, mauInObject.ThamPhan));
                    document.ReplaceText("{NamThuLyAD}", mauInObject.NgayNopDonTaiToaAn.Year.ToString());
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else if (mauInObject.GiaiDoan == 1)
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayThangNamPhanCong}", NgayThangNam(mauInObject.NgayPhanCong));
                    document.ReplaceText("{ThamPhanDuKhuyet}", thamphandukhuyet);
                    document.ReplaceText("{SoThuLyAD}", mauInObject.ThuLyAD);
                    document.ReplaceText("{NgayNopDon}", NgayThangNam(mauInObject.NgayNopDonTaiToaAn));
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NgayThangNamThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{BienPhapXuLyHanhChinh}", mauInObject.BienPhapXuLyHanhChinh);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ChanhAn);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInADQuyetDinhPCTPDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInADQuyetDinhMoPhienHopSoThamDoc(MauInSo47ViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguoiBiDeNghi = GetParagraph(document, "{NguoiBiDeNghi}");
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {
                        foreach (var item in mauInObject.DanhSachDuongSu)
                        {
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.FirstOrDefault()))
                            {
                                NguoiBiDeNghi.ReplaceText("{NguoiBiDeNghi}", item.HoTenDuongSu + ".");
                            }
                            else
                            {
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Con ông: " + (item.HoTenCha ?? "..........") + " và bà: " + (item.HoTenMe ?? "..........") + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nơi cư trú: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                                var p3 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nghề nghiệp: " + (item.NgheNghiep ?? "..........") + ".", false, format);
                                p3.IndentationFirstLine = 1.0f;
                                p3.SpacingBefore(6);
                                p3.SpacingAfter(6);
                                var p4 = NguoiBiDeNghi.InsertParagraphAfterSelf("CMND số: " + (item.SoCMND ?? "..........") + "   Ngày cấp: " + (item.NgayCapCMND.ToString("dd/MM/yyyy") ?? "..........") + "   Nơi cấp: " + (item.NoiCapCMND ?? "..........") + ".", false, format);
                                p4.IndentationFirstLine = 1.0f;
                                p4.SpacingBefore(6);
                                p4.SpacingAfter(6);
                                var p5 = NguoiBiDeNghi.InsertParagraphAfterSelf("Sinh ngày: " + (item.NgayThangNamSinh ?? "..........") + ".", false, format);
                                p5.IndentationFirstLine = 1.0f;
                                p5.SpacingBefore(6);
                                p5.SpacingAfter(6);
                                var p6 = NguoiBiDeNghi.InsertParagraphAfterSelf(item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.LastOrDefault()))
                            {
                                var f = mauInObject.DanhSachDuongSu.FirstOrDefault();
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Con ông: " + (f.HoTenCha ?? "..........") + " và bà: " + (f.HoTenMe ?? "..........") + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nơi cư trú: " + (f.NoiTamTru ?? f.NoiDKHKTT) + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                                var p3 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nghề nghiệp: " + (f.NgheNghiep ?? "..........") + ".", false, format);
                                p3.IndentationFirstLine = 1.0f;
                                p3.SpacingBefore(6);
                                p3.SpacingAfter(6);
                                var p4 = NguoiBiDeNghi.InsertParagraphAfterSelf("CMND số: " + (f.SoCMND ?? "..........") + "   Ngày cấp: " + (f.NgayCapCMND.ToString("dd/MM/yyyy") ?? "..........") + "   Nơi cấp: " + (f.NoiCapCMND ?? "..........") + ".", false, format);
                                p4.IndentationFirstLine = 1.0f;
                                p4.SpacingBefore(6);
                                p4.SpacingAfter(6);
                                var p5 = NguoiBiDeNghi.InsertParagraphAfterSelf("Sinh ngày: " + (f.NgayThangNamSinh ?? "..........") + ".", false, format);
                                p5.IndentationFirstLine = 1.0f;
                                p5.SpacingBefore(6);
                                p5.SpacingAfter(6);
                            }
                        }
                    }
                    
                    //replace               
                    
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                    document.ReplaceText("{NamXetXu}", mauInObject.NgayRaQuyetDinhXetXu.Year.ToString());
                    document.ReplaceText("{NgayXetXu}", NgayThangNam(mauInObject.NgayRaQuyetDinhXetXu));
                    document.ReplaceText("{BienPhapXuLyHanhChinh}", mauInObject.BienPhapXuLyHanhChinh);
                    document.ReplaceText("{TenCoQuanDeNghi}", mauInObject.TenCoQuanDeNghi);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{SoThuLyAD}", mauInObject.ThuLyAD);
                    document.ReplaceText("{NamThuLyAD}", mauInObject.NgayNopDonTaiToaAn.Year.ToString());
                    document.ReplaceText("{NgayNopDon}", NgayThangNam(mauInObject.NgayNopDonTaiToaAn));
                    document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{DiaDiemMoPhienToa}", mauInObject.DiaDiemMoPhienToa);
                    document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhanChuToa);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInADQuyetDinhMoPhienHopSoThamDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInADGiayTrieuTapDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath)
        {
            string filePathTemp = filePath.Replace(".docx", "_temp_.docx");
            string filePathTemp2 = filePath.Replace(".docx", "_temp2_.docx");
            try
            {
                string duongsudauvu = mauInObject.TenVuAn;
                if (duongsudauvu.Count(x => x == '-') > 1)
                    duongsudauvu = duongsudauvu.Remove(duongsudauvu.LastIndexOf(" - "));
                foreach (var item in mauInObject.DanhSachDuongSu)
                {
                    if (item == mauInObject.DanhSachDuongSu.FirstOrDefault() || mauInObject.DanhSachDuongSu.Count == 1)
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            document.ApplyTemplate(templatePath);
                            document.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            document.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            document.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            document.ReplaceText("{DuongSu}", item.HoTenDuongSu);
                            document.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            document.ReplaceText("{BienPhapXuLyHanhChinh}", mauInObject.BienPhapXuLyHanhChinh);
                            document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            document.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            document.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                            document.ReplaceText("{NamXetXu}", mauInObject.ThoiGianMoPhienToa.Year.ToString());
                            document.ReplaceText("{TenVuAn}", duongsudauvu);
                            document.ReplaceText("{TenTP}", mauInObject.ThamPhan.Substring(mauInObject.ThamPhan.LastIndexOf(" ") + 1));

                            document.SaveAs(filePath);
                        }
                    }
                    else
                    {
                        using (DocX temp = DocX.Create(filePathTemp))
                        {
                            temp.ApplyTemplate(templatePath);

                            temp.ReplaceText("{SoTrieuTap}", item.SoTrieuTap.ToString());
                            temp.ReplaceText("{NamTrieuTap}", mauInObject.NgayRaThongBao.Year.ToString());
                            temp.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                            if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                            {
                                temp.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                            }
                            else
                            {
                                var tempToaAnTinhParagraph = GetParagraph(temp, "{ToaAnTinh}");
                                tempToaAnTinhParagraph.Remove(false);
                            }
                            temp.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                            temp.ReplaceText("{NgayRaThongBao}", NgayThangNam(mauInObject.NgayRaThongBao));
                            temp.ReplaceText("{DuongSu}",  item.HoTenDuongSu);
                            temp.ReplaceText("{DiaChiDuongSu}", item.NoiTamTru ?? item.NoiDKHKTT);
                            temp.ReplaceText("{BienPhapXuLyHanhChinh}", mauInObject.BienPhapXuLyHanhChinh);
                            temp.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                            temp.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                            temp.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                            temp.ReplaceText("{GiaiDoan}", (mauInObject.GiaiDoan == 1 ? ViewText.LABEL_SOTHAM.ToLower() : ViewText.LABEL_PHUCTHAM.ToLower()));
                            temp.ReplaceText("{HoTenNguoiKy}", mauInObject.NguoiKy);
                            temp.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                            temp.ReplaceText("{NamXetXu}", mauInObject.ThoiGianMoPhienToa.Year.ToString());
                            temp.ReplaceText("{TenVuAn}", duongsudauvu);
                            temp.ReplaceText("{TenTP}", mauInObject.ThamPhan.Substring(mauInObject.ThamPhan.LastIndexOf(" ") + 1));
                            temp.SaveAs(filePathTemp);

                            File.Copy(filePath, filePathTemp2);

                            List<Source> sources = new List<Source>();

                            sources.Add(new Source(new WmlDocument(filePathTemp2), true));

                            sources.Add(new Source(new WmlDocument(filePathTemp), true));

                            DocumentBuilder.BuildDocument(sources, filePath);


                            File.Delete(filePathTemp2);
                            File.Delete(filePathTemp);
                        }
                    }
                }


                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInADGiayTrieuTapDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInADQuyetDinhDinhChiDoc(MauInQuyetDinhDinhChiViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                if (mauInObject.SoQuyetDinh == null && mauInObject.NgayRaQuyetDinh <= DateTime.MinValue)
                    return null;
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);
                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguoiBiDeNghi = GetParagraph(document, "{NguoiBiDeNghi}");
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {
                        foreach (var item in mauInObject.DanhSachDuongSu)
                        {
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.FirstOrDefault()))
                            {
                                NguoiBiDeNghi.ReplaceText("{NguoiBiDeNghi}", item.HoTenDuongSu + ".");
                            }
                            else
                            {
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Con ông: " + (item.HoTenCha ?? "..........") + " và bà: " + (item.HoTenMe ?? "..........") + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nơi cư trú: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                                var p3 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nghề nghiệp: " + (item.NgheNghiep ?? "..........") + ".", false, format);
                                p3.IndentationFirstLine = 1.0f;
                                p3.SpacingBefore(6);
                                p3.SpacingAfter(6);
                                var p4 = NguoiBiDeNghi.InsertParagraphAfterSelf("CMND số: " + (item.SoCMND ?? "..........") + "   Ngày cấp: " + (item.NgayCapCMND.ToString("dd/MM/yyyy") ?? "..........") + "   Nơi cấp: " + (item.NoiCapCMND ?? "..........") + ".", false, format);
                                p4.IndentationFirstLine = 1.0f;
                                p4.SpacingBefore(6);
                                p4.SpacingAfter(6);
                                var p5 = NguoiBiDeNghi.InsertParagraphAfterSelf("Sinh ngày: " + (item.NgayThangNamSinh ?? "..........") + ".", false, format);
                                p5.IndentationFirstLine = 1.0f;
                                p5.SpacingBefore(6);
                                p5.SpacingAfter(6);
                                var p6 = NguoiBiDeNghi.InsertParagraphAfterSelf(item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.LastOrDefault()))
                            {
                                var f = mauInObject.DanhSachDuongSu.FirstOrDefault();
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Con ông: " + (f.HoTenCha ?? "..........") + " và bà: " + (f.HoTenMe ?? "..........") + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nơi cư trú: " + (f.NoiTamTru ?? f.NoiDKHKTT) + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                                var p3 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nghề nghiệp: " + (f.NgheNghiep ?? "..........") + ".", false, format);
                                p3.IndentationFirstLine = 1.0f;
                                p3.SpacingBefore(6);
                                p3.SpacingAfter(6);
                                var p4 = NguoiBiDeNghi.InsertParagraphAfterSelf("CMND số: " + (f.SoCMND ?? "..........") + "   Ngày cấp: " + (f.NgayCapCMND.ToString("dd/MM/yyyy") ?? "..........") + "   Nơi cấp: " + (f.NoiCapCMND ?? "..........") + ".", false, format);
                                p4.IndentationFirstLine = 1.0f;
                                p4.SpacingBefore(6);
                                p4.SpacingAfter(6);
                                var p5 = NguoiBiDeNghi.InsertParagraphAfterSelf("Sinh ngày: " + (f.NgayThangNamSinh ?? "..........") + ".", false, format);
                                p5.IndentationFirstLine = 1.0f;
                                p5.SpacingBefore(6);
                                p5.SpacingAfter(6);
                            }
                        }
                    }

                    //replace    
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoQD}", mauInObject.SoQuyetDinh);
                    document.ReplaceText("{NamRaQD}", mauInObject.NgayRaQuyetDinh.Year.ToString());
                    document.ReplaceText("{NgayRaQD}", NgayThangNam(mauInObject.NgayRaQuyetDinh));
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{SoThuLy}", mauInObject.ThuLyAD);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayNopDonTaiToaAn.Year.ToString());
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{NgayNopDon}", NgayThangNam(mauInObject.NgayNopDonTaiToaAn));
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{BienPhapXuLyHanhChinh}", mauInObject.BienPhapXuLyHanhChinh);
                    if (String.IsNullOrEmpty(mauInObject.LyDo))
                        document.ReplaceText("{LyDo}", "Không");
                    else
                    {
                        var lydo = StripHtmlTags(mauInObject.LyDo);
                        Xceed.Words.NET.Paragraph LyDo = GetParagraph(document, "{LyDo}");
                        foreach (var item in lydo)
                        {
                            if (lydo.Count == 1 || item == lydo.FirstOrDefault())
                                LyDo.ReplaceText("{LyDo}", item);
                            else
                            {
                                LyDo.InsertParagraphAfterSelf(item, false, format).SpacingBefore(0).SpacingAfter(0).IndentationFirstLine = 1.0f;
                            }
                        }
                    }
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhan);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInADQuyetDinhDinhChiDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public string[] MauInADBiaHoSoDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath)
        {

            try
            {
                // Create a new document.
                using (DocX document = DocX.Create(filePath))
                {
                    // Apply a template to the document based on a path.
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };

                    Xceed.Words.NET.Paragraph NguoiBiDeNghi = GetParagraph(document, "{NguoiBiDeNghi}");
                    Xceed.Words.NET.Paragraph NguoiDaiDien = GetParagraph(document, "{NguoiDaiDien}");
                    Xceed.Words.NET.Paragraph NguoiDaiDien2 = GetParagraph(document, "{NguoiDaiDien2}");
                    int i = 1;
                    foreach (var item in mauInObject.DanhSachDuongSuAD)
                    {
                        if (item.NgayThangNamSinh == null)
                            item.NgayThangNamSinh = "..........";
                        if (mauInObject.DanhSachDuongSuAD.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSuAD.FirstOrDefault()))
                        {
                            NguoiBiDeNghi.ReplaceText("{NguoiBiDeNghi}", "Họ tên: " + item.HoTenDuongSu + ".");
                            NguoiBiDeNghi.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                            NguoiBiDeNghi.AppendLine("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);                                                          
                        }
                        else
                        {
                            NguoiBiDeNghi.AppendLine("Họ tên: " + item.HoTenDuongSu + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6);
                            NguoiBiDeNghi.AppendLine((item.NgayThangNamSinh.Count() > 4 ? "Ngày sinh: " : "   Năm sinh: ") + item.NgayThangNamSinh + ".").Font("Times New Roman").FontSize(14);
                            NguoiBiDeNghi.AppendLine("Địa chỉ: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);    
                        }
                    }
                    var nguoidd = new List<DuongSuModel>();
                    foreach(var item in mauInObject.DanhSachDuongSuAD)
                    {
                        if(item.HoTenNguoiGiamHo!=null)
                        {
                            var a = new DuongSuModel();
                            a.HoTenDuongSu = item.HoTenNguoiGiamHo;
                            a.NoiDKHKTT = item.DiaChiNguoiGiamHo;
                        }                                                
                    }
                    if (nguoidd.Count < 1)
                        NguoiDaiDien.ReplaceText("{NguoiDaiDien}", "");
                    else
                    {
                        foreach(var item in nguoidd)
                        {
                            if (mauInObject.DanhSachNguyenDon.Count() == 1 || item.Equals(mauInObject.DanhSachNguyenDon.FirstOrDefault()))
                            {
                                NguoiDaiDien.ReplaceText("{NguoiDaiDien}", "Họ tên: " + item.HoTenDuongSu + ".");
                                NguoiDaiDien.AppendLine("Địa chỉ: " +  item.NoiDKHKTT + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                            }
                            else
                            {
                                if(i==2)
                                {
                                    NguoiDaiDien2.ReplaceText("{NguoiDaiDien2}", "Họ tên: " + item.HoTenDuongSu + ".");
                                    NguoiDaiDien2.AppendLine("Địa chỉ: " + item.NoiDKHKTT + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                                }
                                else if(i%2==0)
                                {
                                    NguoiDaiDien2.AppendLine( "Họ tên: " + item.HoTenDuongSu + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6); ;
                                    NguoiDaiDien2.AppendLine("Địa chỉ: " + item.NoiDKHKTT + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                                }
                                else
                                {
                                    NguoiDaiDien.AppendLine("Họ tên: " + item.HoTenDuongSu + ".").Font("Times New Roman").FontSize(14).SpacingBefore(6); ;
                                    NguoiDaiDien.AppendLine("Địa chỉ: " + item.NoiDKHKTT + ".").Font("Times New Roman").FontSize(14).SpacingAfter(6);
                                }
                            }
                            i++;
                        }
                    }
                    if (nguoidd.Count<2)
                        NguoiDaiDien2.ReplaceText("{NguoiDaiDien2}", "");

                    //replacement
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());
                    if (mauInObject.TenToaAn.IndexOf("huyện") != -1 || mauInObject.TenToaAn.IndexOf("thành phố") != -1)
                    {
                        document.ReplaceText("{ToaAnTinh}", "TỈNH CÀ MAU");
                    }
                    else if (mauInObject.GiaiDoan == 1)
                    {
                        var tempToaAnTinhParagraph = GetParagraph(document, "{ToaAnTinh}");
                        tempToaAnTinhParagraph.Remove(false);
                    }
                    document.ReplaceText("{SoThuLyAD}", mauInObject.ThuLyAD);                    
                    document.ReplaceText("{NamNhanDon}", mauInObject.NgayNopDonTaiToaAn.Year.ToString());
                    document.ReplaceText("{BienPhapXuLyHanhChinh}", mauInObject.BienPhapXuLyHanhChinh);                    
                    document.ReplaceText("{NgayNhanDon}", NgayThangNam(mauInObject.NgayNopDonTaiToaAn));
                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{TenCoQuanDeNghi}", mauInObject.TenCoQuanDeNghi);
                    document.ReplaceText("{SoQD}", mauInObject.SoQuyetDinh);
                    document.ReplaceText("{NamRaQD}", mauInObject.NgayRaQuyetDinh.Year.ToString());
                    document.ReplaceText("{NgayRaQD}", NgayThangNam(mauInObject.NgayRaQuyetDinh));
                   
                    if (mauInObject.GiaiDoan == 2)
                    {
                        document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                        document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                        document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                        document.ReplaceText("{SoQDPT}",  mauInObject.SoQuyetDinhPT);
                        document.ReplaceText("{NamRaQDPT}", mauInObject.NgayRaQuyetDinhPT.Year.ToString());
                        document.ReplaceText("{NgayRaQDPT}", NgayThangNam(mauInObject.NgayRaQuyetDinhPT));
                    }
                    
                    // Save this document to disk.
                    document.SaveAs(filePath);
                    //}
                }

                // Open in word:
                //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauIn0716BiaHoSoDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }

        }

        public string[] MauInADQuyetDinhMoPhienHopPhucThamDoc(MauInSo47PTViewModel mauInObject, string filePath, string templatePath)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.ApplyTemplate(templatePath);

                    Formatting format = new Formatting
                    {
                        FontFamily = new Xceed.Words.NET.Font("Times New Roman"),
                        Size = 14
                    };
                    Xceed.Words.NET.Paragraph NguoiBiDeNghi = GetParagraph(document, "{NguoiBiDeNghi}");
                    if (mauInObject.DanhSachDuongSu != null || mauInObject.DanhSachDuongSu.Any())
                    {
                        foreach (var item in mauInObject.DanhSachDuongSu)
                        {
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.FirstOrDefault()))
                            {
                                NguoiBiDeNghi.ReplaceText("{NguoiBiDeNghi}", item.HoTenDuongSu + ".");
                            }
                            else
                            {
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Con ông: " + (item.HoTenCha ?? "..........") + " và bà: " + (item.HoTenMe ?? "..........") + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nơi cư trú: " + (item.NoiTamTru ?? item.NoiDKHKTT) + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                                var p3 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nghề nghiệp: " + (item.NgheNghiep ?? "..........") + ".", false, format);
                                p3.IndentationFirstLine = 1.0f;
                                p3.SpacingBefore(6);
                                p3.SpacingAfter(6);
                                var p4 = NguoiBiDeNghi.InsertParagraphAfterSelf("CMND số: " + (item.SoCMND ?? "..........") + "   Ngày cấp: " + (item.NgayCapCMND.ToString("dd/MM/yyyy") ?? "..........") + "   Nơi cấp: " + (item.NoiCapCMND ?? "..........") + ".", false, format);
                                p4.IndentationFirstLine = 1.0f;
                                p4.SpacingBefore(6);
                                p4.SpacingAfter(6);
                                var p5 = NguoiBiDeNghi.InsertParagraphAfterSelf("Sinh ngày: " + (item.NgayThangNamSinh ?? "..........") + ".", false, format);
                                p5.IndentationFirstLine = 1.0f;
                                p5.SpacingBefore(6);
                                p5.SpacingAfter(6);
                                var p6 = NguoiBiDeNghi.InsertParagraphAfterSelf(item.HoTenDuongSu + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                            }
                            if (mauInObject.DanhSachDuongSu.Count() == 1 || item.Equals(mauInObject.DanhSachDuongSu.LastOrDefault()))
                            {
                                var f = mauInObject.DanhSachDuongSu.FirstOrDefault();
                                var p1 = NguoiBiDeNghi.InsertParagraphAfterSelf("Con ông: " + (f.HoTenCha ?? "..........") + " và bà: " + (f.HoTenMe ?? "..........") + ".", false, format);
                                p1.IndentationFirstLine = 1.0f;
                                //p1.SpacingBefore(6);
                                p1.SpacingAfter(6);
                                var p2 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nơi cư trú: " + (f.NoiTamTru ?? f.NoiDKHKTT) + ".", false, format);
                                p2.IndentationFirstLine = 1.0f;
                                //p2.SpacingBefore(6);
                                p2.SpacingAfter(6);
                                var p3 = NguoiBiDeNghi.InsertParagraphAfterSelf("Nghề nghiệp: " + (f.NgheNghiep ?? "..........") + ".", false, format);
                                p3.IndentationFirstLine = 1.0f;
                                p3.SpacingBefore(6);
                                p3.SpacingAfter(6);
                                var p4 = NguoiBiDeNghi.InsertParagraphAfterSelf("CMND số: " + (f.SoCMND ?? "..........") + "   Ngày cấp: " + (f.NgayCapCMND.ToString("dd/MM/yyyy") ?? "..........") + "   Nơi cấp: " + (f.NoiCapCMND ?? "..........") + ".", false, format);
                                p4.IndentationFirstLine = 1.0f;
                                p4.SpacingBefore(6);
                                p4.SpacingAfter(6);
                                var p5 = NguoiBiDeNghi.InsertParagraphAfterSelf("Sinh ngày: " + (f.NgayThangNamSinh ?? "..........") + ".", false, format);
                                p5.IndentationFirstLine = 1.0f;
                                p5.SpacingBefore(6);
                                p5.SpacingAfter(6);
                            }
                        }
                    }

                    //replace               

                    document.ReplaceText("{ThamPhan}", ThemOngBa(mauInObject.GioiTinhThamPhanChuToa, mauInObject.ThamPhanChuToa));
                    document.ReplaceText("{ThuKy}", ThemOngBa(mauInObject.GioiTinhThuKy, mauInObject.ThuKy));
                    document.ReplaceText("{MaHoSo}", mauInObject.MaHoSo);
                    document.ReplaceText("{TenToaAnInHoa}", mauInObject.TenToaAn.ToUpper());                    
                    document.ReplaceText("{TinhThanh}", mauInObject.TenToaAn.Replace("huyện", "").Replace("thành phố", "TP.").Replace("tỉnh", ""));
                    document.ReplaceText("{SoHoSo}", mauInObject.SoHoSo);
                    document.ReplaceText("{NamXetXu}", mauInObject.NgayRaQuyetDinhXetXu.Year.ToString());
                    document.ReplaceText("{NgayXetXu}", NgayThangNam(mauInObject.NgayRaQuyetDinhXetXu));
                    document.ReplaceText("{BienPhapXuLyHanhChinh}", mauInObject.BienPhapXuLyHanhChinh);
                    document.ReplaceText("{TenCoQuanDeNghi}", mauInObject.TenCoQuanDeNghi);
                    document.ReplaceText("{TenToaAn}", mauInObject.TenToaAn);
                    document.ReplaceText("{SoThuLy}", mauInObject.SoThuLy);
                    document.ReplaceText("{NamThuLy}", mauInObject.NgayThuLy.Year.ToString());
                    document.ReplaceText("{NgayThuLy}", NgayThangNam(mauInObject.NgayThuLy));
                    document.ReplaceText("{ToaAnST}", mauInObject.ToaAnSoTham);
                    document.ReplaceText("{GioMoPhienToa}", GioPhut(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{ThoiGianMoPhienToa}", NgayThangNam(mauInObject.ThoiGianMoPhienToa));
                    document.ReplaceText("{DiaDiemMoPhienToa}", mauInObject.DiaDiemMoPhienToa);
                    document.ReplaceText("{DiaChiToaAn}", mauInObject.DiaChiToaAn);
                    document.ReplaceText("{HoTenNguoiKy}", mauInObject.ThamPhanChuToa);

                    document.SaveAs(filePath);
                }

                return new string[] { converter(filePath, ".docx", ".doc") };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("MauInADQuyetDinhMoPhienHopPhucThamDoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        #endregion

        public string converter(string filePath, string fromExtension, string toExtension)
        {
            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile(filePath);
            string newfilePath = filePath.Replace(fromExtension, toExtension);
            switch (toExtension)
            {
                case ".doc":
                    document.SaveToFile(newfilePath, FileFormat.Doc); break;
                case ".docx":
                    document.SaveToFile(newfilePath, FileFormat.Docx2010); break;
                case ".pdf":
                    document.SaveToFile(newfilePath, FileFormat.PDF); break;
            }
            File.Delete(filePath);
            return newfilePath;
        }

        private static void InsertImageSpire(string filePath, string imagePath)
        {
            using (Spire.Doc.Document document=new Spire.Doc.Document())
            {
                document.LoadFromFile(filePath);
                Spire.Doc.Table table1 = (Spire.Doc.Table)document.Sections[0].Tables[1];
                DocPicture picture = table1.Rows[0].Cells[1].Paragraphs[2].AppendPicture(System.Drawing.Image.FromFile(imagePath));
                picture.TextWrappingStyle = Spire.Doc.Documents.TextWrappingStyle.Behind;
                picture.HorizontalAlignment = Spire.Doc.ShapeHorizontalAlignment.Center;
                picture.VerticalAlignment = Spire.Doc.ShapeVerticalAlignment.Center;
                document.SaveToFile(filePath, FileFormat.Docx);
            }
        }

        #region Converting numbers of VND to words đồng
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }
            return Ktach;
        }
        public static string So_chu(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng";

            return lso_chu.ToString().Trim();
        }
        #endregion

        #region Private Methods
        private string replacetag(string s)
        {
            if (s != null && s != "")
                s = s.Replace("<p>", "<li>").Replace("</p>", "</li>");
            return s;
        }

        private string AddZero(string s)
        {
            int i = 0;
            if (s != null && s != "" && int.TryParse(s,out i) && int.Parse(s) < 10 && s.Count() == 1 )
                return "0" + s;
            return s;
        }

        private static List<string> StripHtmlTags(string html)
        {
            if (String.IsNullOrEmpty(html)) return null;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            string str = HttpUtility.HtmlDecode(doc.DocumentNode.InnerText.Replace("\r\n\r\n", "|"));
            if (str.LastIndexOf("\r\n") != -1)
                str = str.Remove(str.LastIndexOf("\r\n"), 2);
            return str.Split('|').ToList();
        }
             
        private string ThemOngBa(string gioitinh, string hoten)
        {
            return (gioitinh == "Nam" ? "Ông " : "Bà ") + hoten;
        }

        private string ThemOngBa_ChuThuong(string gioitinh, string hoten)
        {
            return (gioitinh == "Nam" ? "ông " : "bà ") + hoten;
        }

        private string NgayThangNam(DateTime date)
        {
            return "ngày "+(date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString()) + " tháng " + (date.Month < 3 ? "0" + date.Month.ToString() : date.Month.ToString()) + " năm " + date.Year.ToString();
        }

        private string GioPhut(DateTime date)
        {
            return (date.Hour < 10 ? "0" + date.Hour.ToString() : date.Hour.ToString()) + " giờ " + (date.Minute < 10 ? "0" + date.Minute.ToString() : date.Minute.ToString()) + " phút";
        }
        
        private Xceed.Words.NET.Paragraph GetParagraph(DocX doc, string pattern)
        {
            foreach (var p in doc.Paragraphs)
            {
                var pTemp = p.Text.Contains(pattern);
                if (pTemp)
                {
                    return p;
                }
            }

            return null;
        }
        #endregion
    }
}
