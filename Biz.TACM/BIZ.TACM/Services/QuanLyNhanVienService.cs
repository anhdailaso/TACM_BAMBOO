using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.QuanLyNhanVien.DataAccess;
using Biz.Lib.TACM.QuanLyNhanVien.IDataAccess;
using Biz.Lib.TACM.QuanLyNhanVien.Model;
using Biz.Lib.TACM.SoDoToChuc.DataAccess;
using Biz.Lib.TACM.SoDoToChuc.IDataAccess;
using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.SoDoToChuc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Excel;
using Biz.TACM.Models.ViewModel.QuanLyNhanVien;
using System.Threading.Tasks;
using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.Models.ViewModel.TaiKhoanNhanVien;

namespace Biz.TACM.Services
{
    public class QuanLyNhanVienService : IQuanLyNhanVienService
    {
        private IQuanLyNhanVienDataAccess _quanlynhanvienDA;

        private IQuanLyNhanVienDataAccess QuanLyNhanVienDA
        {
            get { return _quanlynhanvienDA ?? (_quanlynhanvienDA = new QuanLyNhanVienDataAccess()); }
        }
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        private ISoDoToChucDataAccess _sodotochucDataAccess;
        private ISoDoToChucDataAccess SoDoToChucDataAccess => _sodotochucDataAccess ?? (_sodotochucDataAccess = new SoDoToChucDataAccess());

        //public IEnumerable<NhanVienModel> DanhsachNhanVien()
        //{
        //    try
        //    {
        //        var toaan = SettingDataService.GetToaAnTheoMaNhanVien(AccountUtils.CurrentUsername());
        //        int id = toaan.ToaAnID;
        //        var dbModel = QuanLyNhanVienDA.DanhsachNhanVienTheoToaAn(id);
        //        return dbModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.Error(string.Format("DanhsachNhanVien with error [{0}]", ex.ToString()), AppName.BizSecurity);
        //        return null;
        //    }
        //}
        public IEnumerable<NhanVienModel> DanhsachNhanVienTheoToaAn(int ToaAnID)
        {
            try
            {
                var dbModel = QuanLyNhanVienDA.DanhsachNhanVienTheoToaAn(ToaAnID).OrderBy(x=>x.HoVaTen).ToList();
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhsachNhanVienTheoToaAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public NhanVienViewModel ChitietNhanVien(int NhanVienid)
        {
            try
            {
                var dbModel = QuanLyNhanVienDA.ChitietNhanVien(NhanVienid);
                return new NhanVienViewModel
                {
                    NhanvienID = dbModel.NhanvienID,
                    ToaAnID = dbModel.ToaAnID,
                    MaNV = dbModel.MaNV,
                    MaNVMoi = dbModel.MaNVMoi,
                    HoNV = dbModel.HoNV,
                    TenLotNV = dbModel.TenLotNV,
                    TenNV = dbModel.TenNV,
                    GioiTinh = dbModel.GioiTinh,
                    NgaySinh = dbModel.NgaySinh !=null ? dbModel.NgaySinh.Value.ToString("dd/MM/yyyy") : null,
                    SoDienThoai = dbModel.SoDienThoai,
                    Email = dbModel.Email,
                    DuongDanHinhDaiDien = dbModel.DuongDanHinhDaiDien,
                    SoDoToChucID = dbModel.SoDoToChucID,
                    SoDoToChucChucVuID = dbModel.SoDoToChucChucVuID,
                    ChucDanh = dbModel.ChucDanh,
                    ChucVu=dbModel.ChucVu,
                    TrangThai = dbModel.TrangThai,
                    NguoiTao = dbModel.NguoiTao,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChitietNhanVienTheoId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new NhanVienViewModel();
            }
        }
        public IEnumerable<ChucNangModel> ChitietNhanVienChucNang(int NhanVienid)
        {
            try
            {
                var dbModel = QuanLyNhanVienDA.ChiTietChucNang(NhanVienid);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietChucNangNhanVien with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public ResponseResult ThemNhanVien(NhanVienViewModel model)
        {
            ResponseResult result = null;
            try
            {
                model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                var dbModel = new NhanVienModel
                {
                    //ListChucNang = listcn,
                    ToaAnID = model.ToaAnID,
                    MaNV = model.MaNV,
                    HoNV = model.HoNV,
                    TenLotNV = model.TenLotNV,
                    TenNV = model.TenNV,
                    GioiTinh = model.GioiTinh,
                    NgaySinh = model.NgaySinh !=null ? Convert.ToDateTime(model.NgaySinh) : (DateTime?)null,
                    SoDienThoai = model.SoDienThoai,
                    Email = model.Email,
                    DuongDanHinhDaiDien = model.DuongDanHinhDaiDien,
                    SoDoToChucID = model.SoDoToChucID,
                    SoDoToChucChucVuID = model.SoDoToChucChucVuID,
                    TaoTaiKhoan = model.TaoTaiKhoan,
                    TrangThai = model.TrangThai,
                    NguoiTao = model.NguoiTao,
                    GhiChu = model.GhiChu
                };
                result = QuanLyNhanVienDA.ThemNhanVien(dbModel);
                if (result == null) return null;
                if(model.TaoTaiKhoan)
                Task.Run(() => SendMailToNewNhanVien(dbModel, dbModel.ToaAnID, true));
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemNhanVien with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }
        public ResponseResult SuaNhanVien(NhanVienViewModel model)
        {
            ResponseResult result = null;
            try
            {
                var nv = QuanLyNhanVienDA.ChitietNhanVien(model.NhanvienID);
                model.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                var dbModel = new NhanVienModel
                {
                    NhanvienID = model.NhanvienID,
                    MaNV = model.MaNV,
                    MaNVMoi = model.MaNVMoi,
                    HoNV = model.HoNV,
                    TenNV = model.TenNV,
                    TenLotNV = model.TenLotNV,
                    GioiTinh = model.GioiTinh,
                    NgaySinh = model.NgaySinh != null ? Convert.ToDateTime(model.NgaySinh) : (DateTime?)null,
                    SoDienThoai = model.SoDienThoai,
                    Email = model.Email,
                    SoDoToChucID = model.SoDoToChucID,
                    SoDoToChucChucVuID = model.SoDoToChucChucVuID,
                    TrangThai = model.TrangThai,
                    NguoiTao = model.NguoiTao,
                    GhiChu = model.GhiChu
                };
                result = QuanLyNhanVienDA.SuaNhanVien(dbModel);
                result.ResponseMessage = nv.TenNV;
                if (result != null && result.ResponseCode == 1 && nv.MaNVMoi != model.MaNVMoi )
                {
                    var tenToaAn = SettingDataService.GetToaAnTheoToaAnID(nv.ToaAnID).TenToaAn;
                    var mailBody = string.Format(EmailTemplate.SUAMANHANVIEN_EMAIL_BODY,
                            model.HoVaTen,
                            tenToaAn,
                            nv.MaNVMoi,
                            model.MaNVMoi
                            );
                    Commons.SendMail(string.Format(EmailTemplate.SUAMANHANVIEN_EMAIL_SUBJECT), model.Email, mailBody);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("... with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }
        public ResponseResult XoaNhanVien(int NhanVienid)
        {
            ResponseResult result = null;
            try
            {
                result = QuanLyNhanVienDA.XoaNhanVien(NhanVienid);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("XoaNhanVien with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }
        public ResponseResult Capnhathinhdaidien(int NhanVienid, String image_url)
        {
            ResponseResult result = null;
            try
            {
                result = QuanLyNhanVienDA.Capnhathinhdaidien(NhanVienid, image_url);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("Capnhathinhdaidien with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public IEnumerable<SelectListChucDanhModel> DanhSachChucDanh(int toaAnid)
        {
            try
            {
                var dbModel = QuanLyNhanVienDA.DanhSachChucDanh(toaAnid);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhsachChucDanh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public IEnumerable<NhanVienToaAnModel> DanhSachNhanVienToaAn(int NhanVienID)
        {
            try
            {
                var dbModel = QuanLyNhanVienDA.DanhSachNhanVienToaAn(NhanVienID);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhsachNhanVienToaAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public ResponseResult ThemNhanVienToaAn(int NhanVienID, int ToaAnID)
        {
            ResponseResult result = null;
            try
            {
                result = QuanLyNhanVienDA.ThemNhanVienToaAn(NhanVienID, ToaAnID, SettingDataService.GetNhanVienSessionInfo().MaNV);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemNhanVienToaAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }
        public ResponseResult XoaNhanVienToaAn(int NhanVienToaAnID, int NhanVienID, int ToaAnID)
        {
            ResponseResult result = null;
            try
            {
                result = QuanLyNhanVienDA.XoaNhanVienToaAn(NhanVienToaAnID, NhanVienID, ToaAnID, SettingDataService.GetNhanVienSessionInfo().MaNV);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("XoaNhanVienToaAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }
        public ResponseResult SuaToaAnTrucThuoc(int SuaToaAnTrucThuoc, int toaAnID)
        {
            ResponseResult result = null;
            try
            {
                result = QuanLyNhanVienDA.SuaToaAnTrucThuoc(SuaToaAnTrucThuoc, toaAnID);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaToaAnTrucThuoc with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }
        #region import nhan vien
        public string ValidateExcel(HttpPostedFileBase dataSource, int toaAnID)
        {
            var message = string.Empty;
            dataSource.InputStream.Position = 0;
            var memoryStream = new MemoryStream();
            dataSource.InputStream.CopyTo(memoryStream);
            using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(memoryStream))
            {
                var validationFormatMessage = ValidateFormatExcel(excelReader);
                if (!string.IsNullOrEmpty(validationFormatMessage))
                    return validationFormatMessage;
                var validationDataMessage = ValidateDataExcel(excelReader);
                if (!string.IsNullOrEmpty(validationDataMessage))
                    return validationDataMessage;
                var validationDuplicatedMessage = ValidateDuplicated(excelReader,toaAnID);
                if (!string.IsNullOrEmpty(validationDuplicatedMessage))
                    return validationDuplicatedMessage;
            }
            SaveImportNhanVienFile(dataSource);
            return message;
        }

        public int ImportNhanVien(HttpPostedFileBase file, int ToaAnID)
        {
            IEnumerable<NhanVienModel> importednhanvien;
            file.InputStream.Position = 0;
            var memoryStream = new MemoryStream();
            file.InputStream.CopyTo(memoryStream);
            using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(memoryStream))
            {
                excelReader.IsFirstRowAsColumnNames = true;
                var listChucDanh = QuanLyNhanVienDA.DanhSachChucDanh(ToaAnID);
                var nhanvien =
                    Commons.ConvertDataTable<NhanVienExcelModel>(excelReader.AsDataSet().Tables["NhanViens"]);
                foreach(var row in nhanvien)
                {
                    row.CHUCDANH = listChucDanh.Where(x => x.ChucDanh.ToLower() == row.CHUCDANH.ToLower()).First().SoDoToChucID.ToString();
                    if(row.CHUCVU!=null && row.CHUCVU != "")
                        row.CHUCVU = listChucDanh.Where(x => x.ChucDanh.ToLower() == row.CHUCVU.ToLower()).First().SoDoToChucID.ToString();
                }
                excelReader.Dispose();
                var nhanvienDataXml = Commons.ConvertToXml(nhanvien, "NhanViens", "NhanVien");
                importednhanvien = QuanLyNhanVienDA.ImportNhanVien(SettingDataService.GetNhanVienSessionInfo().MaNV, nhanvienDataXml, ToaAnID).ToList();
            }

            if (importednhanvien.Any())
            {
                foreach (var nv in importednhanvien)
                {
                    if(nv.TaoTaiKhoan)
                        Task.Run(() => SendMailToNewNhanVien(nv, ToaAnID, true));
                }
                return ResponseCode.Success.GetHashCode();
            }

            return ResponseCode.Error.GetHashCode();
        }

        private string ValidateFormatExcel(IExcelDataReader excelReader)
        {
            excelReader.IsFirstRowAsColumnNames = true;
            var nhanvien = excelReader.AsDataSet().Tables["NhanViens"];

            if (nhanvien == null || nhanvien.Columns.Count != typeof(NhanVienExcelModel).GetProperties().Length)
            {
                return string.Format(NotifyMessage.WARNING_IMPORT_FILE_NOT_VALID, "NhanViens");
            }

            return null;
        }

        private string ValidateDataExcel(IExcelDataReader excelReader)
        {
            excelReader.IsFirstRowAsColumnNames = true;
            var nhanvien = excelReader.AsDataSet().Tables["NhanViens"];
            var copiedNhanVien = nhanvien.Rows.Cast<DataRow>().Where(w => !w.ItemArray.All(a => a is DBNull)).CopyToDataTable();
            foreach (var row in copiedNhanVien.Rows)
            {
                var exception = CheckExcelItem<NhanVienExcelModel>((DataRow)row);
                if (!string.IsNullOrEmpty(exception))
                    return string.Format(NotifyMessage.WARNING_IMPORT_SHEET, "NhanViens", exception);
            }
            return null;
        }

        private string ValidateDuplicated(IExcelDataReader excelReader,int toaAnID)
        {
            excelReader.IsFirstRowAsColumnNames = true;
            var nhanvien =
                Commons.ConvertDataTable<NhanVienExcelModel>(excelReader.AsDataSet().Tables["NhanViens"]);
            excelReader.Dispose();
            var listChucDanh = QuanLyNhanVienDA.DanhSachChucDanh(toaAnID);
            foreach(var row in nhanvien)
            {
                string a = null;
                a = GetManVTheoEmail(row.EMAIL);
                if(a!=null)
                    return string.Format(NotifyMessage.EMAIL_KHONGBITRUNG, row.EMAIL);
                ResponseResult rs = null;
                rs = QuanLyNhanVienDA.CheckMaNV(row.MANV);
                if (rs.ResponseMessage == "has" && rs.ResponseCode == 1)
                    return string.Format(NotifyMessage.BITRUNG, ViewText.LABEL_MANV, row.MANV);
                var cd = listChucDanh.Where(x => x.ChucDanh.ToLower()==row.CHUCDANH.ToLower()).FirstOrDefault();
                if (cd == null)
                    return string.Format(NotifyMessage.CHUCDANH_KHONGHOPLE, row.CHUCDANH);
                if(row.CHUCVU!=null && row.CHUCVU!="")
                {
                    var cv = listChucDanh.Where(x => x.ChucDanh.ToLower() == row.CHUCVU.ToLower()).FirstOrDefault();
                    if (cv == null)
                        return string.Format(NotifyMessage.CHUCVU_KHONGHOPLE, row.CHUCVU);
                }              
            }
            return null;
        }
        
        private string CheckExcelItem<T>(DataRow dr)
        {

            var temp = typeof(T);
            var obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                if (column.ColumnName!="TENLOTNV" && column.ColumnName != "NGAYSINH" && column.ColumnName != "CHUCVU" && column.ColumnName != "SODIENTHOAI")
                {
                    try
                    {
                        var pro = temp.GetProperties().FirstOrDefault(f => f.Name.ToLower() == column.ColumnName.ToLower());
                        if (pro != null)
                        {
                            if (dr[column.ColumnName] == DBNull.Value)
                            {
                                return string.Format(NotifyMessage.DULIEU_KHONGHOPLE, column.ColumnName);
                            }

                            pro.SetValue(obj,
                                Convert.ChangeType(dr[column.ColumnName], Type.GetType(pro.PropertyType.ToString())));

                        }
                        else
                        {
                            return string.Format(NotifyMessage.WARNING_DOES_NOT_EXIST, column.ColumnName);
                        }
                    }
                    catch
                    {
                        return string.Format(NotifyMessage.DULIEU_KHONGHOPLE, column.ColumnName);
                    }
                }
            }
            return null;
        }
        public string GetManVTheoEmail(string email)
        {
            return QuanLyNhanVienDA.GetMaNVtheoEmail(email);

        }
        private void SendMailToNewNhanVien(NhanVienModel model, int ToaAnID, bool isAsync = false)
        {
            try
            {
                string manv = GetManVTheoEmail(model.Email);
                var tenToaAn = SettingDataService.GetToaAnTheoToaAnID(ToaAnID).TenToaAn;
                var mailBody = string.Format(EmailTemplate.NEW_NHANVIEN_EMAIL_BODY,
                        model.HoVaTen,
                        tenToaAn,
                        manv,
                        EmailTemplate.NHANVIEN_DEFAULT_PASSWORD);
                Commons.SendMail(string.Format(EmailTemplate.NEW_NHANVIEN_EMAIL_SUBJECT), model.Email, mailBody);
            }
            catch (Exception ex)
            {
                if (!isAsync)
                    throw;
                WriteLog.Error(string.Format("SendMailToNewNhanVien with error [{0}]", ex.ToString()), AppName.BizSecurity);
            }

        }
        private void SaveImportNhanVienFile(HttpPostedFileBase dataSource)
        {
            var fileName =
                string.Format(Setting.NHANVIEN_IMPORT_NHANVIEN_FILE_NAME, DateTime.Now.ToString("hhmmssfff"));

            var fileFolder =
                HttpContext.Current.Server.MapPath(
                    string.Format(
                        Setting.NHANVIEN_IMPORT_NHANVIEN_FILE_FOLDER,
                        DateTime.Now.Year,
                        DateTime.Now.Month,
                        DateTime.Now.Day));

            var filePath = fileFolder + "/" + fileName;

            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }

            dataSource.SaveAs(filePath);
        }
        #endregion

        #region Searching Nhan Vien by Keyword
        public IEnumerable<NhanVienModel> DanhSachNhanVienSearchByKeyword(string keyword, int? toaAnID)
        {
            var danhSachNhanVien = QuanLyNhanVienDA.DanhSachNhanVienSearchByKeyword(keyword, toaAnID);
            return danhSachNhanVien;
        }
        #endregion

        #region So Do To Chuc Quan Ly Nhan Vien
        public IEnumerable<ChucDanhViewModel> SoDoToChucQuanLyNhanVien(int? toaAnId)
        {
            var dbModel = SoDoToChucDataAccess.DanhSachChucDanhTheoToaAn(toaAnId).Where(x=>x.Loai!=2);
            List<ChucDanhViewModel> danhSach = new List<ChucDanhViewModel>();
            foreach (var chucDanh in dbModel)
            {
                ChucDanhViewModel temp = new ChucDanhViewModel();
                temp.SoDoToChucID = chucDanh.SoDoToChucID;
                temp.ChucDanh = chucDanh.ChucDanh;
                temp.ChucDanhChaID = chucDanh.ChucDanhChaID;
                temp.SoLuongNhanVienTheoChucDanh = QuanLyNhanVienDA.DanhSachNhanVienTheoChucDanh(toaAnId, chucDanh.ChucDanh).ToList().Count;
                danhSach.Add(temp);
            }
            return PreOrderTreeSoDoToChuc(danhSach);
        }
        public IEnumerable<ChucDanhViewModel> SoDoToChucQuanLyNhanVienChucVu(int? toaAnId)
        {
            var dbModel = SoDoToChucDataAccess.DanhSachChucDanhTheoToaAn(toaAnId).Where(x => x.Loai == 2);
            List<ChucDanhViewModel> danhSach = new List<ChucDanhViewModel>();
            foreach (var chucDanh in dbModel)
            {
                ChucDanhViewModel temp = new ChucDanhViewModel();
                temp.SoDoToChucID = chucDanh.SoDoToChucID;
                temp.ChucDanh = chucDanh.ChucDanh;
                temp.ChucDanhChaID = chucDanh.ChucDanhChaID;
                temp.SoLuongNhanVienTheoChucDanh = QuanLyNhanVienDA.DanhSachNhanVienTheoChucVu(toaAnId, chucDanh.ChucDanh).ToList().Count;
                danhSach.Add(temp);
            }
            return PreOrderTreeSoDoToChuc(danhSach);
        }
        public List<ChucDanhViewModel> PreOrderTreeSoDoToChuc(List<ChucDanhViewModel> danhSach)
        {
            List<ChucDanhViewModel> temp = new List<ChucDanhViewModel>();
            List<ChucDanhViewModel> leftOver = danhSach.ToList();

            for (int i = 0; i < danhSach.Count; i++)
            {
                if (danhSach.ElementAt(i).ChucDanhChaID == 0 || danhSach.ElementAt(i).ChucDanhChaID == null)
                {
                    temp.Add(danhSach.ElementAt(i));
                    leftOver.Remove(danhSach.ElementAt(i));
                    FindChildrent(temp, leftOver, danhSach.ElementAt(i).SoDoToChucID);
                }
            }
            return temp;
        }
        public void FindChildrent(List<ChucDanhViewModel> temp, List<ChucDanhViewModel> danhSach, int id)
        {
            List<ChucDanhViewModel> leftOver = danhSach.ToList();
            for (int j = 0; j < danhSach.Count; j++)
            {
                if (danhSach.ElementAt(j).ChucDanhChaID == id)
                {
                    temp.Add(danhSach.ElementAt(j));
                    leftOver.Remove(danhSach.ElementAt(j));
                    FindChildrent(temp, leftOver, danhSach.ElementAt(j).SoDoToChucID);
                }
            }
        }

        public IEnumerable<NhanVienModel> DanhSachNhanVienTheoChucDanh(int? toaAnId, string chucDanh)
        {
            var danhSach = QuanLyNhanVienDA.DanhSachNhanVienTheoChucDanh(toaAnId, chucDanh);
            return danhSach;
        }
        public IEnumerable<NhanVienModel> DanhSachNhanVienTheoChucVu(int? toaAnId, string chucDanh)
        {
            var danhSach = QuanLyNhanVienDA.DanhSachNhanVienTheoChucVu(toaAnId, chucDanh);
            return danhSach;
        }
        #endregion

        #region Cap Nhat Chuc Nang Nhan Vien
        public ResponseResult CapNhatChucNangNhanVien(ChucNangViewModel danhSachChucNang)
        {
            ResponseResult result = null;
            try
            {
                var nguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                List<ChucNangModel> listDbModel = new List<ChucNangModel>();
                foreach (var model in danhSachChucNang.ListChucNang)
                {
                    if (model.IsHaveChucNang)
                    {
                        listDbModel.Add(new ChucNangModel
                        {
                            MaChucNang = model.MaChucNang,
                            TenChucNang = model.TenChucNang,
                            NhanVienID = model.NhanVienID,
                            ChucNangChinh = danhSachChucNang.ChucNangChinh == model.MaChucNang ? 1 : 0,
                            GhiChu = model.GhiChu,
                            NguoiTao = nguoiTao
                        });
                    }
                }

                result = QuanLyNhanVienDA.CapNhatChucNangNhanVien(listDbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("CapNhatChucNang with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }
        #endregion

        #region Cap Nhat Thu Ky Cho Tham Pahan
        public ResponseResult CapNhatThuKyThamPhan(NhanVienViewModel danhsachthuky)
        {
            ResponseResult result = null;
            try
            {
                
                List<NhanVienModel> listDbModel = new List<NhanVienModel>();
                foreach (var model in danhsachthuky.ListNhanVienThuKy)
                {
                    if (model.isckeck)
                    {
                        listDbModel.Add(new NhanVienModel
                        {
                            MaNV = danhsachthuky.MaNV,
                            MaNVThuKy = model.MaNV,
                        });
                    }
                }
                if (listDbModel.Count() == 0)
                {
                    listDbModel.Add(new NhanVienModel
                    {
                        MaNV = danhsachthuky.MaNV,
                        MaNVThuKy = null,
                    });
                }

                result = QuanLyNhanVienDA.CapNhatThuKyThamPhan(listDbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("CapNhatChucNang with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }
        #endregion
        public IEnumerable<NhanVienModel> DanhsachThuKyThamPhan(string MaNV)
        {
            try
            {
                var dbModel = QuanLyNhanVienDA.DanhsachThuKyThamPhan(MaNV);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhsachThuKyThamPhan with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
    }
}