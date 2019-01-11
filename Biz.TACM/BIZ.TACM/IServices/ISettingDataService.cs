using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Biz.Lib.SettingData.Model;
using Biz.TACM.Models.Model;

namespace Biz.TACM.IServices
{
    public interface ISettingDataService
    {
        //Setting Group
        List<SettingItemModel> ListSettingDataItem(string groupName);
        SelectList SettingDataItem(string groupName, string selected);
        SelectList SettingDataItemHaveValueText(string groupName, string selected);
        SelectList SettingDataItemXML(string groupName, string selectedValue);

        //ToaAn
        SelectList SelectListDanhSachToaAn(string selectedValue);
        SelectList SelectListDanhSachToaAnValueIsTenToaAn(string selectedValue);
        SelectList SelectListDanhSachToaAnValueIsToaAnID(int? selectedValue);
        SelectList SelectListDanhSachToaAnValueIsToaAnIDTheoNV(int? selectedValue, string maNV);
        List<ToaAnModel> DanhSachToaAnTheoNV(string maNV);
        List<ToaAnModel> DanhSachToaAn();
        ToaAnModel GetToaAnTheoMaNhanVien(string maNhanVien);
        ToaAnModel GetToaAnTheoToaAnID(int toaAnID);
        ToaAnModel GetToaAnTheoTenToaAn(string tenToaAn);
        SelectList DanhSachDiaChiCoQuanThiHanhAn(string groupName, string selected);

        //NhomAn
        List<NhomAnModel> DanhSachNhomAnTheoToaAn(int toaAnId);
        SelectList DanhSachNhomAnChoDDL(string groupName, string selected);

        //NhanVien
        SelectList DanhSachNhanVienTheoChucDanh(string chucDanh, int? toaAnId, string selected);
        List<NhanVienModel> DanhSachNhanVienTheoChucDanhSelected(string chucDanh, int toaAnId, List<string> selected);
        SelectList DanhSachNhanVienTheoTenChucNangNghiepVu(string tenChucNang, int toaAnId, string selected);
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

        //For So Ban An, So Quyet Dinh, So Thu Ly
        int ParseCacLoaiSoToInt(string strNumber);


        SelectList SelectListLanThu(int lanThu);

        //session nhanvien
        void UpdateNhanVienSessionInfo(int? nhanVienID, string maNV, string maNVMoi, string hoNV, string tenLotNV, string tenNV, string hoVaTen, string pathImage);
        NhanVienSessionModel GetNhanVienSessionInfo();
    }
}
