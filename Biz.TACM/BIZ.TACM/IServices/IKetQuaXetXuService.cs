using Biz.Lib.TACM.KetQuaXetXu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biz.TACM.IServices
{
    public interface IKetQuaXetXuService
    {
        //QuyetDinh
        SelectList DanhSachNgayQuyetDinh(int hoSoVuAnID, int giaiDoan, int selected);
        QuyetDinhModel ChiTietQuyetDinhTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        QuyetDinhModel ChiTietQuyetDinhTheoQuyetDinhID(int quyetDinhID);
        QuyetDinhModel ChiTietQuyetDinhTheoSoQuyetDinh(string soQuyetDinh, int hoSoVuAnID, DateTime ngayRaQuyetDinh);
        ResponseResult ThemQuyetDinh(QuyetDinhModel viewModel);
        int SoQuyetDinhMax(int hoSoVuAnId, DateTime? ngayRaQuyetDinh);
        //BanAn       
        SelectList DanhSachNgayBanAn(int hoSoVuAnID, int giaiDoan, int selected);
        BanAnModel ChiTietBanAnTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        BanAnModel ChiTietBanAnTheoBanAnID(int quyetDinhID);
        BanAnModel ChiTietBanAnTheoSoBanAn(string soBanAn, int hoSoVuAnID, DateTime ngayTuyenAn);
        ResponseResult ThemBanAn(BanAnModel viewModel);
        int SoBanAnMax(int hoSoVuAnId, DateTime? ngayTuyenAn);
        ResponseResult CapNhatHuyBanAnSoTham(int hoSoVuAnID);

        //QuyetDinh NhomAn ADBPXLHC - PhucTham
        QuyetDinhADBPXLHCModel ChiTietQuyetDinhADBPXLHCTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        QuyetDinhADBPXLHCModel ChiTietQuyetDinhADBPXLHCTheoQuyetDinhID(int quyetDinhId);
        ResponseResult ThemQuyetDinhADBPXLHC(QuyetDinhADBPXLHCModel viewModel);

        //Toi Danh
        ToiDanhModel ChiTietToiDanhTheoID(int toiDanhID);
        IEnumerable<ToiDanhModel> DanhSachKetQuaXetXu(int hoSoVuAnId);
        ResponseResult TaoToiDanh(ToiDanhModel model);
        ResponseResult SuaToiDanh(ToiDanhModel model);
        ResponseResult XoaToiDanh(int toiDanhId);
    }
}