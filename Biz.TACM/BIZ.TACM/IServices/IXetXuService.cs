using Biz.Lib.TACM.XetXu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.SettingData.Model;

namespace Biz.TACM.IServices
{
    public interface IXetXuService
    {
        //NoiDung
        //SelectList SelectListThuTuc(string selectedValue);
        //SelectList SelectListVuAnDuocXetXu(string selectedValue);
        SelectList DanhSachNgayXetXu(int hoSoVuAnID, int giaiDoan, int xetXuGroup, int selected);
        SelectList SelectListNhanVien(string nhomChucNang, int toaAnId, int selected);
        List<NhanVienModel> DanhSachNhanVien(string nhomChucNang, int toaAnId, List<NhanVienModel> selected);
        XetXuModel ChiTietXetXuTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int xetXuGroup);
        XetXuModel ChiTietXetXuTheoXetXuID(int khieuNaiTraDonID);
        ResponseResult ThemXetXu(XetXuModel viewModel);
        XetXuModel AddDanhSachDuKhuyetTheoHoSoVuAnIdToModel(XetXuModel dbModel);

        //TrieuTap
        SelectList DanhSachNgayTrieuTap(int hoSoVuAnID, int giaiDoan, int selected);
        List<DuongSuModel> DanhSachDuongSu(int hoSoVuAnID, List<string> selected);
        List<DuongSuModel> DanhSachTrieuTapDuongSu(int trieuTapID);
        TrieuTapModel ChiTietTrieuTapTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        TrieuTapModel ChiTietTrieuTapTheoTrieuTapID(int khieuNaiTraDonID);
        ResponseResult ThemTrieuTap(TrieuTapModel viewModel);
        SelectList SelectListLanThu(int lanThu);
    }
}