using System;
using System.Collections.Generic;
using Biz.Lib.TACM.ThuLy.Model;

namespace Biz.Lib.TACM.ThuLy.IDataAccess
{
    public interface IThuLyDataAccess
    {
        //ChiTietHoSoVuAn       
        IEnumerable<HoSoVuAnThuLyModel> GetDanhSachNgaySuaDoiHoSoVuAnThuLy(int hoSoVuAnId, int giaiDoan);
        HoSoVuAnThuLyModel ChiTietHoSoVuAnThuLyTheoId(int hoSoVuAnLogId);
        HoSoVuAnThuLyModel ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(int hoSoVuAnId, int giaiDoan);
        ResponseResult SuaDoiHoSoVuAnThuLy(HoSoVuAnThuLyModel dbModel);

        //AnPhiLePhi
        IEnumerable<AnPhiModel> GetDanhSachNgaySuaDoiAnPhi(int hoSoVuAnId, int giaiDoan, int congDoan);
        AnPhiModel ChiTietAnPhiTheoId(int hoSoVuAnId, int anPhiId, int giaiDoan, int congDoan);
        ResponseResult SuaYeuCauDuNopAnPhi(YeuCauDuNopAnPhiModel dbModel);
        ResponseResult SuaKetQuaDuNopAnPhi(AnPhiModel dbModel);
        ResponseResult SuaMienDuNopAnPhi(MienDuNopModel dbModel);

        //ThongTinThuLy
        IEnumerable<ThongTinThuLyModel> GetDanhSachNgaySuaDoiThongTinThuLy(int hoSoVuAnId, int giaiDoan);
        ThongTinThuLyModel ChiTietThongTinThuLyTheoId(int thongTinThuLyId, int hoSoVuAnId);
        ResponseResult SuaDoiThongTinThuLy(ThongTinThuLyModel dbModel);
        int KiemTraTinhTrangNhapThongTinAnPhi(int hoSoVuAnId);
        int SoThuLyMax(int hoSoVuAnId, DateTime? ngayThuLy);
        int SoThuLyMaxChoADBPXLHC(int ToaAnID, string NhomAn, int giaiDoan);
        //ResponseResult CheckSoThuLy(string SoThuLy, int HoSoVuAnID, int ToaAnID, string NhomAn, int GiaiDoan);
        ResponseResult CheckSoThuLy(string SoThuLy, int HoSoVuAnID, DateTime NgayThuLy);
        ThongTinThuLyModel ChiTietThongTinThuLyCopySangPhucTham(int hoSoVuAnId);
        ThongTinThuLyModel ChiTietNoiDungKhangCaoTheoHoSoVuAnId(int hoSoVuAnId);

        //PhanCongThamPhan
        IEnumerable<PhanCongThamPhanModel> GetDanhSachNgaySuaDoiPhanCongThamPhan(int hoSoVuAnId, int giaiDoan);
        IEnumerable<ThamPhanDuKhuyetModel> DanhSachThamPhanDuKhuyetTheoThamPhanId(int thamPhanId);
        IEnumerable<HoiThamNhanDanDuKhuyetModel> DanhSachHoiThamNhanDanDuKhuyetTheoThamPhanId(int thamPhanId);
        IEnumerable<ThuKyDuKhuyetModel> DanhSachThuKyDuKhuyetTheoThamPhanId(int thamPhanId);
        PhanCongThamPhanModel ChiTietPhanCongThamPhanTheoId(int thamPhanId);
        ResponseResult SuaDoiPhanCongThamPhan(PhanCongThamPhanModel dbModel);

        //GhiChuPhanCong
        IEnumerable<GhiChuPhanCongModel> GetDanhSachNgaySuaDoiGhiChuPhanCong(int hoSoVuAnId, int giaiDoan);
        GhiChuPhanCongModel ChiTietGhiChuPhanCongId(int ghichuid);
        ResponseResult ThemGhiChuPhanCong(GhiChuPhanCongModel dbModel);
    }
}
