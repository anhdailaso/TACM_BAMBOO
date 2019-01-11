using Biz.Lib.TACM.QuanLyNhanVien.Model;
using Biz.TACM.Models.ViewModel.TaiKhoanNhanVien;
using System;
using System.Collections.Generic;
using System.Web;

namespace Biz.TACM.IServices
{
    public interface ITaiKhoanNhanVienService
    {
        NhanVienModel ThongTinNhanVien(string maNV);
        ChucNangViewModel DanhSachChucNangTheoMaNhanVien(string maNV);
        ResponseResult CapNhatThongTinNhanVien(NhanVienModel viewModel);
        ResponseResult CapNhatHinhDaiDien(string maNV, HttpPostedFileBase file);
    }
}
