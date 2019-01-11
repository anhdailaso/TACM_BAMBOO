using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Enums
{
    public enum GiaiDoan
    {
        SoTham = 1,
        PhucTham = 2
    }

    public enum CongDoan
    {
        NhanDon = 1,
        NhanHoSo = 1,
        ThuLy = 2,
        ChuanBiXetXu = 3,
        KetQuaXetXu = 4,
        SauXetXu = 5,
        LuuKho = -1
    }

    public enum ToaAn
    {
        ThanhPhoCaMau = 1,
        HuyenCaiNuoc = 2,
        HuyenDamDoi = 3,
        HuyenNamCan = 4,
        HuyenNgocHien = 5,
        HuyenPhuTan = 6,
        HuyenThoiBinh = 7,
        HuyenTranVanThoi = 8,
        HuyenUMinh = 9,
        TinhCaMau = 10
    }

    public enum HoiDong
    {
        HoiDong3 = 1,
        HoiDong5 = 2
    }

    public enum CBXXQuyetDinhLoai
    {
        QuyetDinh = 1,
        ThongBao = 2
    }

    public enum XetXuGroup
    {
        DuaVuAnRaXetXu = 1,
        MoPhienHop = 2
    }

    public enum QuyetDinhHinhSuGroup
    {
        GiaHanThoiHanCBXX = 1,
        TraHoSoDieuTraBoSung = 2,
        TamDinhChiVuAn = 3,
        DinhChiVuAn = 4,
        PhucHoiVuAn = 5,
        DuaVuAnRaXetXu = 6,
        DinhChiXetXuPhucTham = 7
    }

    public enum LoaiDinhChi
    {
        KhangCao = 1,
        KhangNghi = 2,
        KhangCaoKhangNghi = 3
    }

    public enum LoaiKetQuaXetXu
    {
        BanAn = 1,
        QuyetDinh = 2,
        BanAnVaQuyetDinh = 3
    }

    public enum ThongKe
    {
        TongHop = 1,
        NhomAn = 2,
        ThamPhan = 3,
        ThuKy = 4
    }
}