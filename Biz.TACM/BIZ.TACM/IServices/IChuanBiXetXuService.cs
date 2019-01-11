using Biz.Lib.TACM.ChuanBiXetXu.Model;
using Biz.TACM.Models.ViewModel.ChuanBiXetXu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biz.TACM.IServices
{
    public interface IChuanBiXetXuService
    {
        //HoaGiai
        SelectList SelectListNhanVien(string nhomChucNang, int selected);
        IEnumerable<NhanVienModel> DanhSachThuKyTheoThamPhan(string maNV);
        SelectList SelectListThuKyTheoThamPhan(string maNV, string selected);
        SelectList DanhSachNgayHoaGiai(int hoSoVuAnID, int giaiDoan, int selected);
        HoaGiaiModel ChiTietHoaGiaiTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        HoaGiaiModel ChiTietHoaGiaiTheoHoaGiaiID(int banAnID);
        ResponseResult ThemHoaGiai(HoaGiaiModel viewModel);

        //QuyetDinh
        SelectList SelectListQuyetDinhLoai(string selectedValue);
        //IEnumerable<DLLoaiQuyetDinhModel> DLLoaiQuyetDinh(int giaiDoan);
        //IEnumerable<DLQuyetDinhModel> DLDanhSachQuyetDinh(int loaiQuyetDinhID);
        IEnumerable<QuyetDinhModel> DanhSachQuyetDinh(int hoSoVuAnID, int giaiDoan);
        QuyetDinhModel ChiTietQuyetDinhTheoQuyetDinhID(int quyetDinhID);
        ResponseResult SuaQuyetDinh(QuyetDinhModel viewModel);
        ResponseResult ThemQuyetDinh(QuyetDinhModel viewModel);
        ResponseResult XoaQuyetDinh(int quyetDinhID);

        //QuyetDinh HinhSu
        IEnumerable<LyDo> DanhSachLyDoSelected(string groupName, List<string> selected);
        SelectList DanhSachNgaySuaQuyetDinhHinhSu(int hoSoVuAnID, int giaiDoan, int quyetDinhGroup, int selected);
        QuyetDinhHinhSuModel ChiTietQuyetDinhHinhSuTheoHoSoVuAnId(int hoSoVuAnId, int giaiDoan, int quyetDinhGroup);
        QuyetDinhHinhSuModel ChiTietQuyetDinhHinhSuTheoQuyetDinhId(int quyetDinhId);
        ResponseResult ThemQuyetDinhHinhSu(QuyetDinhHinhSuModel viewModel, int quyetdinhGroup);

        //ChuyenHoSo
        ChuyenHoSoViewModel ChiTietChuyenHoSoTheoId(int chuyenDonId);
        Lib.TACM.NhanDon.Model.ResponseResult SuaDoiChuyenHoSo(ChuyenHoSoViewModel viewModel);


    }
}