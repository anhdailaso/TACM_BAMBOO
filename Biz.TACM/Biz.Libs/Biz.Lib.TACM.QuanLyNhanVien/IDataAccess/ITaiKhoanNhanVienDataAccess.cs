using Biz.Lib.TACM.QuanLyNhanVien.Model;
using System;
using System.Collections.Generic;

namespace Biz.Lib.TACM.QuanLyNhanVien.IDataAccess
{
    public interface ITaiKhoanNhanVienDataAccess
    {
        NhanVienModel ThongTinNhanVien(string maNV);
        IEnumerable<ChucNangModel> DanhSachChucNangTheoMaNhanVien(string maNV);
        ResponseResult CapNhatThongTinNhanVien(NhanVienModel model);
        ResponseResult CapNhatDuongDanHinhDaiDienNhanVien(NhanVienModel model);
    }
}
