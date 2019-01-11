using Biz.Lib.Authentication;
using Biz.TACM.Models.ViewModel.ChuanBiXetXu;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.NhanDon.Model;
using Biz.Lib.TACM.NhanDon.DataAccess;
using Biz.Lib.TACM.NhanDon.IDataAccess;
using Biz.Lib.TACM.ChuanBiXetXu.DataAccess;
using Biz.Lib.TACM.ChuanBiXetXu.IDataAccess;
using Biz.Lib.TACM.ChuanBiXetXu.Model;
using Biz.TACM.IServices;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.TACM.Services
{
    public class ChuanBiXetXuService : IChuanBiXetXuService
    {
        private IChuanBiXetXuDataAccess _chuanBiXetXuDA;

        private IChuanBiXetXuDataAccess ChuanBiXetXuDA
        {
            get { return _chuanBiXetXuDA ?? (_chuanBiXetXuDA = new ChuanBiXetXuDataAccess()); }
        }
        private INhanDonDataAccess _nhanDonDA;
        private INhanDonDataAccess NhanDonDA => _nhanDonDA ?? (_nhanDonDA = new NhanDonDataAccess());

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        #region HoaGiai
        public SelectList SelectListNhanVien(string nhomChucNang, int selected)
        {
            try
            {
                var dbModel = ChuanBiXetXuDA.DanhSachNhanVien(nhomChucNang);

                var danhSachNhanVien = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoVaTenNV,
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

        public IEnumerable<Lib.TACM.ChuanBiXetXu.Model.NhanVienModel> DanhSachThuKyTheoThamPhan(string maNV)
        {
            try
            {
                var danhSachNhanVien = ChuanBiXetXuDA.DanhSachThuKyTheoThamPhan(maNV);


                return danhSachNhanVien;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachThuKyTheoThamPhan with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SelectListThuKyTheoThamPhan(string maNV, string selected)
        {
            try
            {
                var dbModel = DanhSachThuKyTheoThamPhan(maNV);

                var danhSachNhanVien = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoVaTenNV + " (" + x.MaNV.ToString() + ")",
                        Value = x.MaNV.ToString()
                    }
                );

                return new SelectList(danhSachNhanVien, "Value", "Text", selected);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SelectListThuKyTheoThamPhan with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public SelectList DanhSachNgayHoaGiai(int hoSoVuAnID, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = ChuanBiXetXuDA.DanhSachNgayHoaGiai(hoSoVuAnID, giaiDoan);

                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.HoaGiaiID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgayHoaGiai with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public HoaGiaiModel ChiTietHoaGiaiTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            try
            {
                var dbModel = ChuanBiXetXuDA.ThongTinHoaGiaiTheoHoSoVuAnID(hoSoVuAnID, giaiDoan);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietHoaGiaiTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new HoaGiaiModel();
            }
        }

        public HoaGiaiModel ChiTietHoaGiaiTheoHoaGiaiID(int banAnID)
        {
            try
            {
                var dbModel = ChuanBiXetXuDA.ThongTinHoaGiaiTheoHoaGiaiID(banAnID);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietHoaGiaiTheoHoaGiaiID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new HoaGiaiModel();
            }
        }

        public Lib.TACM.ChuanBiXetXu.Model.ResponseResult ThemHoaGiai(HoaGiaiModel viewModel)
        {
            Lib.TACM.ChuanBiXetXu.Model.ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = ChuanBiXetXuDA.ThemHoaGia(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemHoaGiai with error [{0}]", ex.ToString()), AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }
        #endregion

        #region QuyetDinh

        // thong bao/ quyet dinh
        public SelectList SelectListQuyetDinhLoai(string selectedValue)
        {
            var list = XMLUtils.BindData("QuyetDinhLoai");
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        //public IEnumerable<DLLoaiQuyetDinhModel> DLLoaiQuyetDinh(int giaiDoan)
        //{
        //    try
        //    {
        //       var danhSach = ChuanBiXetXuDA.DLLoaiQuyetDinh(giaiDoan);


        //        return danhSach;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.Error(string.Format("DLLoaiQuyetDinh with error [{0}]", ex.ToString()), AppName.BizSecurity);
        //        return null;
        //    }
        //}

        //public IEnumerable<DLQuyetDinhModel> DLDanhSachQuyetDinh(int loaiQuyetDinhID)
        //{
        //    try
        //    {
        //        var danhSach = ChuanBiXetXuDA.DLDanhSachQuyetDinh(loaiQuyetDinhID);

        //        return danhSach;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.Error(string.Format("DLLoaiQuyetDinh with error [{0}]", ex.ToString()), AppName.BizSecurity);
        //        return null;
        //    }
        //}

        public IEnumerable<QuyetDinhModel> DanhSachQuyetDinh(int hoSoVuAnID, int giaiDoan)
        {
            try
            {
                var listQuyetDinh = ChuanBiXetXuDA.DanhSachQuyetDinh(hoSoVuAnID, giaiDoan);
                return listQuyetDinh;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachQuyetDinh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public QuyetDinhModel ChiTietQuyetDinhTheoQuyetDinhID(int quyetDinhID)
        {
            try
            {
                var dbModel = ChuanBiXetXuDA.ThongTinQuyetDinhTheoQuyetDinhID(quyetDinhID);
                if (dbModel != null)
                {
                    var nguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                    dbModel.NguoiTao = nguoiTao;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietQuyetDinhTheoQuyetDinhID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new QuyetDinhModel();
            }
        }

        public Lib.TACM.ChuanBiXetXu.Model.ResponseResult SuaQuyetDinh(QuyetDinhModel viewModel)
        {
            Lib.TACM.ChuanBiXetXu.Model.ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = ChuanBiXetXuDA.SuaQuyetDinh(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaQuyetDinh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public Lib.TACM.ChuanBiXetXu.Model.ResponseResult ThemQuyetDinh(QuyetDinhModel viewModel)
        {
            Lib.TACM.ChuanBiXetXu.Model.ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = ChuanBiXetXuDA.ThemQuyetDinh(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemQuyetDinh with error [{0}]", ex.ToString()), AppName.BizSecurity); 
                result = null;
            }
            return result;
        }

        public Lib.TACM.ChuanBiXetXu.Model.ResponseResult XoaQuyetDinh(int quyetDinhID)
        {
            Lib.TACM.ChuanBiXetXu.Model.ResponseResult result = null;
            try
            {
                result = ChuanBiXetXuDA.XoaQuyetDinh(quyetDinhID);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("XoaQuyetDinh with error [{0}]", ex.ToString()), AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }

        #endregion

        #region QuyetDinh HinhSu

        public SelectList DanhSachNgaySuaQuyetDinhHinhSu(int hoSoVuAnID, int giaiDoan, int quyetDinhGroup, int selected)
        {
            try
            {
                var dbModel = ChuanBiXetXuDA.DanhSachNgaySuaQuyetDinhHinhSu(hoSoVuAnID, giaiDoan, quyetDinhGroup);

                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format(Setting.DINHDANG_NGAY_DANHSACHNGAYSUADOI, x.NgayTao),
                        Value = x.QuyetDinhID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgaySuaQuyetDinhHinhSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public QuyetDinhHinhSuModel ChiTietQuyetDinhHinhSuTheoHoSoVuAnId(int hoSoVuAnId, int giaiDoan, int quyetDinhGroup)
        {
            try
            {
                var dbModel = ChuanBiXetXuDA.ThongTinQuyetDinhHinhSuTheoHoSoVuAnID(hoSoVuAnId, giaiDoan, quyetDinhGroup);

                var danhSachLyDo = new List<string>();
                if (dbModel != null)
                {
                    if (dbModel.LyDo != null)
                    {
                        var lyDo = dbModel.LyDo.Split('&');
                        foreach (string item in lyDo)
                        {
                            danhSachLyDo.Add(item);
                        }
                    }
                    dbModel.DanhSachLyDo = danhSachLyDo;
                    dbModel.NguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                }                
                
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietQuyetDinhHinhSuTheoHoSoVuAnId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new QuyetDinhHinhSuModel();
            }
        }

        public QuyetDinhHinhSuModel ChiTietQuyetDinhHinhSuTheoQuyetDinhId(int quyetDinhId)
        {
            try
            {
                var dbModel = ChuanBiXetXuDA.ThongTinQuyetDinhHinhSuTheoQuyetDinhID(quyetDinhId);
                if (dbModel != null)
                {
                    var danhSachLyDo = new List<string>();
                    if (dbModel.LyDo != null)
                    {
                        var lyDo = dbModel.LyDo.Split('&');
                        foreach (string item in lyDo)
                        {
                            danhSachLyDo.Add(item);
                        }
                    }
                    dbModel.DanhSachLyDo = danhSachLyDo;
                    dbModel.NguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietQuyetDinhHinhSuTheoQuyetDinhId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new QuyetDinhHinhSuModel();
            }
        }

        public Lib.TACM.ChuanBiXetXu.Model.ResponseResult ThemQuyetDinhHinhSu(QuyetDinhHinhSuModel viewModel, int quyetdinhGroup)
        {
            Lib.TACM.ChuanBiXetXu.Model.ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = ChuanBiXetXuDA.ThemQuyetDinhHinhSu(viewModel,quyetdinhGroup);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemQuyetDinhHinhSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public IEnumerable<LyDo> DanhSachLyDoSelected(string groupName, List<string> selected)
        {
            try
            {
                var listItem = SettingDataService.SettingDataItem(groupName, "");
                var listLyDo = listItem.Select(s => new LyDo()
                {
                    LyDoItem = s.Value
                }).ToList();

                var list = new List<LyDo>();
                foreach (var item in listLyDo)
                {
                    bool check = selected != null && selected.Contains(item.LyDoItem);
                    item.Checked = check;
                    list.Add(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachLyDoSelected with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
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
                    NguoiTao = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.NguoiTao).HoTenVaMaNV,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu,
                    TrangThaiCongDoan = dbModel.TrangThaiCongDoan
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietChuyenDonTheoId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new ChuyenHoSoViewModel();
            }
        }

        public Lib.TACM.NhanDon.Model.ResponseResult SuaDoiChuyenHoSo(ChuyenHoSoViewModel viewModel)
        {
            Lib.TACM.NhanDon.Model.ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
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
                WriteLog.Error(string.Format("SuaDoiChuyenDon with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }
        #endregion
    }
}