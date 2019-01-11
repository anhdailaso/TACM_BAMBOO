using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.TACM.QuanLyNhanVien.Model;

namespace Biz.Lib.TACM.QuanLyNhanVien.IDataAccess
{
    public interface IQuanLyNhanVienDataAccess
    {
        IEnumerable<NhanVienModel> DanhsachNhanVien();
        NhanVienModel ChitietNhanVien(int NhanVienid);
        IEnumerable<ChucNangModel> ChiTietChucNang(int NhanVienid);
        ResponseResult ThemNhanVien(NhanVienModel model);
        ResponseResult SuaNhanVien(NhanVienModel model);
        ResponseResult XoaNhanVien(int NhanVienid);
        IEnumerable<SelectListChucDanhModel> DanhSachChucDanh(int toaAnid);
        string GetMaNVtheoEmail(string email);
        ResponseResult CheckMaNV(string MaNV);
        IEnumerable<NhanVienModel> ImportNhanVien(string nguoitao, string nhanvienDataXml, int ToaAnID);
        IEnumerable<NhanVienModel> DanhsachNhanVienTheoToaAn(int ToaAnID);
        IEnumerable<NhanVienModel> DanhSachNhanVienSearchByKeyword(string keyword, int? toaAnID);
        IEnumerable<NhanVienModel> DanhSachNhanVienTheoChucDanh(int? toaAnID, string chucDanh);
        IEnumerable<NhanVienModel> DanhSachNhanVienTheoChucVu(int? toaAnID, string chucDanh);
        ResponseResult CapNhatChucNangNhanVien(List<ChucNangModel> danhSachChucNang);
        ResponseResult CapNhatThuKyThamPhan(List<NhanVienModel> danhSachChucNang);
        ResponseResult Capnhathinhdaidien(int NhanVienid, string image_url);
        IEnumerable<NhanVienToaAnModel> DanhSachNhanVienToaAn(int NhanVienID);
        ResponseResult SuaToaAnTrucThuoc(int nhanVienID, int toaAnID);
        ResponseResult ThemNhanVienToaAn(int NhanVienID, int ToaAnID, string nguoiTao);
        ResponseResult XoaNhanVienToaAn(int NhanVienToaAnID, int NhanVienID, int ToaAnID, string nguoiTao);
        IEnumerable<NhanVienModel> DanhsachThuKyThamPhan(string MaNV);
    }
}
