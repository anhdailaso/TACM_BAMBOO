using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Biz.Lib.Authentication;
using Biz.Lib.TACM.SauXetXu.IDataAccess;
using Biz.Lib.TACM.SauXetXu.Model.TamUngAnPhi;
using Biz.Lib.TACM.SauXetXu.Model.KhangCao.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.KhangCaoQuaHan.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.KhangNghi.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.LuuKho.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.Shared.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.SuaChuaBanAn.DataRequest;
using Biz.TACM.IServices;
using Biz.TACM.Models.Model.SauXetXu.KhangCao;
using Biz.TACM.Models.Model.SauXetXu.KhangCaoQuaHan;
using Biz.TACM.Models.Model.SauXetXu.KhangNghi;
using Biz.TACM.Models.Model.SauXetXu.LuuKho;
using Biz.TACM.Models.Model.SauXetXu.PhatHanhBanAn;
using Biz.TACM.Models.Model.SauXetXu.SuaChuaBanAn;
using Biz.TACM.Models.ViewModel.SauXetXu.TamUngAnPhi;
using Biz.TACM.Models.ViewModel.SauXetXu.ChuyenHoSo;
using Biz.Lib.TACM.NhanDon.Model;
using Biz.Lib.TACM.NhanDon.DataAccess;
using Biz.Lib.TACM.NhanDon.IDataAccess;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.Models.ViewModel.SauXetXu.Shared;
using DuongSuModel = Biz.Lib.TACM.SauXetXu.Model.Shared.DuongSuModel;

namespace Biz.TACM.Services
{
    public class SauXetXuService : ISauXetXuService
    {
        private readonly ISauXetXuDataAccess _sauXetXuDataAccess;
        private readonly ISettingDataService _settingDataService;
        private INhanDonDataAccess _nhanDonDA;
        private INhanDonDataAccess NhanDonDA => _nhanDonDA ?? (_nhanDonDA = new NhanDonDataAccess());
        public SauXetXuService(ISauXetXuDataAccess sauXetXuDataAccess, ISettingDataService settingDataService)
        {
            _sauXetXuDataAccess = sauXetXuDataAccess;
            _settingDataService = settingDataService;
        }

        #region PhatHanhBanAn
        public IEnumerable<NgayPhatHanhBanAnModel> DanhSachNgayPhatHanhBanAn(int hoSoVuAnId, int giaiDoan)
        {
            var model = _sauXetXuDataAccess.DanhSachNgayPhatHanhBanAn(hoSoVuAnId, giaiDoan);
            return model.Select(s => new NgayPhatHanhBanAnModel
            {
                PhatHanhBanAnId = s.PhatHanhBanAnID,
                NgayPhatHanh = s.NgayTao.ToString("dd/MM/yyyy HH:mm:ss")
            }).ToList();
        }

        public ThongTinPhatHanhBanAnModel ThongTinPhatHanhBanAnTheoId(int phatHanhBanAnId)
        {
            var model = _sauXetXuDataAccess.ThongTinPhatHanhBanAnTheoId(phatHanhBanAnId);
            return new ThongTinPhatHanhBanAnModel
            {
                PhatHanhBanAnId = model.PhatHanhBanAnID,
                NgayPhatHanh = model.NgayPhatHanh.ToString("dd/MM/yyyy"),
                HieuLuc =  string.Format("{0:dd/MM/yyyy}",model.HieuLuc),
                NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiTao).HoTenVaMaNV,
                NgayTao = model.NgayTao.ToString("HH:mm:ss, dd/MM/yyyy")
            };
        }

        public IEnumerable<DuongSuViewModel> DanhSachDuongSuNhanPhatHanhBanAn(int phatHanhBanAnId)
        {
            var model = _sauXetXuDataAccess.DanhSachDuongSuNhanPhatHanhBanAn(phatHanhBanAnId);
            var modelThongTinPhatHanhBanAn = _sauXetXuDataAccess.ThongTinPhatHanhBanAnTheoId(phatHanhBanAnId);
            return model.Select(s => new DuongSuViewModel
            {
                DuongSuId = s.DuongSuID,
                HoVaTen = s.HoVaTen,
                TuCachThamGiaToTung = s.TuCachThamGiaToTung,
                NgayNhanPhatHanhBanAn = s.NgayNhanPhatHanhBanAn != null ? $"{s.NgayNhanPhatHanhBanAn:dd/MM/yyyy}" : modelThongTinPhatHanhBanAn.NgayPhatHanh.ToString("dd/MM/yyyy")
            }).ToList();
        }

        public IEnumerable<DuongSuViewModel> DanhSachDuongSuTheoHoSo(int hoSoVuAnId, int? phatHanhBanAnId)
        {
            var model = _sauXetXuDataAccess.DanhSachDuongSuTheoHoSo(hoSoVuAnId);
            var modelDuongSuNhanPhatHanhBanAn = _sauXetXuDataAccess.DanhSachDuongSuNhanPhatHanhBanAn(phatHanhBanAnId.GetValueOrDefault());
            List<DuongSuViewModel> danhSachDuongSu = new List<DuongSuViewModel>();
            foreach (var item in model)
            {
                DuongSuViewModel temp = new DuongSuViewModel
                {
                    DuongSuId = item.DuongSuID,
                    HoVaTen = item.HoVaTen,
                    TuCachThamGiaToTung = item.TuCachThamGiaToTung
                };
                var findingDuongSu = modelDuongSuNhanPhatHanhBanAn.SingleOrDefault(m=>m.DuongSuID == item.DuongSuID);
                if (findingDuongSu != null)
                {
                    temp.NgayNhanPhatHanhBanAn = string.Format("{0:dd/MM/yyyy}", findingDuongSu.NgayNhanPhatHanhBanAn);
                }
                danhSachDuongSu.Add(temp);
            }
            return danhSachDuongSu;
        }

        public bool PhatHanhBanAn(int hoSoVuAnId, DateTime ngayPhatHanhBanAn, DateTime? hieuLuc, IEnumerable<int> duongSuNhanPhatHanhBanAnIds, IEnumerable<string> duongSuNgayPhatHanh)
        {
            var dataRequest = new PhatHanhBanAnDataRequest
            {
                NgayPhatHanh = ngayPhatHanhBanAn,
                HieuLuc = hieuLuc,
                HoSoVuAnId = hoSoVuAnId,
                GiaiDoan = null,
                CongDoanHoSo = null,
                NgayTao = DateTime.Now,
                NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                GhiChu = null,
                DuongSuNhanPhatHanhBanAnIds = duongSuNhanPhatHanhBanAnIds,
                DuongSuNgayPhatHanh = duongSuNgayPhatHanh
            };

            var result = _sauXetXuDataAccess.PhatHanhBanAn(dataRequest);
            return result;
        }

        #endregion

        #region SuaChuaBanAn
        public IEnumerable<NgaySuaChuaBanAnModel> DanhSachNgaySuaChuaBanAn(int hoSoVuAnId, int giaiDoan)
        {
            var model = _sauXetXuDataAccess.DanhSachNgaySuaChuaBanAn(hoSoVuAnId, giaiDoan);
            return model.Select(s => new NgaySuaChuaBanAnModel
            {
                SuaChuaBanAnId = s.SuaChuaBanAnID,
                NgaySuaChua = s.NgayTao.ToString("dd/MM/yyyy HH:mm:ss")
            }).ToList();
        }

        public ThongTinSuaChuaBanAnModel ThongTinSuaChuaBanAnTheoId(int suaChuaBanAnId)
        {
            var model = _sauXetXuDataAccess.ThongTinSuaChuaBanAnTheoId(suaChuaBanAnId);
            return new ThongTinSuaChuaBanAnModel
            {
                SuaChuaBanAnId = model.SuaChuaBanAnID,
                NgaySuaChua = model.NgaySuaChua.ToString("dd/MM/yyy"),
                LyDo = model.LyDo,
                NguoiKy = model.NguoiKy,
                NoiDung = model.NoiDungSuaChua,
                DinhKemFile = model.DinhKemFile,
                NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiTao).HoTenVaMaNV,
                NgayTao = model.NgayTao.ToString("HH:mm:ss, dd/MM/yyy")
            };
        }


        public bool SuaChuaBanAn(int hoSoVuAnId, DateTime ngaySuaChua, string lyDo, string nguoiKy, string noiDung, string dinhKemFile)
        {
            var dataRequest = new SuaChuaBanAnDataRequest
            {
                HoSoVuAnId = hoSoVuAnId,
                NgaySuaChua = ngaySuaChua,
                GiaiDoan = null,
                CongDoanHoSo = null,
                LyDo = lyDo,
                NguoiKy = nguoiKy,
                NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                NgayTao = DateTime.Now,
                NoiDungSuaChua = noiDung,
                DinhKemFile = dinhKemFile,
                GhiChu = null
            };

            var result = _sauXetXuDataAccess.SuaChuaBanAn(dataRequest);
            return result;
        }
        #endregion

        #region KhangNghi
        public IEnumerable<NgayKhangNghiModel> DanhSachNgayKhangNghi(int hoSoVuAnId, int giaiDoan)
        {
            var model = _sauXetXuDataAccess.DanhSachNgayKhangNghi(hoSoVuAnId, giaiDoan);
            return model.Select(s => new NgayKhangNghiModel
            {
                KhangNghiId = s.KhangNghiID,
                NgayToaAnNhanVanBan = s.NgayTao.ToString("dd/MM/yyyy HH:mm:ss")
            }).ToList();
        }

        public ThongTinKhangNghiModel ThongTinKhangNghiTheoId(int khangNghiId)
        {
            var model = _sauXetXuDataAccess.ThongTinKhangNghiTheoId(khangNghiId);
            var danhSachNguoiBiKhangNghi = new List<string>();

            if (model.NguoiBiKhangNghi != null)
            {
                var list = model.NguoiBiKhangNghi.Split(',');
                foreach (string item in list)
                {
                    danhSachNguoiBiKhangNghi.Add(item);
                }
            }
            return new ThongTinKhangNghiModel
            {
                HoSoVuAnID = model.HoSoVuAnID,
                KhangNghiId = model.KhangNghiID,
                NgayToaAnNhanVanBan = model.NgayToaAnNhanVanBan.ToString("dd/MM/yyy"),
                VienKiemSatKhangNghi = model.VienKiemSatKhangNghi,
                VanBanKhangNghi = model.VanBanKhangNghi,
                NoiDungKhangNghi = model.NoiDungKhangNghi,
                NguoiBiKhangNghi = model.NguoiBiKhangNghi,
                DanhSachNguoiBiKhangNghi = danhSachNguoiBiKhangNghi,
                HinhThuc = model.HinhThuc,
                NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiTao).HoTenVaMaNV,
                NgayTao = model.NgayTao.ToString("HH:mm:ss, dd/MM/yyy")
            };
        }

        public bool KhangNghi(int hoSoVuAnId, DateTime ngayToaAnNhanVanBan, string vienKiemSatKhangNghi, string vanBanKhangNghi,
            string hinhThuc, string noiDung, List<string> nguoiBiKhangNghi)
        {
            string danhSachNguoiBiKhangNghi = "";
            if (nguoiBiKhangNghi != null)
                danhSachNguoiBiKhangNghi = String.Join(",", nguoiBiKhangNghi);

            var dataRequest = new KhangNghiDataRequest
            {
                HoSoVuAnId = hoSoVuAnId,
                NgayToaAnNhanVanBan = ngayToaAnNhanVanBan,
                GiaiDoan = null,
                CongDoanHoSo = null,
                VienKiemSatKhangNghi = vienKiemSatKhangNghi,
                VanBanKhangNghi = vanBanKhangNghi,
                NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                NgayTao = DateTime.Now,
                NoiDungKhangNghi = noiDung,
                GhiChu = null,
                HinhThuc = hinhThuc,
                NguoiBiKhangNghi = danhSachNguoiBiKhangNghi
            };

            var result = _sauXetXuDataAccess.KhangNghi(dataRequest);
            return result;
        }
        #endregion

        #region KhangCaoQuaHan
        public IEnumerable<NgayKhangCaoQuaHanModel> DanhSachNgayKhangCaoQuaHan(int hoSoVuAnId, int giaiDoan)
        {
            var model = _sauXetXuDataAccess.DanhSachNgayKhangCaoQuanHan(hoSoVuAnId, giaiDoan);
            return model.Select(s => new NgayKhangCaoQuaHanModel
            {
                KhangCaoQuaHanId = s.KhangCaoQuaHanID,
                NgayTao = s.NgayTao.ToString("dd/MM/yyyy HH:mm:ss")
            }).ToList();
        }

        public ThongTinKhangCaoQuaHanModel ThongTinKhangCaoQuaHanTheoId(int khangCaoQuaHanId)
        {
            var model = _sauXetXuDataAccess.ThongTinKhangCaoQuaHanTheoId(khangCaoQuaHanId);
            return new ThongTinKhangCaoQuaHanModel
            {
                KhangCaoQuaHanId = model.KhangCaoQuaHanID,
                KhangCaoHayKhangNghi = model.KhangCaoHayKhangNghi,
                LyDo = model.LyDo,
                KetQua = model.KetQua
            };
        }

        public bool KhangCaoQuaHan(int hoSoVuAnId, string khangCaoHayKhangNghi, string lyDo, string ketQua)
        {
            var dataRequest = new KhangCaoQuaHanDataRequest
            {
                HoSoVuAnId = hoSoVuAnId,
                KhangCaoHayKhangNghi = khangCaoHayKhangNghi,
                LyDo = lyDo,
                KetQua = ketQua,
                GiaiDoan = null,
                CongDoanHoSo = null,
                NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                NgayTao = DateTime.Now,
                GhiChu = null
            };

            var result = _sauXetXuDataAccess.KhangCaoQuaHan(dataRequest);
            return result;
        }

        public SelectList KhangCaoHayKhangNghi(string selectedValue)
        {
            var list = XMLUtils.BindData("KhangCaoKhangNghiQuaHan");
            return new SelectList(list, "Value", "Text", selectedValue);
        }
        #endregion

        #region LuuKho
        public IEnumerable<NgayLuuKhoModel> DanhSachNgayLuuKho(int hoSoVuAnId, int giaiDoan)
        {
            var model = _sauXetXuDataAccess.DanhSachNgayLuuKho(hoSoVuAnId, giaiDoan);
            return model.Select(s => new NgayLuuKhoModel
            {
                LuuKhoId = s.LuuKhoID,
                NgayTao = s.NgayTao.ToString("dd/MM/yyyy HH:mm:ss")
            }).ToList();
        }

        public ThongTinLuuKhoModel ThongTinLuuKhoTheoId(int luuKhoId)
        {
            var model = _sauXetXuDataAccess.ThongTinLuuKhoTheoId(luuKhoId);
            return new ThongTinLuuKhoModel
            {
                LuuKhoId = model.LuuKhoID,
                NgayLuu = model.NgayLuu.ToString("dd/MM/yyyy"),               
                GhiChuTinhTrangHoSo = model.GhiChuTinhTrangHoSo,
                GhiChu = model.GhiChu,
                NguoiGiao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiGiao).HoTenVaMaNV,
                NguoiNhanLuu = _settingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiNhanLuu).HoTenVaMaNV,
                NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiTao).HoTenVaMaNV,
                NgayTao = model.NgayTao.ToString("HH:mm:ss, dd/MM/yyyy")
            };
        }

        public bool LuuKho(LuuKhoDataRequest model)
        {

            var result = _sauXetXuDataAccess.LuuKho(model);
            return result;
        }
        #endregion

        #region KhangCao
        public IEnumerable<KhangCaoModel> DanhSachKhangCao(int hoSoVuAnId, int giaiDoan)
        {
            var model = _sauXetXuDataAccess.DanhSachKhangCao(hoSoVuAnId, giaiDoan);
            return model.Select(s => new KhangCaoModel
            {
                KhangCaoId = s.KhangCaoID,
                NguoiKhangCao = s.NguoiKhangCao,
                TuCachThamGiaToTung = s.TuCachThamGiaToTung,
                NgayLamDon = s.NgayLamDon.ToString("dd/MM/yyyy"),
                NgayNopDon = s.NgayNopDon.ToString("dd/MM/yyyy"),
                HinhThucNopDon = s.HinhThucNopDon,
                TaiLieuChungTuKemTheo = s.TaiLieuChungTuKemTheo,
                TinhTrangKhangCao = s.TinhTrangKhangCao, 
                NguoiKhieuNai = s.NguoiKhieuNai,
                NguoiNhanKhieuNai = _settingDataService.ChiTietNhanVienTheoMaNhanVien(s.NguoiNhanKhieuNai).HoVaTenNV,
            }).ToList();
        }

        public ThongTinKhangCaoModel ThongTinKhangCaoTheoId(int khangCaoId)
        {
            var model = _sauXetXuDataAccess.ThongTinKhangCaoTheoId(khangCaoId);
            var danhSachNguoiBiKhangCao = new List<string>();
 
            if (model.NguoiBiKhangCao != null)
            {
                var list = model.NguoiBiKhangCao.Split(',');
                foreach (string item in list)
                {
                    danhSachNguoiBiKhangCao.Add(item);
                }
            }

            return new ThongTinKhangCaoModel
            {
                KhangCaoId = model.KhangCaoID,
                DuongSuId = model.DuongSuID,
                HoSoVuAnId = model.HoSoVuAnID,
                NguoiKhangCao = model.NguoiKhangCao,                
                TuCachThamGiaToTung = model.TuCachThamGiaToTung,
                NgayLamDon = model.NgayLamDon.ToString("dd/MM/yyyy"),
                NgayNopDon = model.NgayNopDon.ToString("dd/MM/yyyy"),
                HinhThucNopDon = model.HinhThucNopDon,
                TinhTrangKhangCao = model.TinhTrangKhangCao,
                NoiDungKhangCao = model.NoiDungKhangCao,
                TaiLieuChungTuKemTheo = model.TaiLieuChungTuKemTheo,         
                NguoiKhieuNai = model.NguoiKhieuNai,                
                LyDo = model.LyDo,
                NguoiNhanKhieuNai = _settingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiNhanKhieuNai).HoTenVaMaNV,
                NguoiBiKhangCao = model.NguoiBiKhangCao,
                DanhSachNguoiBiKhangCao = danhSachNguoiBiKhangCao,
                NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(model.NguoiTao).HoTenVaMaNV,
                NgayTao = model.NgayTao.ToString("HH:mm:ss, dd/MM/yyyy"),
                GhiChu = model.GhiChu
            };
        }

        public IEnumerable<DuongSuViewModel> DanhSachDuongSuTheoHoSo(int hoSoVuAnId)
        {
            var model = _sauXetXuDataAccess.DanhSachDuongSuTheoHoSo(hoSoVuAnId);
            return model.Select(s => new DuongSuViewModel
            {
                DuongSuId = s.DuongSuID,
                HoVaTen = s.HoVaTen,
                TuCachThamGiaToTung = s.TuCachThamGiaToTung,
                NgayNhanPhatHanhBanAn = s.NgayNhanPhatHanhBanAn.ToString()
            }).ToList();
        }

        public IEnumerable<DuongSuViewModel> DanhSachDuongSuKhacBiCaoTheoHoSo(int hoSoVuAnId)
        {
            var model = _sauXetXuDataAccess.DanhSachDuongSuTheoHoSo(hoSoVuAnId);
            return model.Select(s => new DuongSuViewModel
            {
                DuongSuId = s.DuongSuID,
                HoVaTen = s.HoVaTen,
                TuCachThamGiaToTung = s.TuCachThamGiaToTung,
                NgayNhanPhatHanhBanAn = s.NgayNhanPhatHanhBanAn.ToString()
            }).Where(x => x.TuCachThamGiaToTung != Setting.HS_TUCACHTOTUNG_BICAO).ToList();
        }
        public IEnumerable<DuongSuViewModel> DanhSachBiCao(int hoSoVuAnId)
        {
            var model = _sauXetXuDataAccess.DanhSachDuongSuTheoHoSo(hoSoVuAnId);
            return model.Select(s => new DuongSuViewModel
            {
                DuongSuId = s.DuongSuID,
                HoVaTen = s.HoVaTen,
                TuCachThamGiaToTung = s.TuCachThamGiaToTung
            }).Where(x=>x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO).ToList();
        }

        public IEnumerable<DuongSuViewModel> DanhSachBiCaoSelected(int hoSoVuAnId, List<string> selected)
        {
            try
            {
                var model = _sauXetXuDataAccess.DanhSachDuongSuTheoHoSo(hoSoVuAnId);
                var listDuongSu = model.Select(s => new DuongSuModel
                {
                    DuongSuID = s.DuongSuID,
                    HoVaTen = s.HoVaTen,
                    TuCachThamGiaToTung = s.TuCachThamGiaToTung
                }).Where(x => x.TuCachThamGiaToTung == Setting.HS_TUCACHTOTUNG_BICAO).ToList();

                var list = new List<DuongSuModel>();

                foreach (var item in listDuongSu)
                {
                    bool check = selected != null && selected.Contains(item.HoVaTen);
                    item.Checked = check;
                    list.Add(item);
                }

                return list.Select(s => new DuongSuViewModel
                {
                    DuongSuId = s.DuongSuID,
                    HoVaTen = s.HoVaTen,
                    TuCachThamGiaToTung = s.TuCachThamGiaToTung,
                    Checked = s.Checked
                }).ToList();
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachDuongSuSelected with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public bool KhangCao(int hoSoVuAnId, int nguoiKhangCao, DateTime ngayLamDon, DateTime ngayNopDon, string hinhThucNop,
            string tinhTrangKhangcao, string noiDungKhangCao, string taiLieuChungTuKemTheo, string nguoiKhieuNai, string lyDo, string ghiChu, List<string> nguoiBiKhangCao)
        {

            string danhSachNguoiBiKhangCao = "";
            if (nguoiBiKhangCao != null)
                danhSachNguoiBiKhangCao = String.Join(",", nguoiBiKhangCao);

            var dataRequest = new KhangCaoDataRequest
            {
                HoSoVuAnId = hoSoVuAnId,
                GiaiDoan = null,
                CongDoanHoSo = null,
                NguoiKhangCao = nguoiKhangCao,
                NgayLamDon = ngayLamDon,
                NgayNopDon = ngayNopDon,
                HinhThucNopDon = hinhThucNop,
                TinhTrangKhangCao = tinhTrangKhangcao,
                NoiDungKhangCao = noiDungKhangCao,
                TaiLieuChungTuKemTheo = taiLieuChungTuKemTheo,
                NguoiKhieuNai = nguoiKhieuNai,
                LyDo = lyDo,
                NguoiNhanKhieuNai = _settingDataService.GetNhanVienSessionInfo().MaNV,
                NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                NgayTao = DateTime.Now,
                GhiChu = ghiChu,
                NguoiBiKhangCao = danhSachNguoiBiKhangCao
            };

            var result = _sauXetXuDataAccess.KhangCao(dataRequest);
            return result;
        }

        public bool SuaKhangCao(int khangCaoId, int nguoiKhangCao, DateTime ngayLamDon, DateTime ngayNopDon, string hinhThucNop,
            string tinhTrangKhangcao, string noiDungKhangCao, string taiLieuChungTuKemTheo, string nguoiKhieuNai, string lyDo, string ghiChu, List<string> nguoiBiKhangCao)
        {
            string danhSachNguoiBiKhangCao = "";
            if (nguoiBiKhangCao != null)
                danhSachNguoiBiKhangCao = String.Join(",", nguoiBiKhangCao);

            var dataRequest = new SuaKhangCaoDataRequest
            {
                KhangCaoId = khangCaoId,
                NguoiKhangCao = nguoiKhangCao,
                NgayLamDon = ngayLamDon,
                NgayNopDon = ngayNopDon,
                HinhThucNopDon = hinhThucNop,
                TinhTrangKhangCao = tinhTrangKhangcao,
                NoiDungKhangCao = noiDungKhangCao,
                TaiLieuChungTuKemTheo = taiLieuChungTuKemTheo,
                NguoiKhieuNai = nguoiKhieuNai,
                LyDo = lyDo,
                NguoiNhanKhieuNai = _settingDataService.GetNhanVienSessionInfo().MaNV,
                NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                NgayTao = DateTime.Now,
                GhiChu = ghiChu,
                NguoiBiKhangCao = danhSachNguoiBiKhangCao
            };

            var result = _sauXetXuDataAccess.SuaKhangCao(dataRequest);
            return result;
        }

        public bool XoaKhangCao(int khangCaoId)
        {
            var result = _sauXetXuDataAccess.XoaKhangCao(khangCaoId);
            return result;
        }
        #endregion

        #region TamUngAnPhi
        public SelectList DanhSachNgaySuaDoiTamUngAnPhi(int hoSoVuAnId, int giaiDoan, int congDoan, int selected)
        {
            var dbModel = _sauXetXuDataAccess.DanhSachNgaySuaDoiTamUngAnPhi(hoSoVuAnId, giaiDoan, congDoan);
            var listNgayTao = dbModel.Select(
                x => new SelectListItem
                {
                    Text = x.NgayTao.ToString("dd/MM/yyyy hh:mm:ss"),
                    Value = x.AnPhiID.ToString()
                }
            );
            return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
        }

        public TamUngAnPhiViewModel ChiTietTamUngAnPhiTheoId(int hoSoVuAnId, int anPhiId, int giaiDoan, int congDoan)
        {
            var dbModel = _sauXetXuDataAccess.ChiTietTamUngAnPhiTheoId(hoSoVuAnId, anPhiId, giaiDoan, congDoan);
            if (dbModel == null)
                return null;
            var yeuCauDuNopAnPhiModel = new YeuCauDuNopAnPhiViewModel
            {
                NopAnPhi = dbModel.NopAnPhi,
                GiaTriTranhChap = string.Format("{0:n0}", dbModel.GiaTriTranhChap),
                TongAnPhi = string.Format("{0:n0}", dbModel.TongAnPhi),
                PhanTramDuNop = String.Format("{0:#0.0}", dbModel.PhanTramDuNop),
                GiaTriDuNop = string.Format("{0:n0}", dbModel.GiaTriDuNop),
                HanNopAnPhi = $"{dbModel.HanNopAnPhi:dd/MM/yyyy}",
                CoQuanThiHanhAnThu = dbModel.CoQuanThiHanhAnThu,
                DiaChiCoQuanThiHanhAnThu = dbModel.DiaChiCoQuanThiHanhAnThu,
                NgayRaThongBao = $"{dbModel.NgayRaThongBao:dd/MM/yyyy}",
                NgayGiaoThongBao = $"{dbModel.NgayGiaoThongBao:dd/MM/yyyy}"
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
            return new TamUngAnPhiViewModel
            {
                AnPhiID = dbModel.AnPhiID,
                HoSoVuAnID = dbModel.HoSoVuAnID,
                NopAnPhi = dbModel.NopAnPhi,
                YeuCauDuNopViewModel = yeuCauDuNopAnPhiModel,
                KetQuaNopViewModel = ketQuaNopAnPhiModel,
                NhomNghiepVu = dbModel.NhomNghiepVu,
                GiaiDoan = dbModel.GiaiDoan,
                CongDoanHoSo = dbModel.CongDoanHoSo,
                TrangThai = dbModel.TrangThai,
                NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd/MM/yyyy"),
                GhiChu = dbModel.GhiChu
            };
        }

        public Lib.TACM.SauXetXu.Model.TamUngAnPhi.ResponseResult SuaMienDuNopTamUngAnPhi(TamUngAnPhiViewModel viewModel)
        {
            Lib.TACM.SauXetXu.Model.TamUngAnPhi.ResponseResult result = null;
            try
            {
                var dbModel = new MienDuNopModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    NopAnPhi = viewModel.MienDuNopViewModel.NopAnPhi,
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = 6, // SauXetXu
                    NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                    GhiChu = viewModel.GhiChu
                };
                result = _sauXetXuDataAccess.SuaMienDuNopTamUngAnPhi(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaMienDuNopTamUngAnPhi with error [{ex.ToString()}]", AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public Lib.TACM.SauXetXu.Model.TamUngAnPhi.ResponseResult SuaYeuCauDuNopTamUngAnPhi(TamUngAnPhiViewModel viewModel)
        {
            Lib.TACM.SauXetXu.Model.TamUngAnPhi.ResponseResult result = null;
            try
            {
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.TongAnPhi, out decimal tongAnPhi);
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.GiaTriDuNop, out decimal giaTriDuNop);

                var dbModel = new YeuCauDuNopAnPhiModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    NopAnPhi = viewModel.YeuCauDuNopViewModel.NopAnPhi,
                    TongAnPhi = tongAnPhi,
                    GiaTriDuNop = giaTriDuNop,
                    HanNopAnPhi = Convert.ToDateTime(viewModel.YeuCauDuNopViewModel.HanNopAnPhi),
                    CoQuanThiHanhAnThu = viewModel.YeuCauDuNopViewModel.CoQuanThiHanhAnThu,
                    DiaChiCoQuanThiHanhAnThu = viewModel.YeuCauDuNopViewModel.DiaChiCoQuanThiHanhAnThu,
                    NgayRaThongBao = Convert.ToDateTime(viewModel.YeuCauDuNopViewModel.NgayRaThongBao),
                    NgayGiaoThongBao = Convert.ToDateTime(viewModel.YeuCauDuNopViewModel.NgayGiaoThongBao),
                    NhomNghiepVu = viewModel.NhomNghiepVu,
                    GiaiDoan = viewModel.GiaiDoan,
                    CongDoanHoSo = 6, // SauXetXu
                    NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                    GhiChu = viewModel.GhiChu
                };
                result = _sauXetXuDataAccess.SuaYeuCauDuNopTamUngAnPhi(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaYeuCauDuNopTamUngAnPhi with error [{ex.ToString()}]", AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public Lib.TACM.SauXetXu.Model.TamUngAnPhi.ResponseResult SuaKetQuaDuNopTamUngAnPhi(TamUngAnPhiViewModel viewModel)
        {
            Lib.TACM.SauXetXu.Model.TamUngAnPhi.ResponseResult result = null;
            try
            {
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.TongAnPhi, out decimal tongAnPhi);
                decimal.TryParse(viewModel.YeuCauDuNopViewModel.GiaTriDuNop, out decimal giaTriDuNop);

                var dbModel = new TamUngAnPhiModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    NopAnPhi = viewModel.YeuCauDuNopViewModel.NopAnPhi,
                    TongAnPhi = tongAnPhi,
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
                    CongDoanHoSo = 6, // SauXetXu
                    NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV,
                    GhiChu = viewModel.GhiChu
                };
                result = _sauXetXuDataAccess.SuaKetQuaDuNopTamUngAnPhi(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SuaKetQuaDuNopTamUngAnPhi with error [{ex.ToString()}]", AppName.BizSecurity);
                result = null;
            }
            return result;
        }
        #endregion

        #region ChuyenHoSo
        public ChuyenHoSoViewModel ChiTietChuyenHoSoTheoId(int chuyenDonId)
        {
            try
            {
                var dbModel = NhanDonDA.ChiTietChuyenDonTheoId(chuyenDonId);
                if (dbModel == null)
                    return null;
                return new ChuyenHoSoViewModel
                {
                    ChuyenHoSoID = dbModel.ChuyenDonID,
                    HoSoVuAnID = dbModel.HoSoVuAnID,
                    NgayRaThongBao = dbModel.NgayRaThongBao.ToString("dd'/'MM'/'yyyy"),
                    NgayChuyenHoSo = dbModel.NgayChuyenDon.ToString("dd'/'MM'/'yyyy"),
                    ToaAnChuyenDi = dbModel.ToaAnChuyenDi,
                    ToaAnChuyenDen = dbModel.ToaAnChuyenDen,
                    LyDoChuyenHoSo = dbModel.LyDoChuyenDon,
                    TruongHopChuyen = dbModel.TruongHopChuyen,
                    NhomNghiepVu = dbModel.NhomNghiepVu,
                    GiaiDoan = dbModel.GiaiDoan,
                    CongDoanHoSo = dbModel.CongDoanHoSo,
                    TrangThai = dbModel.TrangThai,
                    NguoiTao = _settingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu,
                    TrangThaiCongDoan = dbModel.TrangThaiCongDoan
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietChuyenHoSoTheoId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new ChuyenHoSoViewModel();
            }
        }

        public Lib.TACM.NhanDon.Model.ResponseResult SuaDoiChuyenHoSo(ChuyenHoSoViewModel viewModel)
        {
            Lib.TACM.NhanDon.Model.ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV;
                var dbModel = new ChuyenDonModel
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    NgayRaThongBao = DateTime.ParseExact(viewModel.NgayRaThongBao, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    NgayChuyenDon = DateTime.ParseExact(viewModel.NgayChuyenHoSo, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ToaAnChuyenDi = viewModel.ToaAnChuyenDi,
                    ToaAnChuyenDen = viewModel.ToaAnChuyenDen,
                    LyDoChuyenDon = viewModel.LyDoChuyenHoSo,
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
                WriteLog.Error(string.Format("SuaDoiChuyenHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public int KiemTraTinhTrangDinhKemFileQuyetDinh(int hoSoVuAnId, int giaiDoan)
        {
            int result = 0;
            try
            {
                result = _sauXetXuDataAccess.KiemTraDinhKemFileQuyetDinh(hoSoVuAnId, giaiDoan);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"KiemTraTinhTrangDinhKemFileQuyetDinh with error [{ex.ToString()}]", AppName.BizSecurity);
            }
            return result;
        }

        public int KiemTraTinhTrangDinhKemFileBanAn(int hoSoVuAnId, int giaiDoan)
        {
            int result = 0;
            try
            {
                result = _sauXetXuDataAccess.KiemTraDinhKemFileBanAn(hoSoVuAnId, giaiDoan);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"KiemTraTinhTrangDinhKemFileBanAn with error [{ex.ToString()}]", AppName.BizSecurity);
            }
            return result;
        }
        #endregion
    }
}