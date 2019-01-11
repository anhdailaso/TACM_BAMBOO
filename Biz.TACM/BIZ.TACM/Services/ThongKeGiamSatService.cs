using Biz.Lib.TACM.ThongKeGiamSat.IDataAccess;
using Biz.Lib.TACM.ThongKeGiamSat.DataAccess;
using Biz.TACM.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.ThongKeGiamSat.Models;
using Biz.TACM.Models.ViewModel.ThongKeGiamSat;

namespace Biz.TACM.Services
{
    public class ThongKeGiamSatService : IThongKeGiamSatService
    {
        private IThongKeGiamSatDataAccess _thongKeGiamSatDataAccess;
        private IThongKeGiamSatDataAccess ThongKeGiamSatDataAccess => _thongKeGiamSatDataAccess ?? (_thongKeGiamSatDataAccess = new ThongKeGiamSatDataAccess());

        private INhanDonService _nhandonService;
        private INhanDonService NhanDonService
        { get { return _nhandonService ?? (_nhandonService = new NhanDonService()); } }

        #region Giam Sat
        public IEnumerable<GiamSatHoSoVuAnViewModel> GetDanhSachHoSoVuAnGiamSat(string listHoSoVuAnID)
        {
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.GetDanhSachHoSoVuAnGiamSat(listHoSoVuAnID);

                return dbModel.Select(s => new GiamSatHoSoVuAnViewModel
                {
                    HoSoVuAnID = s.HoSoVuAnID,
                    MaHoSo = s.MaHoSo,
                    SoHoSo = s.SoHoSo,
                    DuongSu = s.DuongSu,
                    ToaAn = s.ToaAn,
                    NhomAn = s.NhomAn,
                    GiaiDoan = s.GiaiDoan,
                    CongDoanHoSo = s.CongDoanHoSo,
                }).ToList();
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThongKeTongHop with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public GiamSatViewModel LocDuLieuGiamSat(GiamSatLocDuLieuViewModel viewModel)
        {
            GiamSatViewModel giamSatViewModel = new GiamSatViewModel();
            List<GiamSatDuLieuBieuDoViewModel> result = new List<GiamSatDuLieuBieuDoViewModel>();
            string listHoSoVuAnID = null;
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.LocDuLieuGiamSat(ref listHoSoVuAnID, viewModel.TuNgay, viewModel.DenNgay, viewModel.Group, viewModel.ToaAnID, viewModel.NhomAn, viewModel.GiaiDoan);
                result = dbModel.Select(s => new GiamSatDuLieuBieuDoViewModel
                {
                    GroupName = s.TenNhom,
                    Amount = s.SoLuongHoSo
                }).ToList();
                giamSatViewModel.ListData = result;
                giamSatViewModel.ListHoSoVuAnID = listHoSoVuAnID;
                return giamSatViewModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("LocDuLieuGiamSat with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        #endregion

        #region Thong Ke
        public IEnumerable<ThongKeTongHopModel> ThongKeTongHop(int loaiThongKe, string tuNgay, string denNgay, int? toaAnID = null, string nhomAn = null, int? giaiDoan = null, string loaiQuanHe = null, string loaiDeNghi = null, string quanHePhapLuat = null, string toiDanh = null)
        {
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.ThongKeTongHop(loaiThongKe, tuNgay, denNgay, toaAnID, nhomAn, giaiDoan,loaiQuanHe,loaiDeNghi,quanHePhapLuat,toiDanh);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThongKeTongHop with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public DuLieuThongKeTrucTuyenModel ThongKeTrucTuyenPhanCongThamPhan(string tuNgay, string denNgay, int toaAnId)
        {
            var viewModel = new DuLieuThongKeTrucTuyenModel();
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.ThongKeTrucTuyenPhanCongThamPhan(tuNgay, denNgay, toaAnId);
                if (dbModel != null)
                {                    
                    foreach (var item in dbModel)
                    {
                        viewModel.TONG_HSSTTL += item.HSSTTL;
                        viewModel.TONG_HSSTGQ += item.HSSTGQ;
                        viewModel.TONG_HSPTTL += item.HSPTTL;
                        viewModel.TONG_HSPTGQ += item.HSPTGQ;

                        viewModel.TONG_DSSTTL += item.DSSTTL;
                        viewModel.TONG_DSSTGQ += item.DSSTGQ;
                        viewModel.TONG_DSPTTL += item.DSPTTL;
                        viewModel.TONG_DSPTGQ += item.DSPTGQ;

                        viewModel.TONG_HNSTTL += item.HNSTTL;
                        viewModel.TONG_HNSTGQ += item.HNSTGQ;
                        viewModel.TONG_HNPTTL += item.HNPTTL;
                        viewModel.TONG_HNPTGQ += item.HNPTGQ;

                        viewModel.TONG_KTSTTL += item.KTSTTL;
                        viewModel.TONG_KTSTGQ += item.KTSTGQ;
                        viewModel.TONG_KTPTTL += item.KTPTTL;
                        viewModel.TONG_KTPTGQ += item.KTPTGQ;

                        viewModel.TONG_HCSTTL += item.HCSTTL;
                        viewModel.TONG_HCSTGQ += item.HCSTGQ;
                        viewModel.TONG_HCPTTL += item.HCPTTL;
                        viewModel.TONG_HCPTGQ += item.HCPTGQ;

                        viewModel.TONG_LDSTTL += item.LDSTTL;
                        viewModel.TONG_LDSTGQ += item.LDSTGQ;
                        viewModel.TONG_LDPTTL += item.LDPTTL;
                        viewModel.TONG_LDPTGQ += item.LDPTGQ;

                        viewModel.TONG_ADTL += item.ADTL;
                        viewModel.TONG_ADGQ += item.ADGQ;

                        viewModel.TONG_TONGTL += item.TONGTL;
                        viewModel.TONG_TONGTLST += item.TONGTLST;
                        viewModel.TONG_TONGTLPT += item.TONGTLPT;

                        viewModel.TONG_TONGGQ += item.TONGGQ;
                        viewModel.TONG_TONGGQST += item.TONGGQST;
                        viewModel.TONG_TONGGQPT += item.TONGGQPT;

                        viewModel.TONG_TONGCL += item.TONGCL;
                    }
                }
                viewModel.ListData = dbModel;

                return viewModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThongKeTrucTuyenPhanCongThamPhan with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public IEnumerable<HoSoBaoCaoThongKeModel> DanhSachHoSoBaoCaoThongKe(string danhSachId)
        {
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.DanhSachHoSoBaoCaoThongKe(danhSachId);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachHoSoBaoCaoThongKe with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public DataSet GetDataSetHoSoBaoCaoThongKe(string danhSachId)
        {
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.GetDataSetHoSoBaoCaoThongKe(danhSachId);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetDataSetHoSoBaoCaoThongKe with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public IEnumerable<HoSoPhanCongThamPhanModel> DanhSachHoSoChuaPhanCongThamPhan(string tuNgay, string denNgay, int toaAnId)
        {
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.DanhSachHoSoChuaPhanCongThamPhan(tuNgay, denNgay, toaAnId);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachHoSoChuaPhanCongThamPhan with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public Stream ExportDanhSachHoSoThongKe(string danhSachId)
        {          
            try
            {
                danhSachId = danhSachId == string.Empty ? null : danhSachId;
                var ds = ThongKeGiamSatDataAccess.GetDataSetHoSoBaoCaoThongKe(danhSachId);
                var workbook = new ClosedXML.Excel.XLWorkbook();

                foreach (DataTable dt in ds.Tables)
                {
                    var worksheet = workbook.Worksheets.Add(dt.TableName);
                    worksheet.Cell(1, 1).InsertTable(dt);
                    worksheet.Columns().AdjustToContents();
                }

                var ms = new MemoryStream();
                workbook.SaveAs(ms);
                ms.Position = 0;
                return ms;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ExportDanhSachHoSoThongKe with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }        
        }

        public Stream ExportChiTienXetXu(string tuNgay, string denNgay, int? toaAnId)
        {
            try
            {
                tuNgay = tuNgay == string.Empty ? null : tuNgay;
                denNgay = denNgay == string.Empty ? null : denNgay;

                var ds = ThongKeGiamSatDataAccess.GetDataSetChiTienXetXu(tuNgay, denNgay, toaAnId);
                var workbook = new ClosedXML.Excel.XLWorkbook();

                foreach (DataTable dt in ds.Tables)
                {
                    var worksheet = workbook.Worksheets.Add(dt.TableName);
                    worksheet.Cell(1, 1).InsertTable(dt);
                    worksheet.Columns().AdjustToContents();
                }

                var ms = new MemoryStream();
                workbook.SaveAs(ms);
                ms.Position = 0;
                return ms;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ExportChiTienXetXu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        #endregion

        #region ThongKeLuuKho

        public ThongKeLuuKhoModel DuLieuBieuDoThongKeLuuKho(ref string listHoSoVuAnId, string tuNgay, string denNgay, int group, int? toaAnId = null, string nhomAn = null, int? giaiDoan = null)
        {
            ThongKeLuuKhoModel luuKhoModel = new ThongKeLuuKhoModel();
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.DuLieuBieuDoThongKeLuuKho(ref listHoSoVuAnId, tuNgay, denNgay, group, toaAnId, nhomAn, giaiDoan);
                luuKhoModel.ListData = dbModel.ToList();
                luuKhoModel.ListHoSoVuAnID = listHoSoVuAnId;

                return luuKhoModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DuLieuBieuDoThongKeLuuKho with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public IEnumerable<HoSoThongKeLuuKhoModel> DanhSachHoSoThongKeLuuKho(string listHoSoVuAnId)
        {
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.GetDanhSachHoSoThongKeLuuKho(listHoSoVuAnId);
                foreach(var item in dbModel)
                {
                    if (item.TenVuAn == null)
                    {
                        var duongsu = NhanDonService.NguyenDonVaBiDon(item.HoSoVuAnID);
                        List<string> tenvuan = new List<string>();
                        foreach (var obj in duongsu)
                        {
                            tenvuan.Add(obj.HoVaTen);
                        }
                        item.TenVuAn = String.Join(" - ", tenvuan);
                    }
                }
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachHoSoThongKeLuuKho with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
                
        public Stream ExportThongKeLuuKho(string listHoSoVuAnId)
        {
            try
            {
                var ds = ThongKeGiamSatDataAccess.GetDataSetThongKeLuuKho(listHoSoVuAnId);
                var workbook = new ClosedXML.Excel.XLWorkbook();

                foreach (DataTable dt in ds.Tables)
                {
                    var worksheet = workbook.Worksheets.Add(dt.TableName);
                    worksheet.Cell(1, 1).InsertTable(dt);
                    worksheet.Columns().AdjustToContents();
                }

                var ms = new MemoryStream();
                workbook.SaveAs(ms);
                ms.Position = 0;
                return ms;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ExportThongKeLuuKho with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        #endregion

        #region ThongKeAnHuySua

        public ThongKeAnHuySuaModel DuLieuBieuDoThongKeAnHuySua(ref string listHoSoHuyId, ref string listHoSoSuaId, string tuNgay, string denNgay, int? toaAnId, string thamPhan)
        {
            ThongKeAnHuySuaModel huySuaModel = new ThongKeAnHuySuaModel();
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.DuLieuBieuDoThongKeAnHuySua(ref listHoSoHuyId, ref listHoSoSuaId, tuNgay, denNgay, toaAnId, thamPhan);
                huySuaModel.ListData = dbModel.ToList();
                huySuaModel.ListHoSoHuyID = listHoSoHuyId;
                huySuaModel.ListHoSoSuaID = listHoSoSuaId;

                return huySuaModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DuLieuBieuDoThongKeAnHuySua with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public IEnumerable<HoSoThongKeHuySuaModel> DanhSachHoSoThongKeAnHuySua(string listHoSoHuyId, string listHoSoSuaId)
        {
            try
            {
                var dbModel = ThongKeGiamSatDataAccess.GetDanhSachHoSoThongKeAnHuySua(listHoSoHuyId, listHoSoSuaId);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachHoSoThongKeAnHuySua with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        #endregion
    }
}
        
        