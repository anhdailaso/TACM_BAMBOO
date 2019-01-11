using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.KetQuaXetXu.DataAccess;
using Biz.Lib.TACM.KetQuaXetXu.IDataAccess;
using Biz.Lib.TACM.KetQuaXetXu.Model;
using Biz.TACM.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biz.TACM.Services
{
    public class KetQuaXetXuService : IKetQuaXetXuService
    {
        private IKetQuaXetXuDataAccess _ketQuaXetXuDA;

        private IKetQuaXetXuDataAccess KetQuaXetXuDA
        {
            get { return _ketQuaXetXuDA ?? (_ketQuaXetXuDA = new KetQuaXetXuDataAccess()); }
        }

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        #region QuyetDinh
        //public SelectList SelectListLoaiQuyetDinh(string selectedValue)
        //{
        //    try
        //    {
        //        var dbModel = KetQuaXetXuDA.DanhSachLoaiQuyetDinh();

        //        var list = dbModel.Select(
        //            x => new SelectListItem
        //            {
        //                Text = x.TenLoaiQuyetDinh,
        //                Value = x.DuLieuLoaiQuyetDinhID.ToString()
        //            }
        //        );

        //        return new SelectList(list, "Text", "Text", selectedValue);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.Error(string.Format("SelectListLoaiQuyetDinh with error [{0}]", ex.ToString()), AppName.BizSecurity);
        //        return null;
        //    }
        //}
        private QuyetDinhModel AddNhanVienToQuyetDinhModel(QuyetDinhModel dbModel)
        {
            dbModel.NhanVienThamPhan = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan);
            dbModel.NhanVienThamPhan1 = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan1);
            dbModel.NhanVienThamPhan2 = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan2);
            dbModel.NhanVienThamPhanKhac = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhanKhac);
            dbModel.NhanVienHoiThamNhanDan = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan);
            dbModel.NhanVienHoiThamNhanDan2 = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan2);
            dbModel.NhanVienHoiThamNhanDan3 = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan3);
            dbModel.NhanVienThuKy = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThuKy);
            dbModel.NhanVienKiemSatVien = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.KiemSatVien);

            var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
            dbModel.NguoiTao = nguoiTao;

            return dbModel;
        }
        public SelectList DanhSachNgayQuyetDinh(int hoSoVuAnID, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.DanhSachNgayQuyetDinh(hoSoVuAnID, giaiDoan);

                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.QuyetDinhID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgayXetXu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public QuyetDinhModel ChiTietQuyetDinhTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.ThongTinQuyetDinhTheoHoSoVuAnID(hoSoVuAnID, giaiDoan);
                if (dbModel != null)
                {
                    dbModel = AddNhanVienToQuyetDinhModel(dbModel);
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietQuyetDinhTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new QuyetDinhModel();
            }
        }

        public QuyetDinhModel ChiTietQuyetDinhTheoQuyetDinhID(int quyetDinhID)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.ThongTinQuyetDinhTheoQuyetDinhID(quyetDinhID);
                if (dbModel != null)
                {
                    dbModel = AddNhanVienToQuyetDinhModel(dbModel);
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietQuyetDinhTheoQuyetDinhID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new QuyetDinhModel();
            }
        }

        public QuyetDinhModel ChiTietQuyetDinhTheoSoQuyetDinh(string soQuyetDinh, int hoSoVuAnID, DateTime ngayRaQuyetDinh)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.ThongTinQuyetDinhTheoSoQuyetDinh(soQuyetDinh, hoSoVuAnID, ngayRaQuyetDinh);
                if (dbModel != null)
                {
                    dbModel = AddNhanVienToQuyetDinhModel(dbModel);
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietQuyetDinhTheoSoQuyetDinh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new QuyetDinhModel();
            }
        }

        public ResponseResult ThemQuyetDinh(QuyetDinhModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = KetQuaXetXuDA.ThemQuyetDinh(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemQuyetDinh with error [{0}]", ex.ToString()), AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }

        public int SoQuyetDinhMax(int hoSoVuAnId, DateTime? ngayRaQuyetDinh)
        {
            int t = 1;
            try
            {
                t += KetQuaXetXuDA.SoQuyetDinhMax(hoSoVuAnId, ngayRaQuyetDinh);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SoQuyetDinhMax with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
            }
            return t;
        }
        #endregion

        #region QuyetDinh NhomAn ADBPXLHC

        public QuyetDinhADBPXLHCModel ChiTietQuyetDinhADBPXLHCTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.ThongTinQuyetDinhADBPXLHCTheoHoSoVuAnID(hoSoVuAnID, giaiDoan);
                if (dbModel != null)
                {
                    dbModel.NhanVienThamPhan = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan);
                    dbModel.NhanVienThuKy = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThuKy);
                    dbModel.NhanVienKiemSatVien = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.KiemSatVien);
                    dbModel.NguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietQuyetDinhADBPXLHCTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new QuyetDinhADBPXLHCModel();
            }
        }

        public QuyetDinhADBPXLHCModel ChiTietQuyetDinhADBPXLHCTheoQuyetDinhID(int quyetDinhId)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.ThongTinQuyetDinhADBPXLHCTheoQuyetDinhID(quyetDinhId);
                if (dbModel != null)
                {
                    dbModel.NhanVienThamPhan = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan);
                    dbModel.NhanVienThuKy = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThuKy);
                    dbModel.NhanVienKiemSatVien = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.KiemSatVien);
                    dbModel.NguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietQuyetDinhADBPXLHCTheoQuyetDinhID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new QuyetDinhADBPXLHCModel();;
            }
        }

        public ResponseResult ThemQuyetDinhADBPXLHC(QuyetDinhADBPXLHCModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = KetQuaXetXuDA.ThemQuyetDinhADBPXLHC(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemQuyetDinhADBPXLHC with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        #endregion

        #region BanAn
        //public SelectList SelectListXetXu(string selectedValue)
        //{
        //    var list = XMLUtils.BindData("XetXu");
        //    return new SelectList(list, "Value", "Text", selectedValue);
        //}

        public SelectList DanhSachNgayBanAn(int hoSoVuAnID, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.DanhSachNgayBanAn(hoSoVuAnID, giaiDoan);

                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.KQXXBanAnID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgayBanAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public BanAnModel ChiTietBanAnTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.ThongTinBanAnTheoHoSoVuAnID(hoSoVuAnID, giaiDoan);
                
                if (dbModel != null)
                {
                    dbModel = AddNhanVienToBanAnModel(dbModel);
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietBanAnTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new BanAnModel();
            }
        }

        public BanAnModel ChiTietBanAnTheoBanAnID(int banAnID)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.ThongTinBanAnTheoBanAnID(banAnID);
                if (dbModel != null)
                {
                    dbModel = AddNhanVienToBanAnModel(dbModel);
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThongTinBanAnTheoBanAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new BanAnModel();
            }
        }

        public BanAnModel ChiTietBanAnTheoSoBanAn(string soBanAn, int hoSoVuAnID, DateTime ngayTuyenAn)
        {
            try
            {
                var dbModel = KetQuaXetXuDA.ThongTinBanAnTheoSoBanAn(soBanAn, hoSoVuAnID, ngayTuyenAn);
                if (dbModel != null)
                {
                    dbModel = AddNhanVienToBanAnModel(dbModel);
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietBanAnTheoSoBanAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new BanAnModel();
            }
        }

        private BanAnModel AddNhanVienToBanAnModel(BanAnModel dbModel)
        {
            dbModel.NhanVienThamPhan = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan);
            dbModel.NhanVienThamPhan1 = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan1);
            dbModel.NhanVienThamPhan2 = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan2);
            dbModel.NhanVienThamPhanKhac = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhanKhac);
            dbModel.NhanVienHoiThamNhanDan = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan);
            dbModel.NhanVienHoiThamNhanDan2 = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan2);
            dbModel.NhanVienHoiThamNhanDan3 = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan3);
            dbModel.NhanVienThuKy = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThuKy);
            dbModel.NhanVienKiemSatVien = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.KiemSatVien);

            var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
            dbModel.NguoiTao = nguoiTao;

            return dbModel;
        }

        public ResponseResult ThemBanAn(BanAnModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = KetQuaXetXuDA.ThemBanAn(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemBanAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public int SoBanAnMax(int hoSoVuAnId, DateTime? ngayTuyenAn)
        {
            int t = 1;
            try
            {
                t += KetQuaXetXuDA.SoBanAnMax(hoSoVuAnId, ngayTuyenAn);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SoBanAnMax with error [{ex.ToString()}]", AppName.BizSecurity); //AppName.TACMThuLy
            }
            return t;
        }

        public ResponseResult CapNhatHuyBanAnSoTham(int hoSoVuAnID)
        {
            ResponseResult result = null;
            try
            {
                result = KetQuaXetXuDA.CapNhatHuyBanAnSoTham(hoSoVuAnID);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("CapNhatHuyBanAnSoTham with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        #region Toi Danh
        public IEnumerable<ToiDanhModel> DanhSachKetQuaXetXu(int hoSoVuAnId)
        {
            var listDbModel = KetQuaXetXuDA.DanhSachKetQuaXetXu(hoSoVuAnId);
            return listDbModel;
        }
        public ToiDanhModel ChiTietToiDanhTheoID(int toiDanhID)
        {
            var result = KetQuaXetXuDA.ChiTietToiDanhTheoID(toiDanhID);
            return result;
        }
        public ResponseResult TaoToiDanh(ToiDanhModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                
                result = KetQuaXetXuDA.TaoToiDanh(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("TaoDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw ex;
            }
            return result;
        }
        public ResponseResult SuaToiDanh(ToiDanhModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;

                result = KetQuaXetXuDA.SuaToiDanh(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaToiDanh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw ex;
            }
            return result;
        }
        public ResponseResult XoaToiDanh(int toiDanhId)
        {
            ResponseResult result = null;
            try
            {
                var nguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;

                result = KetQuaXetXuDA.XoaToiDanh(toiDanhId, nguoiTao);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("XoaToiDanh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw ex;
            }
            return result;
        }
        #endregion

        #endregion
    }
}