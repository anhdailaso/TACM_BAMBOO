using Biz.Lib.TACM.QuanLyNhanVien.Model;
using Biz.TACM.Models.ViewModel.SoDoToChuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.TACM.Models.ViewModel.QuanLyNhanVien;
using Biz.TACM.Models.ViewModel.TaiKhoanNhanVien;

namespace Biz.TACM.IServices
{
    public interface IQuanLyNhanVienService
    {
        string GetManVTheoEmail(string email);
        //IEnumerable<NhanVienModel> DanhsachNhanVien();
        NhanVienViewModel ChitietNhanVien(int NhanVienid);
        IEnumerable<ChucNangModel> ChitietNhanVienChucNang(int NhanVienid);
        ResponseResult ThemNhanVien(NhanVienViewModel model);
        ResponseResult SuaNhanVien(NhanVienViewModel model);
        ResponseResult XoaNhanVien(int NhanVienid);
        IEnumerable<SelectListChucDanhModel> DanhSachChucDanh(int toaAnid);
        IEnumerable<NhanVienModel> DanhsachNhanVienTheoToaAn(int ToaAnID);
        string ValidateExcel(HttpPostedFileBase dataSource, int ToaAnID);
        int ImportNhanVien(HttpPostedFileBase file, int ToaAnID);
        IEnumerable<NhanVienModel> DanhSachNhanVienSearchByKeyword(string keyword, int? toaAnID);
        IEnumerable<ChucDanhViewModel> SoDoToChucQuanLyNhanVien(int? toaAnId);
        IEnumerable<ChucDanhViewModel> SoDoToChucQuanLyNhanVienChucVu(int? toaAnId);
        IEnumerable<NhanVienModel> DanhSachNhanVienTheoChucDanh(int? toaAnId, string chucDanh);
        IEnumerable<NhanVienModel> DanhSachNhanVienTheoChucVu(int? toaAnId, string chucDanh);
        ResponseResult CapNhatChucNangNhanVien(ChucNangViewModel danhSachChucNang);
        ResponseResult CapNhatThuKyThamPhan(NhanVienViewModel danhsachthuky);
        IEnumerable<NhanVienToaAnModel> DanhSachNhanVienToaAn(int NhanVienID);
        ResponseResult ThemNhanVienToaAn(int NhanVienID, int ToaAnID);
        ResponseResult XoaNhanVienToaAn(int NhanVienToaAnID, int NhanVienID, int ToaAnID);
        ResponseResult SuaToaAnTrucThuoc(int nhanVienID, int toaAnID);
        IEnumerable<NhanVienModel> DanhsachThuKyThamPhan(string ManV);
    }
}