using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.NhanDon.DataAccess;
using Biz.Lib.TACM.NhanDon.IDataAccess;
using Biz.Lib.TACM.NhanDon.Model;
using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.NhanDon;
using System.Web.Mvc;
using Biz.Lib.SettingData.Model;
using Biz.TACM.Enums;
using Biz.Lib.TACM.Resources.Resources;
using Microsoft.Ajax.Utilities;
using CongDoanHoSoModel = Biz.Lib.TACM.NhanDon.Model.CongDoanHoSoModel;
using HoSoVuAnModel = Biz.Lib.TACM.NhanDon.Model.HoSoVuAnModel;

namespace Biz.TACM.Services
{
    public class NhanDonService : INhanDonService
    {
        private INhanDonDataAccess _nhandonDA;

        private INhanDonDataAccess NhanDonDA
        {
            get { return _nhandonDA ?? (_nhandonDA = new NhanDonDataAccess()); }
        }

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        #region NhanDon

        public SelectList SelectListCongDoanHoSo(int selectedValue, int giaiDoan)
        {
            IEnumerable<DataItemModel> list;
            if (selectedValue == CongDoan.LuuKho.GetHashCode())
            {
                list = XMLUtils.BindData(giaiDoan == GiaiDoan.SoTham.GetHashCode() ? "CongDoanHoSo" : "CongDoanHoSoPhucTham");
            }
            else
            {
                list = giaiDoan == GiaiDoan.SoTham.GetHashCode() ? XMLUtils.BindData("CongDoanHoSo").Where(m => Int32.Parse(m.value) <= selectedValue + 1) : XMLUtils.BindData("CongDoanHoSoPhucTham").Where(m => Int32.Parse(m.value) <= selectedValue + 1);
            }
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public SelectList DanhSachNgayHoSoVuAn(int hoSoVuAnID, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.DanhSachNgayHoSoVuAn(hoSoVuAnID, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.HoSoVuAnID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgayHoSoVuAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SelectListNhanVien(string nhomChucNang, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.DanhSachNhanVien(nhomChucNang);

                var danhSachNhanVien = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoTenVaMaNV,
                        Value = x.MaNV.ToString()
                    }
                );

                return new SelectList(danhSachNhanVien, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SelectListNhanVien with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public IEnumerable<DuongSuModel> NguyenDonVaBiDon(int hoSoVuAnID)
        {
            try
            {
                var model = NhanDonDA.NguyenDonVaBiDon(hoSoVuAnID);
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("NguyenDonVaBiDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SelectListNguoiKhieuNai(int hoSoVuAnId, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.NguyenDonVaBiDon(hoSoVuAnId);
                if (dbModel == null)
                {
                    return new SelectList(Enumerable.Empty<SelectListItem>());
                }
                // var listDuongSu = dbModel.ToList().Where(x => x.TuCachThamGiaToTung == "Nguyên đơn");
                var danhSachDuongSu = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoVaTen,
                        Value = x.HoVaTen
                    }
                );

                return new SelectList(danhSachDuongSu, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SelectListNguoiKhieuNai with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SelectListDuongSu(int ToaAnID, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.DanhSachDuongSu(ToaAnID);

                var danhSachDuongSu = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoVaTen,
                        Value = x.DuongSuID.ToString()
                    }
                );

                return new SelectList(danhSachDuongSu, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public HoSoVuAnModel ChiTietHoSoVuAn(int hoSoVuAnID)
        {
            var model = NhanDonDA.ChiTietHoSoVuAn(hoSoVuAnID);

            if (model != null)
            {
                model.NhanVienThamPhan = NhanDonDA.ThongTinNhanVien(model.ThamPhan);
                model.NhanVienCanBoNhanDon = NhanDonDA.ThongTinNhanVien(model.CanBoNhanDon);
                model.NhanVienNguoiKyXacNhanDaNhanDon = NhanDonDA.ThongTinNhanVien(model.NguoiKyXacNhanDaNhanDon);
                model.NhanVienNguoiLapDon = NhanDonDA.ThongTinNhanVien(model.NguoiTao);

                ChuyenDonModel modelChuyenDon = ChiTietChuyenDonTheoHoSoVuAnID(model.HoSoVuAnID, GiaiDoan.SoTham.GetHashCode(), CongDoan.NhanDon.GetHashCode());
                if (modelChuyenDon != null)
                    model.NgayChuyenDon = modelChuyenDon.NgayChuyenDon;

                TraLaiDonModel modelTraLaiDon = ChiTietTraLaiDonTheoHoSoVuAnID(model.HoSoVuAnID, Enums.CongDoan.NhanDon.GetHashCode());
                if (modelTraLaiDon != null)
                    model.NgayTraDon = modelTraLaiDon.NgayTraDon;

                KhieuNaiTraDonModel modelKhieuNaiTraDon = ChiTietKhieuNaiTraDonTheoHoSoVuAnID(model.HoSoVuAnID);
                if (modelKhieuNaiTraDon != null)
                    model.NgayKhieuNai = modelKhieuNaiTraDon.NgayKhieuNai;

                model.DuongSu = NguyenDonVaBiDon(model.HoSoVuAnID);
            }

            return model;
        }

        public HoSoVuAnModel ChiTietHoSoVuAnTheoGiaiDoan(int hoSoVuAnID, int giaiDoan)
        {
            var model = NhanDonDA.ChiTietHoSoVuAnTheoGiaiDoan(hoSoVuAnID, giaiDoan);

            if (model != null)
            {
                model.NhanVienThamPhan = NhanDonDA.ThongTinNhanVien(model.ThamPhan);
                model.NhanVienCanBoNhanDon = NhanDonDA.ThongTinNhanVien(model.CanBoNhanDon);
                model.NhanVienNguoiKyXacNhanDaNhanDon = NhanDonDA.ThongTinNhanVien(model.NguoiKyXacNhanDaNhanDon);
                model.NhanVienNguoiLapDon = NhanDonDA.ThongTinNhanVien(model.NguoiTao);

                ChuyenDonModel modelChuyenDon = ChiTietChuyenDonTheoHoSoVuAnID(model.HoSoVuAnID, GiaiDoan.SoTham.GetHashCode(), CongDoan.NhanDon.GetHashCode());
                if (modelChuyenDon != null)
                    model.NgayChuyenDon = modelChuyenDon.NgayChuyenDon;

                TraLaiDonModel modelTraLaiDon = ChiTietTraLaiDonTheoHoSoVuAnID(model.HoSoVuAnID, Enums.CongDoan.NhanDon.GetHashCode());
                if (modelTraLaiDon != null)
                    model.NgayTraDon = modelTraLaiDon.NgayTraDon;

                KhieuNaiTraDonModel modelKhieuNaiTraDon = ChiTietKhieuNaiTraDonTheoHoSoVuAnID(model.HoSoVuAnID);
                if (modelKhieuNaiTraDon != null)
                    model.NgayKhieuNai = modelKhieuNaiTraDon.NgayKhieuNai;

                model.DuongSu = NguyenDonVaBiDon(model.HoSoVuAnID);
            }

            return model;
        }

        public HoSoVuAnModel ChiTietHoSoVuAnTheoLog(int id)
        {
            var model = NhanDonDA.ChiTietHoSoVuAnTheoLog(id);

            if (model != null)
            {
                model.NhanVienThamPhan = NhanDonDA.ThongTinNhanVien(model.ThamPhan);
                model.NhanVienCanBoNhanDon = NhanDonDA.ThongTinNhanVien(model.CanBoNhanDon);
                model.NhanVienNguoiKyXacNhanDaNhanDon = NhanDonDA.ThongTinNhanVien(model.NguoiKyXacNhanDaNhanDon);
                model.NhanVienNguoiLapDon = NhanDonDA.ThongTinNhanVien(model.NguoiTao);

                ChuyenDonModel modelChuyenDon = ChiTietChuyenDonTheoHoSoVuAnID(model.HoSoVuAnID, GiaiDoan.SoTham.GetHashCode(), CongDoan.NhanDon.GetHashCode());
                if (modelChuyenDon != null)
                    model.NgayChuyenDon = modelChuyenDon.NgayChuyenDon;

                TraLaiDonModel modelTraLaiDon = ChiTietTraLaiDonTheoHoSoVuAnID(model.HoSoVuAnID, Enums.CongDoan.NhanDon.GetHashCode());
                if (modelTraLaiDon != null)
                    model.NgayTraDon = modelTraLaiDon.NgayTraDon;

                KhieuNaiTraDonModel modelKhieuNaiTraDon = ChiTietKhieuNaiTraDonTheoHoSoVuAnID(model.HoSoVuAnID);
                if (modelKhieuNaiTraDon != null)
                    model.NgayKhieuNai = modelKhieuNaiTraDon.NgayKhieuNai;

                model.DuongSu = NguyenDonVaBiDon(model.HoSoVuAnID);
            }

            return model;
        }

        public HoSoVuAnModel ChiTietHoSoVuAnTheoId(int hoSoVuAnID)
        {
            try
            {
                return NhanDonDA.ChiTietHoSoVuAn(hoSoVuAnID);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietHoSoVuAnTheoID with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public IEnumerable<HoSoVuAnModel> DanhSachHoSoVuAn(HoSoVuAnModel model, string maNV)
        {
            model.NgayTao = model.NgayTao.HasValue ? Convert.ToDateTime(Convert.ToDateTime(model.NgayTao).ToString("MM/dd/yyyy")) : DateTime.Parse("1/1/1753");
            model.NgayCapNhat = model.NgayCapNhat.HasValue ? Convert.ToDateTime(Convert.ToDateTime(model.NgayCapNhat).ToString("MM/dd/yyyy")) : DateTime.MaxValue;

            var danhSach = NhanDonDA.DanhSachHoSoVuAn(model, maNV);
            var results = new List<HoSoVuAnModel>();
            foreach (var entry in danhSach)
            {
                entry.NhanVienThamPhan = NhanDonDA.ThongTinNhanVien(entry.ThamPhan);

                entry.DuongSu = NguyenDonVaBiDon(entry.HoSoVuAnID);

                results.Add(entry);
            }

            return results;
        }

        public ResponseResult ThemHoSoVuAn(HoSoVuAnModel model)
        {
            ResponseResult result = null;

            //model.NhomAn = "DS";                //Mã Nhóm Án (DS, KT, HS...)
            var manv = SettingDataService.GetNhanVienSessionInfo().MaNV;
            model.TrangThaiCongDoan = "";
            model.NguoiTao = manv;
            model.NguoiCapNhat = manv;
            model.NgayLamDon = Convert.ToDateTime(model.NgayLamDon);
            model.NgayNopDonTaiToaAn = Convert.ToDateTime(model.NgayNopDonTaiToaAn);

            result = NhanDonDA.ThemHoSoVuAn(model);

            return result;
        }

        public ResponseResult SuaHoSoVuAn(HoSoVuAnModel model)
        {
            ResponseResult result = null;

            model.NguoiCapNhat = SettingDataService.GetNhanVienSessionInfo().MaNV;

            result = NhanDonDA.SuaHoSoVuAn(model);

            return result;
        }



        public ResponseResult ChuyenTrangThai(HoSoVuAnModel model)
        {
            ResponseResult result = null;

            result = NhanDonDA.ChuyenTrangThai(model);

            return result;
        }

        public ResponseResult ChuyenCongDoanHoSo(HoSoVuAnModel model)
        {
            ResponseResult result = null;

            model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;

            result = NhanDonDA.ChuyenCongDoanHoSo(model);

            return result;
        }

        public Dictionary<int, int> CongDoanHoSo(HoSoVuAnModel model, string maNV)
        {
            try
            {
                var results = new Dictionary<int, int>();

                foreach (CongDoanHoSoModel congDoan in NhanDonDA.CongDoanHoSo(model, maNV))
                {
                    results.Add(congDoan.CongDoanHoSo, congDoan.Tong);
                }

                return results;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"CongDoanHoSo with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }
        #endregion

        #region DuongSu
        public IEnumerable<DuongSuViewModel> GetDanhSachDuongSuCaNhan(int hoSoVuAnId)
        {
            try
            {
                var dbModel = NhanDonDA.GetDanhSachDuongSu(hoSoVuAnId, Setting.DUONGSULA_CANHAN);
                return dbModel.Select(s => new DuongSuViewModel
                {
                    STT = s.STT,
                    DuongSuID = s.DuongSuID,
                    HoSoVuAnID = s.HoSoVuAnID,
                    HoVaTen = s.HoVaTen,
                    SoCMND = s.SoCMND,
                    NgayThangNamSinh = String.Format("{0:dd/MM/yyyy}", s.NgayThangNamSinh),
                    QuocTich = s.QuocTich,
                    TuCachThamGiaToTung = s.TuCachThamGiaToTung,
                    DuongSuLa = s.DuongSuLa,
                    SoDienThoai = s.SoDienThoai,
                    NoiCuTru = s.NoiTamTru ?? s.NoiDKHKTT
                }).ToList();
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public IEnumerable<DuongSuViewModel> GetDanhSachDuongSuToChuc(int hoSoVuAnId)
        {
            try
            {
                var dbModel = NhanDonDA.GetDanhSachDuongSu(hoSoVuAnId, Setting.DUONGSULA_TOCHUC);
                return dbModel.Select(s => new DuongSuViewModel
                {
                    STT = s.STT,
                    DuongSuID = s.DuongSuID,
                    HoSoVuAnID = s.HoSoVuAnID,
                    TenCoQuanToChuc = s.TenCoQuanToChuc,
                    HoVaTen = s.HoVaTen,
                    SoCMND = s.SoCMND,
                    NgayThangNamSinh = String.Format("{0:dd/MM/yyyy}", s.NgayThangNamSinh),
                    SoDienThoai = s.SoDienThoai,
                    NoiDKHKTT = s.NoiDKHKTT,
                    QuocTich = s.QuocTich,
                    TuCachThamGiaToTung = s.TuCachThamGiaToTung,
                    DuongSuLa = s.DuongSuLa
                }).ToList();
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public IEnumerable<DuongSuViewModel> GetDanhSachDuongSu(int hoSoVuAnId, string duongSuLa = null, string tuCachThamGiaToTung = null)
        {
            try
            {
                var dbModel = NhanDonDA.GetDanhSachDuongSu(hoSoVuAnId, duongSuLa, tuCachThamGiaToTung);
                return dbModel.Select(s => new DuongSuViewModel
                {
                    STT = s.STT,
                    DuongSuID = s.DuongSuID,
                    HoSoVuAnID = s.HoSoVuAnID,
                    HoVaTen = s.HoVaTen,
                    TenCoQuanToChuc = s.TenCoQuanToChuc,
                    SoCMND = s.SoCMND,
                    NgayThangNamSinh = String.Format("{0:dd/MM/yyyy}", s.NgayThangNamSinh),
                    SoDienThoai = s.SoDienThoai,
                    NoiDKHKTT = s.NoiDKHKTT,
                    QuocTich = s.QuocTich,
                    TuCachThamGiaToTung = s.TuCachThamGiaToTung,
                    DuongSuLa = s.DuongSuLa
                }).ToList();
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public DuongSuViewModel ChiTietDuongSuTheoId(int duongSuId)
        {
            try
            {
                var dbModel = NhanDonDA.ChiTietDuongSuTheoId(duongSuId);
                List<string> listDacDiemNhanThanBiCao = new List<string>();
                //List<string> listDacDiemNhanThanBiHai = new List<string>();
                //if (dbModel.DacDiemNhanThanBiHai != null)
                //    listDacDiemNhanThanBiHai = dbModel.DacDiemNhanThanBiHai.Split('#').ToList();
                if (dbModel.DacDiemNhanThanBiCao != null)
                    listDacDiemNhanThanBiCao = dbModel.DacDiemNhanThanBiCao.Split('#').ToList();
                string s = "";
                if (dbModel.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_NGUOIDAIDIEN || dbModel.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_NGUOIBAOVEQUYENLOIICH)
                    s = NhanDonDA.ChiTietDuongSuTheoId(dbModel.NguoiDaiDienCua).HoVaTen;

                return new DuongSuViewModel
                {
                    DuongSuID = dbModel.DuongSuID,
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    TenCoQuanToChuc = dbModel.TenCoQuanToChuc,
                    HoVaTen = dbModel.HoVaTen,
                    SoCMND = dbModel.SoCMND,
                    DanToc = dbModel.DanToc,
                    NgayThangNamSinh = String.Format("{0:dd/MM/yyyy}", dbModel.NgayThangNamSinh),
                    GioiTinh = dbModel.GioiTinh,
                    SoDienThoai = dbModel.SoDienThoai,
                    QuocTich = dbModel.QuocTich,
                    NoiDKHKTT = dbModel.NoiDKHKTT,
                    NoiTamTru = dbModel.NoiTamTru,
                    TuCachThamGiaToTung = dbModel.TuCachThamGiaToTung,
                    DuongSuLa = dbModel.DuongSuLa,
                    NhomNghiepVu = dbModel.NhomNghiepVu,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    NguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    TrangThai = dbModel.TrangThai,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu,
                    TonGiao = dbModel.TonGiao,
                    NgheNghiep = dbModel.NgheNghiep,
                    TrinhDoVanHoa = dbModel.TrinhDoVanHoa,
                    NgayCapCMND = dbModel.NgayCapCMND == null ? null : String.Format("{0:dd/MM/yyyy}", dbModel.NgayCapCMND),
                    NoiCapCMND = dbModel.NoiCapCMND,
                    HoTenNguoiGiamHo = dbModel.HoTenNguoiGiamHo,
                    DiaChiNguoiGiamHo = dbModel.DiaChiNguoiGiamHo,
                    HoTenCha = dbModel.HoTenCha,
                    DiaChiCha = dbModel.DiaChiCha,
                    NamSinhCha = dbModel.NamSinhCha,
                    HoTenMe = dbModel.HoTenMe,
                    DiaChiMe = dbModel.DiaChiMe,
                    NamSinhMe = dbModel.NamSinhMe,
                    TienAn = dbModel.TienAn,
                    TienSu = dbModel.TienSu,
                    ChaDaChetHayChua = dbModel.ChaDaChetHayChua,
                    MeDaChetHayChua = dbModel.MeDaChetHayChua,
                    TenGoiKhac = dbModel.TenGoiKhac,
                    ListDacDiemNhanThanBiCao = listDacDiemNhanThanBiCao,
                    LuuYVeChucVu = dbModel.LuuYVeChucVu,
                    TinhTrangGiamGiu = dbModel.TinhTrangGiamGiu,
                    NgayBatTamGiam = dbModel.NgayBatTamGiam == null ? null : String.Format("{0:dd/MM/yyyy}", dbModel.NgayBatTamGiam),
                    ToiDanhTruyTo = dbModel.ToiDanhTruyTo,
                    DieuLuatTruyTo = dbModel.DieuLuatTruyTo,
                    VanPhongLuatSu = dbModel.VanPhongLuatSu,
                    DoanLuatSu = dbModel.DoanLuatSu,
                    NoiCongTac = dbModel.NoiCongTac,
                    NguoiDaiDienCua = dbModel.NguoiDaiDienCua,
                    QuanHeVoiNguoiThamGiaToTung = dbModel.QuanHeVoiNguoiThamGiaToTung,
                    ChucVu = dbModel.ChucVu,
                    DacDiemNhanThanBiHai = dbModel.DacDiemNhanThanBiHai,
                    NguoiBaoChuaLa = dbModel.NguoiBaoChuaLa,
                    NoiSinh = dbModel.NoiSinh,
                    NoiCuTru = dbModel.NoiDKHKTT,
                    DiaChiTruSo = dbModel.NoiDKHKTT,
                    HoTenNguoiDuocDaiDien = s
            };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietDuongSuTheoId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new DuongSuViewModel();
            }
        }

        public ResponseResult TaoDuongSu(DuongSuViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                var hoSoVuAnModel = ChiTietHoSoVuAnTheoId(viewModel.HoSoVuAnID);
                var maNhomAn = hoSoVuAnModel.NhomAn;
                if (maNhomAn.Equals(Setting.MANHOMAN_APDUNG_BPXLHC))
                {
                    viewModel.DuongSuLa = Setting.DUONGSULA_CANHAN;
                    viewModel.TuCachThamGiaToTung = SettingDataService.SettingDataItem("AD_TUCACHTHAMGIATOTUNG", "").FirstOrDefault().Text;
                }
                else if (maNhomAn.Equals(Setting.MANHOMAN_HINHSU))
                {   
                    if (viewModel.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_NGUOIBAOVEQUYENLOIICH))
                        viewModel.NguoiDaiDienCua = viewModel.NguoiBaoVeCua;

                    if (viewModel.DuongSuLa.Equals(Setting.DUONGSULA_TOCHUC))
                        viewModel.NoiDKHKTT = viewModel.DiaChiTruSo;
                    else
                        viewModel.NoiDKHKTT = viewModel.NoiCuTru;
                }

                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;

                var dbModel = new DuongSuModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    TenCoQuanToChuc = viewModel.TenCoQuanToChuc,
                    HoVaTen = viewModel.HoVaTen,
                    SoCMND = viewModel.SoCMND,
                    DanToc = viewModel.DanToc,
                    NgayThangNamSinh = viewModel.NgayThangNamSinh,
                    GioiTinh = viewModel.GioiTinh,
                    SoDienThoai = viewModel.SoDienThoai,
                    QuocTich = viewModel.QuocTich,
                    NoiDKHKTT = viewModel.NoiDKHKTT,
                    NoiTamTru = viewModel.NoiTamTru,
                    TuCachThamGiaToTung = viewModel.TuCachThamGiaToTung,
                    DuongSuLa = viewModel.DuongSuLa,
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = viewModel.CongDoanHoSo,
                    NguoiTao = viewModel.NguoiTao,
                    NgayTao = DateTime.Now, //maybe no need to push to db but need to assign
                    GhiChu = viewModel.GhiChu,
                    TonGiao = viewModel.TonGiao,
                    NgheNghiep = viewModel.NgheNghiep,
                    TrinhDoVanHoa = viewModel.TrinhDoVanHoa,
                    NgayCapCMND = viewModel.NgayCapCMND != null ? Convert.ToDateTime(viewModel.NgayCapCMND) : (DateTime?) null,
                    NoiCapCMND = viewModel.NoiCapCMND,
                    HoTenNguoiGiamHo = viewModel.HoTenNguoiGiamHo,
                    DiaChiNguoiGiamHo = viewModel.DiaChiNguoiGiamHo,
                    HoTenCha = viewModel.HoTenCha,
                    DiaChiCha = viewModel.DiaChiCha,
                    NamSinhCha = viewModel.NamSinhCha,
                    HoTenMe = viewModel.HoTenMe,
                    DiaChiMe = viewModel.DiaChiMe,
                    NamSinhMe = viewModel.NamSinhMe,
                    TienAn = viewModel.TienAn,
                    TienSu = viewModel.TienSu,
                    ChaDaChetHayChua = viewModel.ChaDaChetHayChua,
                    MeDaChetHayChua = viewModel.MeDaChetHayChua,
                    TenGoiKhac = viewModel.TenGoiKhac,
                    DacDiemNhanThanBiCao = viewModel.ListDacDiemNhanThanBiCao == null ? "" : String.Join("#", viewModel.ListDacDiemNhanThanBiCao),
                    LuuYVeChucVu = viewModel.LuuYVeChucVu,
                    TinhTrangGiamGiu = viewModel.TinhTrangGiamGiu,
                    NgayBatTamGiam = viewModel.NgayBatTamGiam != null ? Convert.ToDateTime(viewModel.NgayBatTamGiam) : (DateTime?)null,
                    ToiDanhTruyTo = viewModel.ToiDanhTruyTo,
                    DieuLuatTruyTo = viewModel.DieuLuatTruyTo,
                    VanPhongLuatSu = viewModel.VanPhongLuatSu,
                    DoanLuatSu = viewModel.DoanLuatSu,
                    NoiCongTac = viewModel.NoiCongTac,
                    NguoiDaiDienCua = viewModel.NguoiDaiDienCua,
                    QuanHeVoiNguoiThamGiaToTung = viewModel.QuanHeVoiNguoiThamGiaToTung,
                    ChucVu = viewModel.ChucVu,
                    DacDiemNhanThanBiHai = viewModel.DacDiemNhanThanBiHai,
                    NguoiBaoChuaLa = viewModel.NguoiBaoChuaLa,
                    NoiSinh = viewModel.NoiSinh
                };
                result = NhanDonDA.TaoDuongSu(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("TaoDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw ex;
            }
            return result;
        }

        public ResponseResult SuaDuongSu(DuongSuViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                var hoSoVuAnModel = ChiTietHoSoVuAnTheoId(viewModel.HoSoVuAnID);
                var maNhomAn = hoSoVuAnModel.NhomAn;
                if (maNhomAn.Equals(Setting.MANHOMAN_APDUNG_BPXLHC))
                {
                    viewModel.DuongSuLa = Setting.DUONGSULA_CANHAN;
                    viewModel.TuCachThamGiaToTung = SettingDataService.SettingDataItem("AD_TUCACHTHAMGIATOTUNG", "").FirstOrDefault().Text;
                }
                else if (maNhomAn.Equals(Setting.MANHOMAN_HINHSU))
                {
                    if (viewModel.TuCachThamGiaToTung.Equals(Setting.HS_TUCACHTOTUNG_NGUOIBAOVEQUYENLOIICH))
                        viewModel.NguoiDaiDienCua = viewModel.NguoiBaoVeCua;

                    if (viewModel.DuongSuLa.Equals(Setting.DUONGSULA_TOCHUC))
                        viewModel.NoiDKHKTT = viewModel.DiaChiTruSo;
                    else
                        viewModel.NoiDKHKTT = viewModel.NoiCuTru;
                }

                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;

                var dbModel = new DuongSuModel
                {
                    DuongSuID = viewModel.DuongSuID,
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    TenCoQuanToChuc = viewModel.TenCoQuanToChuc,
                    HoVaTen = viewModel.HoVaTen,
                    SoCMND = viewModel.SoCMND,
                    DanToc = viewModel.DanToc,
                    NgayThangNamSinh = viewModel.NgayThangNamSinh,
                    GioiTinh = viewModel.GioiTinh,
                    SoDienThoai = viewModel.SoDienThoai,
                    QuocTich = viewModel.QuocTich,
                    NoiDKHKTT = viewModel.NoiDKHKTT,
                    NoiTamTru = viewModel.NoiTamTru,
                    TuCachThamGiaToTung = viewModel.TuCachThamGiaToTung,
                    DuongSuLa = viewModel.DuongSuLa,
                    NguoiTao = viewModel.NguoiTao,
                    NgayTao = DateTime.Now, //maybe no need to push to db but need to assign
                    GhiChu = viewModel.GhiChu,
                    TonGiao = viewModel.TonGiao,
                    NgheNghiep = viewModel.NgheNghiep,
                    TrinhDoVanHoa = viewModel.TrinhDoVanHoa,
                    NgayCapCMND = viewModel.NgayCapCMND != null ? Convert.ToDateTime(viewModel.NgayCapCMND) : (DateTime?)null,
                    NoiCapCMND = viewModel.NoiCapCMND,
                    HoTenNguoiGiamHo = viewModel.HoTenNguoiGiamHo,
                    DiaChiNguoiGiamHo = viewModel.DiaChiNguoiGiamHo,
                    HoTenCha = viewModel.HoTenCha,
                    DiaChiCha = viewModel.DiaChiCha,
                    NamSinhCha = viewModel.NamSinhCha,
                    HoTenMe = viewModel.HoTenMe,
                    DiaChiMe = viewModel.DiaChiMe,
                    NamSinhMe = viewModel.NamSinhMe,
                    TienAn = viewModel.TienAn,
                    TienSu = viewModel.TienSu,
                    ChaDaChetHayChua = viewModel.ChaDaChetHayChua,
                    MeDaChetHayChua = viewModel.MeDaChetHayChua,
                    TenGoiKhac = viewModel.TenGoiKhac,
                    DacDiemNhanThanBiCao = viewModel.ListDacDiemNhanThanBiCao == null ? "" : String.Join("#", viewModel.ListDacDiemNhanThanBiCao),
                    LuuYVeChucVu = viewModel.LuuYVeChucVu,
                    TinhTrangGiamGiu = viewModel.TinhTrangGiamGiu,
                    NgayBatTamGiam = viewModel.NgayBatTamGiam != null ? Convert.ToDateTime(viewModel.NgayBatTamGiam) : (DateTime?)null,
                    ToiDanhTruyTo = viewModel.ToiDanhTruyTo,
                    DieuLuatTruyTo = viewModel.DieuLuatTruyTo,
                    VanPhongLuatSu = viewModel.VanPhongLuatSu,
                    DoanLuatSu = viewModel.DoanLuatSu,
                    NoiCongTac = viewModel.NoiCongTac,
                    NguoiDaiDienCua = viewModel.NguoiDaiDienCua,
                    QuanHeVoiNguoiThamGiaToTung = viewModel.QuanHeVoiNguoiThamGiaToTung,
                    ChucVu = viewModel.ChucVu,
                    DacDiemNhanThanBiHai = viewModel.DacDiemNhanThanBiHai,
                    NguoiBaoChuaLa = viewModel.NguoiBaoChuaLa,
                    NoiSinh = viewModel.NoiSinh
                };

                result = NhanDonDA.SuaDuongSu(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public ResponseResult XoaDuongSu(int duongSuId)
        {
            ResponseResult result = null;
            try
            {
                result = NhanDonDA.XoaDuongSu(duongSuId);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("XoaDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public SelectList GetDanhSachDuongSuChoNguoiDaiDien(int hoSoVuAnId)
        {
            try
            {
                List<DuongSuViewModel> temp = new List<DuongSuViewModel>();
                var listBiCan = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_BICAN);
                var listBiCao = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_BICAO);
                var listBiHai = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_BIHAI);
                var listNguyenDon = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_NGUYENDON);
                var listBiDon = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_BIDON);
                var listNguoiLienQuan = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_NGUOILIENQUAN);
                var listNguoiLamChung = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_NGUOILAMCHUNG);

                temp.AddRange(listBiCan);
                temp.AddRange(listBiCao);
                temp.AddRange(listBiHai);
                temp.AddRange(listNguyenDon);
                temp.AddRange(listBiDon);
                temp.AddRange(listNguoiLienQuan);
                temp.AddRange(listNguoiLamChung);

                var listItem = temp.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoVaTen,
                        Value = x.DuongSuID.ToString()
                    });
                return new SelectList(listItem, "Value", "Text", null);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachDuongSuChoNguoiDaiDien with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public SelectList GetDanhSachDuongSuChoNguoiBaoVe(int hoSoVuAnId)
        {
            try
            {
                List<DuongSuViewModel> temp = new List<DuongSuViewModel>();
                var listBiHai = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_BIHAI);
                var listNguyenDon = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_NGUYENDON);
                var listBiDon = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_BIDON);
                var listNguoiLienQuan = GetDanhSachDuongSu(hoSoVuAnId, null, Setting.HS_TUCACHTOTUNG_NGUOILIENQUAN);
                
                temp.AddRange(listBiHai);
                temp.AddRange(listNguyenDon);
                temp.AddRange(listBiDon);
                temp.AddRange(listNguoiLienQuan);

                var listItem = temp.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoVaTen,
                        Value = x.DuongSuID.ToString()
                    });
                return new SelectList(listItem, "Value", "Text", null);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachDuongSuChoNguoiBaoVe with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public ResponseResult CapNhatBiCanBiCaoHinhSu(int hoSoVuAnId)
        {
            ResponseResult result = null;
            try
            {
                result = NhanDonDA.CapNhatBiCanBiCaoHinhSu(hoSoVuAnId);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("CapNhatBiCanBiCaoHinhSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public IEnumerable<DuongSuViewModel.DacDiemNhanThan> DanhSachDacDiemSelected(SelectList selectListDacDiem, List<string> selected)
        {
            try
            {
                var listTemp = selectListDacDiem.Select(s => new DuongSuViewModel.DacDiemNhanThan()
                {
                    DacDiem = s.Text
                }).ToList();

                var listChecked = new List<DuongSuViewModel.DacDiemNhanThan>();
                foreach (var item in listTemp)
                {
                    item.IsChecked = selected != null & selected.Contains(item.DacDiem);
                    listChecked.Add(item);
                }

                return listChecked;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachDacDiemSelected with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        #endregion

        #region NoiDungDon
        public SelectList GetDanhSachNgaySuaDoiNoiDungDon(int hoSoVuAnId, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.GetDanhSachNgaySuaDoiNoiDungDon(hoSoVuAnId, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.NoiDungDonID.ToString()
                    }
                );
                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachNgaySuaDoiNoiDungDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public NoiDungDonViewModel ChiTietNoiDungDonTheoId(int noiDungDonId)
        {
            try
            {
                var dbModel = NhanDonDA.ChiTietNoiDungDonTheoId(noiDungDonId);
                return new NoiDungDonViewModel
                {
                    NoiDungDonID = dbModel.NoiDungDonID,
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    NoiDungDon = dbModel.NoiDungDon,
                    NhomNghiepVu = dbModel.NhomNghiepVu,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    TrangThai = dbModel.TrangThai,
                    NguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietNoiDungDonTheoId with error [{0}]", ex.ToString()), AppName.BizSecurity); //AppName.TACMThuLy
                return new NoiDungDonViewModel();
            }
        }

        public ResponseResult SuaDoiNoiDungDon(NoiDungDonViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                var dbModel = new NoiDungDonModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    NoiDungDon = viewModel.NoiDungDon,
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = viewModel.CongDoanHoSo,
                    NguoiTao = viewModel.NguoiTao,
                    GhiChu = viewModel.GhiChu
                };
                result = NhanDonDA.SuaDoiNoiDungDon(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaDoiNoiDungDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }
        #endregion

        #region ChuyenDon
        public SelectList SelectListVungChuyenDon(string selectedValue)
        {
            var list = XMLUtils.BindData("VungChuyenDon");
            return new SelectList(list, "Value", "Text", selectedValue);
        }


        public ChuyenDonModel ChiTietChuyenDonTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int congDoan)
        {
            try
            {
                var dbModel = NhanDonDA.ChiTietChuyenDonTheoHoSoVuAnID(hoSoVuAnID, giaiDoan, congDoan);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietChuyenDonTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new ChuyenDonModel();
            }
        }

        public SelectList GetDanhSachNgaySuaDoiChuyenDon(int hoSoVuAnId, int giaiDoan, int congDoan, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.GetDanhSachNgaySuaDoiChuyenDon(hoSoVuAnId, giaiDoan, congDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd'/'MM'/'yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.ChuyenDonID.ToString()
                    }
                );
                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachNgaySuaDoiChuyenDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public ChuyenDonViewModel ChiTietChuyenDonTheoId(int chuyenDonId)
        {
            try
            {
                var dbModel = NhanDonDA.ChiTietChuyenDonTheoId(chuyenDonId);
                if (dbModel == null)
                    return null;
                return new ChuyenDonViewModel
                {
                    ChuyenDonID = dbModel.ChuyenDonID,
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    NgayRaThongBao = dbModel.NgayRaThongBao.ToString("dd'/'MM'/'yyyy"),
                    NgayChuyenDon = dbModel.NgayChuyenDon.ToString("dd'/'MM'/'yyyy"),
                    ToaAnChuyenDi = dbModel.ToaAnChuyenDi,
                    ToaAnChuyenDen = dbModel.ToaAnChuyenDen,
                    LyDoChuyenDon = dbModel.LyDoChuyenDon,
                    TruongHopChuyen = dbModel.TruongHopChuyen,
                    NhomNghiepVu = dbModel.NhomNghiepVu,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    TrangThai = dbModel.TrangThai,
                    NguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu,
                    TrangThaiCongDoan = dbModel.TrangThaiCongDoan
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietChuyenDonTheoId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new ChuyenDonViewModel();
            }
        }

        public ResponseResult SuaDoiChuyenDon(ChuyenDonViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                var dbModel = new ChuyenDonModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    NgayRaThongBao = DateTime.ParseExact(viewModel.NgayRaThongBao, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    NgayChuyenDon = DateTime.ParseExact(viewModel.NgayChuyenDon, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ToaAnChuyenDi = viewModel.ToaAnChuyenDi,
                    ToaAnChuyenDen = viewModel.ToaAnChuyenDen,
                    LyDoChuyenDon = viewModel.LyDoChuyenDon,
                    TruongHopChuyen = viewModel.TruongHopChuyen,
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = viewModel.CongDoanHoSo,
                    NguoiTao = viewModel.NguoiTao,
                    GhiChu = viewModel.GhiChu,
                    TrangThaiCongDoan = viewModel.TrangThaiCongDoan
                };
                result = NhanDonDA.SuaDoiChuyenDon(dbModel);

            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaDoiChuyenDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }
        #endregion

        #region ThamPhan
        public ThamPhanModel ChiTietThamPhanTheoHoSoVuAnID(int hoSoVuAnID)
        {
            try
            {
                var dbModel = NhanDonDA.ChiTietThamPhanTheoHoSoVuAnID(hoSoVuAnID);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietThamPhanTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new ThamPhanModel();
            }
        }

        public SelectList GetDanhSachNgaySuaDoiThamPhan(int hoSoVuAnId, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.GetDanhSachNgaySuaDoiThamPhan(hoSoVuAnId, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.ThamPhanID.ToString()
                    }
                );
                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachNgaySuaDoiThamPhan with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public ThamPhanViewModel ChiTietThamPhanTheoId(int thamPhanId)
        {
            try
            {
                var dbModel = NhanDonDA.ChiTietThamPhanTheoId(thamPhanId);
                return new ThamPhanViewModel
                {
                    ThamPhanID = dbModel.ThamPhanID,
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    ThamPhan = dbModel.ThamPhan,
                    NgayPhanCong = dbModel.NgayPhanCong.ToString("dd/MM/yyyy"),
                    TenNguoiPhanCong = dbModel.TenNguoiPhanCong,
                    HoiThamNhanDan2 = dbModel.HoiThamNhanDan2,
                    NhomNghiepVu = dbModel.NhomNghiepVu,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    TrangThai = dbModel.TrangThai,
                    NguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu,
                    NhanVienThamPhan = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan),
                    NhanVienPhanCong = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.TenNguoiPhanCong)
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietThamPhanTheoId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new ThamPhanViewModel();
            }
        }

        public ResponseResult SuaDoiThamPhan(ThamPhanViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                viewModel.CongDoanHoSo = 1;
                var dbModel = new ThamPhanModel
                {
                    ThamPhanID = viewModel.ThamPhanID,
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    ThamPhan = viewModel.ThamPhan,
                    NgayPhanCong = DateTime.ParseExact(viewModel.NgayPhanCong, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    TenNguoiPhanCong = viewModel.TenNguoiPhanCong,
                    HoiThamNhanDan2 = viewModel.HoiThamNhanDan2,
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = viewModel.CongDoanHoSo,
                    NguoiTao = viewModel.NguoiTao,
                    GhiChu = viewModel.GhiChu
                };
                result = NhanDonDA.SuaDoiThamPhan(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaDoiThamPhan with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }
        #endregion

        #region SuaDoiBoSungDon
        public SelectList DanhSachNgaySuaDoiBoSungDon(int hoSoVuAnID, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.DanhSachNgaySuaDoiBoSungDon(hoSoVuAnID, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.SuaDoiBoSungDonID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgaySuaDoiBoSungDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public SuaDoiBoSungDonModel ChiTietSuaDoiBoSungDonTheoHoSoVuAnID(int hoSoVuAnID)
        {
            try
            {
                var dbModel = NhanDonDA.ThongTinSuaDoiBoSungDonTheoHoSoVuAnID(hoSoVuAnID);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietSuaDoiBoSungDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new SuaDoiBoSungDonModel();
            }
        }

        public SuaDoiBoSungDonModel ChiTietSuaDoiBoSungDonTheoSuaDoiBoSungDonID(int suaDoiBoSungDonID)
        {
            try
            {
                var dbModel = NhanDonDA.ThongTinSuaDoiBoSungDonTheoSuaDoiBoSungDonID(suaDoiBoSungDonID);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietSuaDoiBoSungDonTheoID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new SuaDoiBoSungDonModel();
            }
        }

        public ResponseResult ThemSuaDoiBoSungDon(SuaDoiBoSungDonModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                viewModel.NgayYeuCau = Convert.ToDateTime(viewModel.NgayYeuCau);
                viewModel.ThoiHanSuaDoi = Convert.ToDateTime(viewModel.ThoiHanSuaDoi);
                result = NhanDonDA.ThemSuaDoiBoSungDon(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemSuaDoiBoSungDon with error [{0}]", ex.ToString()), AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }
        #endregion

        #region TraLaiDon
        public SelectList SelectListLyDo(string selectedValue)
        {
            var list = XMLUtils.BindData("LyDoTraDon");
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public SelectList DanhSachNgayTraLaiDon(int hoSoVuAnID, int giaiDoan, int CongDoan, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.DanhSachNgayTraLaiDon(hoSoVuAnID, giaiDoan, CongDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.TraLaiDonID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());

            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgayTraLaiDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public TraLaiDonModel ChiTietTraLaiDonTheoHoSoVuAnID(int hoSoVuAnID, int CongDoan)
        {
            try
            {
                var model = NhanDonDA.ThongTinTraLaiDonTheoHoSoVuAnID(hoSoVuAnID, CongDoan);
                if (model != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiTao).HoTenVaMaNV;
                    model.NguoiTao = nguoiTao;
                }
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietTraLaiDonTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new TraLaiDonModel();
            }
        }

        public TraLaiDonModel ChiTietTraLaiDonTheoTraLaiDonID(int traLaiDonID)
        {
            try
            {
                var model = NhanDonDA.ThongTinTraLaiDonTheoTraLaiDonID(traLaiDonID);
                if (model != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiTao).HoTenVaMaNV;
                    model.NguoiTao = nguoiTao;
                }
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietTraLaiDonTheoID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new TraLaiDonModel();
            }
        }

        public ResponseResult ThemTraLaiDon(TraLaiDonModel model)
        {
            ResponseResult result = null;
            try
            {
                model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                var dbModel = new TraLaiDonModel
                {
                    NguoiTao = model.NguoiTao,
                    NgayTraDon = Convert.ToDateTime(model.NgayTraDon),
                    GhiChu = model.GhiChu
                };
                result = NhanDonDA.ThemTraLaiDon(model);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemTraLaiDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        #endregion

        #region Khieu nai tra don
        public SelectList SelectListNhom(string selectedValue)
        {
            var list = XMLUtils.BindData("Nhom");
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public SelectList SelectListLanThu(int lanThu)
        {
            var list = new List<SelectListItem>();

            for (int i = 1; i <= (lanThu + 1); i++)
            {
                list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            return new SelectList(list, "Value", "Text", list);
        }

        public SelectList DanhSachNgayKhieuNaiTraDon(int hoSoVuAnID, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.DanhSachNgayKhieuNaiTraDon(hoSoVuAnID, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.KhieuNaiViecTraDonID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgayTraLaiDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public KhieuNaiTraDonModel ChiTietKhieuNaiTraDonTheoHoSoVuAnID(int hoSoVuAnID)
        {
            try
            {
                var dbModel = NhanDonDA.ThongTinKhieuNaiTraDonTheoHoSoVuAnID(hoSoVuAnID);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietKhieuNaiTraDonTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new KhieuNaiTraDonModel();
            }
        }

        public KhieuNaiTraDonModel ChiTietKhieuNaiTraDonTheoKhieuNaiTraDonID(int khieuNaiTraDonID)
        {
            try
            {
                var dbModel = NhanDonDA.ThongTinKhieuNaiTraDonTheoKhieuNaiTraDonID(khieuNaiTraDonID);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietKhieuNaiTraDonTheoKhieuNaiTraDonID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new KhieuNaiTraDonModel();
            }
        }

        public ResponseResult ThemKhieuNaiTraDon(KhieuNaiTraDonModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                viewModel.NgayKhieuNai = Convert.ToDateTime(viewModel.NgayKhieuNai);
                result = NhanDonDA.ThemKhieuNaiTraDon(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemKhieuNaiTraDon with error [{0}]", ex.ToString()), AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }
        #endregion

        #region Kien nghi viec tra don

        public SelectList SelectListKienNghiLanThu(int lanThu)
        {
            var list = new List<SelectListItem>();

            for (int i = 1; i <= (lanThu + 1); i++)
            {
                list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }


            return new SelectList(list, "Value", "Text", list);
        }

        public SelectList DanhSachNgayKienNghiTraDon(int hoSoVuAnID, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.DanhSachNgayKienNghiTraDon(hoSoVuAnID, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.KienNghiViecTraDonID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachKienNghiTraDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public KienNghiTraDonModel ChiTietKienNghiTraDonTheoHoSoVuAnID(int hoSoVuAnID)
        {
            try
            {
                var dbModel = NhanDonDA.ThongTinKienNghiTraDonTheoHoSoVuAnID(hoSoVuAnID);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietKienNghiTraDonTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new KienNghiTraDonModel();
            }
        }

        public KienNghiTraDonModel ChiTietKienNghiTraDonTheoKienNghiTraDonID(int KienNghiTraDonID)
        {
            try
            {
                var dbModel = NhanDonDA.ThongTinKienNghiTraDonTheoKienNghiTraDonID(KienNghiTraDonID);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietKienNghiTraDonTheoKienNghiTraDonID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new KienNghiTraDonModel();
            }
        }

        public ResponseResult ThemKienNghiTraDon(KienNghiTraDonModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                viewModel.NgayKienNghi = Convert.ToDateTime(viewModel.NgayKienNghi);
                result = NhanDonDA.ThemKienNghiTraDon(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemKienNghiTraDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        #endregion

        #region Searching on Header by Keyword
        public IEnumerable<HoSoVuAnModel> DanhSachHoSoVuAnSearchByKeyword(string keyword)
        {
            var danhSach = NhanDonDA.DanhSachHoSoVuAnSearchByKeyword(keyword);
            var results = new List<HoSoVuAnModel>();
            foreach (var entry in danhSach)
            {
                entry.NhanVienThamPhan = NhanDonDA.ThongTinNhanVien(entry.ThamPhan);

                entry.DuongSu = NguyenDonVaBiDon(entry.HoSoVuAnID);

                results.Add(entry);
            }
            return results;
        }
        #endregion

        #region Nhan Ho So Phuc Tham
        public IEnumerable<NhanHoSoPhucThamViewModel> GetDanhSachHoSoChoPhucTham(string maNhomAn)
        {
            try
            {
                var dbModel = NhanDonDA.DanhSachHoSoChoPhucTham(maNhomAn);
                List<NhanHoSoPhucThamViewModel> list = dbModel.Select(s => new NhanHoSoPhucThamViewModel
                {
                    STT = s.STT,
                    HoSoVuAnID = s.HoSoVuAnID,
                    MaHoSo = s.MaHoSo,
                    ChuyenDenTuToaAn = s.ToaAnChuyenDi,
                    NgayChuyenHoSo = string.Format("{0:dd/MM/yyyy}", s.NgayChuyenDon),
                    NguoiChuyenHoSo = SettingDataService.ChiTietNhanVienTheoMaNhanVien(s.NguoiTao).HoTenVaMaNV,
                    ChuyenDonID = s.ChuyenDonID,
                    TenCacDuongSu = (s.NhomAn == Setting.MANHOMAN_HINHSU ? s.TenVuAn : GetTenCuaCacDuongSu(s.HoSoVuAnID, s.NhomAn))
                }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachHoSoChoPhucTham with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public int GetSoLuongHoSoChoPhucTham(string maNhomAn)
        {
            int soLuongHoSo;
            try
            {
                soLuongHoSo = NhanDonDA.GetSoLuongHoSoChoPhucTham(maNhomAn);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetSoLuongHoSoChoPhucTham with error [{0}]", ex.ToString()), AppName.BizSecurity);
                soLuongHoSo = 0;
            }
            return soLuongHoSo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public ResponseResult CapNhatNhanHoSoTuToaAnKhac(ChuyenDonViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                var dbModel = new ChuyenDonModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    ChuyenDonID = viewModel.ChuyenDonID,
                    //NgayNhanHoSo = DateTime.ParseExact(viewModel.NgayNhanHoSo, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = 1, // Nhan Don / Nhan Ho So
                    NguoiTao = viewModel.NguoiTao,
                    GhiChu = viewModel.GhiChu,
                    TrangThaiCongDoan = "Nhận hồ sơ"
                };
                result = NhanDonDA.CapNhatNhanHoSoTuToaAnKhac(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("CapNhatNhanHoSoTuToaAnKhac with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public ResponseResult XacNhanNhanHoSoPhucTham(HoSoVuAnModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = NhanDonDA.XacNhanNhanHoSoPhucTham(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("XacNhanNhanHoSoPhucTham with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public string GetTenCuaCacDuongSu(int hoSoVuAnID, string nhomAn)
        {
            string tenDuongSu = null;
            var danhSachDuongSu = NguyenDonVaBiDon(hoSoVuAnID);
            var ten = new List<String>();
            if (nhomAn==Setting.MANHOMAN_APDUNG_BPXLHC)
            { 
                foreach (var duongSu in danhSachDuongSu)
                {
                    ten.Add(duongSu.HoVaTen);
                }
            }
            else if(nhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                var nguyendon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower() == "người khởi kiện").FirstOrDefault();
                if (nguyendon == null)
                {
                    nguyendon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower().Contains("đại diện") && x.TuCachThamGiaToTung.ToLower().Contains("khởi kiện")).FirstOrDefault();
                    if (nguyendon == null)
                    {
                        nguyendon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower().Contains("bảo vệ") && x.TuCachThamGiaToTung.ToLower().Contains("khởi kiện")).FirstOrDefault();
                        ten.Add(nguyendon.HoVaTen);
                    }
                    else
                        ten.Add(nguyendon.HoVaTen);
                }
                else
                    ten.Add(nguyendon.HoVaTen);

                var bidon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower() == "người bị kiện").FirstOrDefault();
                if (bidon == null)
                {
                    bidon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower().Contains("đại diện") && x.TuCachThamGiaToTung.ToLower().Contains("bị kiện")).FirstOrDefault();
                    if (bidon == null)
                    {
                        bidon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower().Contains("bảo vệ") && x.TuCachThamGiaToTung.ToLower().Contains("bị kiện")).FirstOrDefault();
                        ten.Add(bidon.HoVaTen);
                    }
                    else
                        ten.Add(bidon.HoVaTen);
                }
                else
                    ten.Add(bidon.HoVaTen);
            }
            else
            {
                var nguoiyeucau = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower() == "người yêu cầu").FirstOrDefault();
                if (nguoiyeucau!=null)
                {
                    ten.Add(nguoiyeucau.HoVaTen);
                    var nguoilienquan= danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower() == "người có quyền lợi, nghĩa vụ liên quan").FirstOrDefault();
                    if (nguoilienquan != null)
                        ten.Add(nguoilienquan.HoVaTen);
                }
                else
                {
                    var nguyendon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower() == "nguyên đơn").FirstOrDefault();
                    if (nguyendon == null)
                    {
                        nguyendon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower().Contains("đại diện") && x.TuCachThamGiaToTung.ToLower().Contains("nguyên đơn")).FirstOrDefault();
                        if (nguyendon == null)
                        {
                            nguyendon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower().Contains("kế thừa") && x.TuCachThamGiaToTung.ToLower().Contains("nguyên đơn")).FirstOrDefault();
                            if (nguyendon != null)
                                ten.Add(nguyendon.HoVaTen);
                        }
                        else
                            ten.Add(nguyendon.HoVaTen);
                    }
                    else
                        ten.Add(nguyendon.HoVaTen);

                    var bidon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower() == "bị đơn").FirstOrDefault();
                    if (bidon == null)
                    {
                        bidon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower().Contains("đại diện") && x.TuCachThamGiaToTung.ToLower().Contains("bị đơn")).FirstOrDefault();
                        if (bidon == null)
                        {
                            bidon = danhSachDuongSu.Where(x => x.TuCachThamGiaToTung.ToLower().Contains("kế thừa") && x.TuCachThamGiaToTung.ToLower().Contains("bị đơn")).FirstOrDefault();
                            if (bidon != null)
                                ten.Add(bidon.HoVaTen);
                        }
                        else
                            ten.Add(bidon.HoVaTen);
                    }
                    else
                        ten.Add(bidon.HoVaTen);
                }
                
            }
            tenDuongSu = String.Join(" - ", ten);
            return tenDuongSu;
        }
        #endregion

        #region Nhan ho so

        //public ResponseResult XacNhanNhanHoSo(HoSoVuAnModel viewModel)
        //{
        //    ResponseResult result = null;
        //    try
        //    {
        //        viewModel.NguoiTao = AccountUtils.CurrentUsername();
        //        result = NhanDonDA.XacNhanNhanHoSo(viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.Error(string.Format("XacNhanNhanHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
        //        result = null;
        //    }
        //    return result;
        //}

        public SelectList DanhSachNgaySuaNhanHoSo(int hoSoVuAnId, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = NhanDonDA.GetDanhSachNgaySuaDoiNhanHoSo(hoSoVuAnId, giaiDoan);
                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.NhanHoSoID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgaySuaNhanHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public NhanHoSoModel ChiTietNhanHoSoTheoId(int nhanHoSoId)
        {
            try
            {
                var dbModel = NhanDonDA.ChiTietNhanHoSoTheoId(nhanHoSoId);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietNhanHoSoTheoId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new NhanHoSoModel();
            }
        }

        public ResponseResult SuaChiTietNhanHoSo(NhanHoSoModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = NhanDonDA.SuaNhanHoSo(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaChiTietNhanHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }
        #endregion

        #region HoSoVuAn ADBPXLHC

        public HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHC(int hoSoVuAnID)
        {
            var model = NhanDonDA.ChiTietHoSoVuAnApDungBPXLHC(hoSoVuAnID);

            if (model != null)
            {
                model.NhanVienCanBoNhanDon = NhanDonDA.ThongTinNhanVien(model.CanBoNhanDon);
                model.DuongSu = NguyenDonVaBiDon(model.HoSoVuAnID);
            }

            return model;
        }

        public HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoGiaiDoan(int hoSoVuAnID, int giaiDoan)
        {
            var model = NhanDonDA.ChiTietHoSoVuAnApDungBPXLHCTheoGiaiDoan(hoSoVuAnID, giaiDoan);

            if (model != null)
            {
                model.NhanVienCanBoNhanDon = NhanDonDA.ThongTinNhanVien(model.CanBoNhanDon);
                model.DuongSu = NguyenDonVaBiDon(model.HoSoVuAnID);
            }

            return model;
        }

        public HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoLog(int id)
        {
            var model = NhanDonDA.ChiTietHoSoVuAnApDungBPXLHCTheoLog(id);

            if (model != null)
            {
                model.NhanVienCanBoNhanDon = NhanDonDA.ThongTinNhanVien(model.CanBoNhanDon);
                model.DuongSu = NguyenDonVaBiDon(model.HoSoVuAnID);
            }

            return model;
        }

        public HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoId(int hoSoVuAnID)
        {
            try
            {
                return NhanDonDA.ChiTietHoSoVuAnApDungBPXLHC(hoSoVuAnID);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietHoSoVuAnApDungBPXLHCTheoID with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public ResponseResult ThemHoSoVuAnApDungBPXLHC(HoSoVuAnApDungModel model)
        {
            ResponseResult result = null;

            model.NguoiCapNhat = SettingDataService.GetNhanVienSessionInfo().MaNV;
            model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
            model.NgayNopDonTaiToaAn = Convert.ToDateTime(model.NgayNopDonTaiToaAn);

            result = NhanDonDA.ThemHoSoVuAnApDungBPXLHC(model);

            return result;
        }

        public ResponseResult SuaHoSoVuAnApDungBPXLHC(HoSoVuAnApDungModel model)
        {
            ResponseResult result = null;

            model.NguoiCapNhat = SettingDataService.GetNhanVienSessionInfo().MaNV;
            model.NgayNopDonTaiToaAn = Convert.ToDateTime(model.NgayNopDonTaiToaAn);

            result = NhanDonDA.SuaHoSoVuAnApDungBPXLHC(model);

            return result;
        }

        #endregion

        #region HoSoVuAn HinhSu

        public HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSu(int hoSoVuAnID)
        {
            var model = NhanDonDA.ChiTietHoSoVuAnHinhSu(hoSoVuAnID);

            if (model != null)
            {
                model.NhanVienCanBoNhanDon = NhanDonDA.ThongTinNhanVien(model.CanBoNhanDon);
                model.DuongSu = NguyenDonVaBiDon(model.HoSoVuAnID);
                model.NguoiTao = NhanDonDA.ThongTinNhanVien(model.NguoiTao).HoTenVaMaNV;
            }

            return model;
        }

        public HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoGiaiDoan(int hoSoVuAnID, int giaiDoan)
        {
            var model = NhanDonDA.ChiTietHoSoVuAnHinhSuTheoGiaiDoan(hoSoVuAnID, giaiDoan);

            if (model != null)
            {
                model.NhanVienCanBoNhanDon = NhanDonDA.ThongTinNhanVien(model.CanBoNhanDon);
                model.DuongSu = NguyenDonVaBiDon(model.HoSoVuAnID);
                model.NguoiTao = NhanDonDA.ThongTinNhanVien(model.NguoiTao).HoTenVaMaNV;
            }

            return model;
        }

        public HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoLog(int id)
        {
            var model = NhanDonDA.ChiTietHoSoVuAnHinhSuTheoLog(id);

            if (model != null)
            {
                model.NhanVienCanBoNhanDon = NhanDonDA.ThongTinNhanVien(model.CanBoNhanDon);
                model.DuongSu = NguyenDonVaBiDon(model.HoSoVuAnID);
                model.NguoiTao = NhanDonDA.ThongTinNhanVien(model.NguoiTao).HoTenVaMaNV;
            }

            return model;
        }

        public HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoId(int hoSoVuAnID)
        {
            try
            {
                return NhanDonDA.ChiTietHoSoVuAnHinhSu(hoSoVuAnID);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietHoSoVuAnHinhSuTheoId with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public ResponseResult ThemHoSoVuAnHinhSu(HoSoVuAnHinhSuModel model)
        {
            ResponseResult result = null;
            var nv = SettingDataService.GetNhanVienSessionInfo().MaNV;
            model.NguoiCapNhat = nv;
            model.NguoiTao = nv;
            model.NgayTruyTo = Convert.ToDateTime(model.NgayTruyTo);

            result = NhanDonDA.ThemHoSoVuAnHinhSu(model);

            return result;
        }

        public ResponseResult SuaHoSoVuAnHinhSu(HoSoVuAnHinhSuModel model)
        {
            ResponseResult result = null;

            model.NguoiCapNhat = SettingDataService.GetNhanVienSessionInfo().MaNV;
            model.NgayTruyTo = Convert.ToDateTime(model.NgayTruyTo);

            result = NhanDonDA.SuaHoSoVuAnHinhSu(model);

            return result;
        }

        #endregion

        #region ToiDanhTruyTo

        
        public IEnumerable<ToiDanhTruyToModel> GetDanhSachToiDanhTruyTo(int hoSoVuAnId)
        {
            try
            {
                var dbModel = NhanDonDA.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
                foreach(var item in dbModel)
                {
                    var dieuluat = DanhSachToiDanhTruyToKhoanDiemTheoId(item.ToiDanhTruyToID);
                    if(dieuluat!=null && dieuluat.Any())
                    {
                        item.DieuLuatApDung = "";
                        foreach (var it in dieuluat)
                        {
                            if (it.Khoan != null)
                                item.DieuLuatApDung +=  it.Khoan ;
                            if (it.Diem != null)
                                item.DieuLuatApDung += ", " + ViewText.LABEL_DIEM + " " + it.Diem + "; ";
                            else
                                item.DieuLuatApDung += "; ";
                        }
                        item.DieuLuatApDung = item.DieuLuatApDung.Remove(item.DieuLuatApDung.LastIndexOf(";"), 2);
                    }                    
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDanhSachToiDanhTruyTo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public ToiDanhTruyToModel ChiTietToiDanhTruyToTheoId(int toiDanhTruyToId)
        {
            try
            {
                var model = NhanDonDA.ChiTietToiDanhTruyToTheoId(toiDanhTruyToId);
                model.KhoanDiem = NhanDonDA.KhoanDiemTheoToiDanhTruyToId(toiDanhTruyToId);
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietToiDanhTruyToTheoId with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<ToiDanhTruyTo_KhoanDiemModel> DanhSachToiDanhTruyToKhoanDiemTheoId(int toiDanhTruyToId)
        {
            try
            {
                var model =  NhanDonDA.KhoanDiemTheoToiDanhTruyToId(toiDanhTruyToId);
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachToiDanhTruyToKhoanDiemTheoId with error [{ex.ToString()}]", AppName.BizSecurity);
                return new List<ToiDanhTruyTo_KhoanDiemModel>();
            }
        }

        public ResponseResult ThemToiDanhTruyTo(ToiDanhTruyToModel model)
        {
            ResponseResult result = null;
            try
            {
                model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = NhanDonDA.ThemToiDanhTruyTo(model);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemToiDanhTruyTo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public ResponseResult ThemToiDanhTruyTo_KhoanDiem(ToiDanhTruyTo_KhoanDiemModel model)
        {
            ResponseResult result = null;
            try
            {
                model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = NhanDonDA.ThemToiDanhTruyTo_KhoanDiem(model);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemToiDanhTruyTo_KhoanDiem with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public ResponseResult SuaToiDanhTruyTo(ToiDanhTruyToModel model)
        {
            ResponseResult result = null;
            try
            {
                model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = NhanDonDA.SuaToiDanhTruyTo(model);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaToiDanhTruyTo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public ResponseResult SuaToiDanhTruyTo_KhoanDiem(ToiDanhTruyTo_KhoanDiemModel model)
        {
            ResponseResult result = null;
            try
            {
                model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = NhanDonDA.SuaToiDanhTruyTo_KhoanDiem(model);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaToiDanhTruyTo_KhoanDiem with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public ResponseResult XoaToiDanhTruyTo(int toiDanhTruyToId)
        {
            ResponseResult result = null;
            try
            {
                result = NhanDonDA.XoaToiDanhTruyTo(toiDanhTruyToId, SettingDataService.GetNhanVienSessionInfo().MaNV);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("XoaToiDanhTruyTo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }


        public ResponseResult XoaToiDanhTruyTo_KhoanDiem(int khoanDiemID)
        {
            ResponseResult result = null;
            try
            {
                result = NhanDonDA.XoaToiDanhTruyTo_KhoanDiem(khoanDiemID, SettingDataService.GetNhanVienSessionInfo().MaNV);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("XoaToiDanhTruyTo_KhoanDiem with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }
        #endregion
    }
}