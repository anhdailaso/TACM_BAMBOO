using Biz.Lib.SettingData.Model;
using System.Collections.Generic;

namespace Biz.Lib.SettingData.DataAccess
{
    public interface ISettingDataAccess
    {
        //Setting Group
        List<SettingItemModel> GetDataItem(string groupName);
        List<SettingItemModel> GetDataItemCached(string groupName);

        //ToaAn
        List<ToaAnModel> DanhSachToaAn();
        List<ToaAnModel> DanhSachToaAnTheoNV(string maNV);
        ToaAnModel GetToaAnTheoMaNhanVien(string maNhanVien);
        ToaAnModel GetToaAnTheoToaAnID(int toaAnID);
        ToaAnModel GetToaAnTheoTenToaAn(string tenToaAn);

        //NhomAn
        List<NhomAnModel> DanhSachNhomAnTheoToaAn(int toaAnId);

        //NhanVien
        List<NhanVienModel> DanhSachNhanVienTheoChucDanh(string chucDanh, int? toaAnId);
        List<NhanVienModel> DanhSachNhanVienTheoTenChucNangNghiepVu(string tenChucNang, int toaAnId);
        NhanVienModel ChiTietNhanVienTheoMaNhanVien(string maNV);
        NhanVienModel GetSessionInfoNhanVienTheoMaNhanVien(string maNV);

        //HoSoVuAn
        List<HoSoVuAnModel> DanhSachHoSoVuAnIsInRole(string maNV);
        string GetLoaiQuanHe(int hoSoVuAnId);

        //DanhSachDuKhuyet
        List<NhanVienModel> DanhSachThamPhanDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId);
        List<NhanVienModel> DanhSachHoiThamNhanDanDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId);
        List<NhanVienModel> DanhSachThuKyDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId);
        List<NhanVienModel> DanhSachKiemSatVienDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId);


    }
}
