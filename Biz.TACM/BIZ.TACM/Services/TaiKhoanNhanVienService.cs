using Biz.Lib.TACM.QuanLyNhanVien.DataAccess;
using Biz.Lib.TACM.QuanLyNhanVien.IDataAccess;
using Biz.Lib.TACM.QuanLyNhanVien.Model;
using Biz.TACM.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.TACM.Models.ViewModel.TaiKhoanNhanVien;
using Biz.Lib.TACM.Resources.Resources;
using System.Web.Mvc;

namespace Biz.TACM.Services
{
    public class TaiKhoanNhanVienService : ITaiKhoanNhanVienService
    {
        private ITaiKhoanNhanVienDataAccess _taiKhoanNhanVienDataAccess;
        private ITaiKhoanNhanVienDataAccess TaiKhoanNhanVienDataAccess => _taiKhoanNhanVienDataAccess ?? (_taiKhoanNhanVienDataAccess = new TaiKhoanNhanVienDataAccess());
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        public NhanVienModel ThongTinNhanVien(string maNV)
        {
            var dbModel = TaiKhoanNhanVienDataAccess.ThongTinNhanVien(maNV);
            dbModel.TenToaAn = SettingDataService.GetToaAnTheoToaAnID(dbModel.ToaAnID).TenToaAn;

            return dbModel;
        }
        public ChucNangViewModel DanhSachChucNangTheoMaNhanVien(string maNV)
        {
            var nhanVienID = SettingDataService.ChiTietNhanVienTheoMaNhanVien(maNV).NhanVienID;
            var danhSachChucNangNV = TaiKhoanNhanVienDataAccess.DanhSachChucNangTheoMaNhanVien(maNV);
            var danhSachTatCaChucNang = SettingDataService.SettingDataItemHaveValueText("DS_SOTHAM_CHUCNANG", "");

            ChucNangViewModel danhSach = new ChucNangViewModel();
            danhSach.ListChucNang = new List<ChucNang>();

            int SoChucNang = danhSachTatCaChucNang.Count();
            for (int i = 0; i < SoChucNang; i++)
            {
                ChucNang temp = new ChucNang();
                temp.NhanVienID = nhanVienID;
                temp.MaChucNang = danhSachTatCaChucNang.ElementAt(i).Value;
                temp.TenChucNang = danhSachTatCaChucNang.ElementAt(i).Text;
                var chucNang = danhSachChucNangNV.SingleOrDefault(m => m.MaChucNang.Equals(danhSachTatCaChucNang.ElementAt(i).Value));
                if (chucNang != null)
                {
                    temp.IsHaveChucNang = true;
                    if (danhSach.ChucNangChinh == null)
                        danhSach.ChucNangChinh = chucNang.ChucNangChinh == 1 ? chucNang.MaChucNang : null;
                }
                else
                {
                    temp.IsHaveChucNang = false;
                }
                danhSach.ListChucNang.Add(temp);
            }

            return danhSach;
        }
        public ResponseResult CapNhatThongTinNhanVien(NhanVienModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = TaiKhoanNhanVienDataAccess.CapNhatThongTinNhanVien(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("CapNhatThongTinNhanVien with error [{0}]", ex.ToString()), AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }

        public ResponseResult CapNhatHinhDaiDien(string maNV, HttpPostedFileBase file)
        {
            ResponseResult result = null;
            try
            {
                var nhanVien = TaiKhoanNhanVienDataAccess.ThongTinNhanVien(maNV);
                var currentPicturePath = HttpContext.Current.Server.MapPath(string.Format(Setting.EMPLOYEE_IMAGE_FOLDER, nhanVien.DuongDanHinhDaiDien));

                if (File.Exists(currentPicturePath))
                    File.Delete(currentPicturePath);

                var fileName = nhanVien.MaNVMoi+ DateTime.Now.Ticks.ToString()+ Path.GetExtension(file.FileName);
                var filePath = HttpContext.Current.Server.MapPath(string.Format(Setting.EMPLOYEE_IMAGE_FOLDER, fileName));
                file.SaveAs(filePath);

                nhanVien.DuongDanHinhDaiDien = fileName;
                result = TaiKhoanNhanVienDataAccess.CapNhatDuongDanHinhDaiDienNhanVien(nhanVien);
                result.ResponseMessage = fileName;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("CapNhatHinhDaiDien with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }
    }
}