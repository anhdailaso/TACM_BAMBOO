using System;
using System.Collections.Generic;
using Biz.Lib.TACM.SauXetXu.Model.KhangCao;
using Biz.Lib.TACM.SauXetXu.Model.KhangCao.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.KhangCaoQuaHan;
using Biz.Lib.TACM.SauXetXu.Model.KhangCaoQuaHan.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.KhangNghi;
using Biz.Lib.TACM.SauXetXu.Model.KhangNghi.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.LuuKho;
using Biz.Lib.TACM.SauXetXu.Model.LuuKho.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.Shared;
using Biz.Lib.TACM.SauXetXu.Model.Shared.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.SuaChuaBanAn;
using Biz.Lib.TACM.SauXetXu.Model.SuaChuaBanAn.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.TamUngAnPhi;


namespace Biz.Lib.TACM.SauXetXu.IDataAccess
{
    public interface ISauXetXuDataAccess
    {
        #region PhatHanhBanAn
        IEnumerable<NgayPhatHanhBanAnModel> DanhSachNgayPhatHanhBanAn(int hoSoVuAnId, int giaiDoan);
        ThongTinPhatHanhBanAnModel ThongTinPhatHanhBanAnTheoId(int phatHanhBanAnId);
        IEnumerable<DuongSuModel> DanhSachDuongSuNhanPhatHanhBanAn(int phatHanhBanAnId);
        IEnumerable<DuongSuModel> DanhSachDuongSuTheoHoSo(int hoSoVuAnId);
        bool PhatHanhBanAn(PhatHanhBanAnDataRequest request);
        #endregion

        #region SuaChuaBanAn
        IEnumerable<NgaySuaChuaBanAnModel> DanhSachNgaySuaChuaBanAn(int hoSoVuAnId, int giaiDoan);
        ThongTinSuaChuaBanAnModel ThongTinSuaChuaBanAnTheoNgay(DateTime ngaySuaChua);
        ThongTinSuaChuaBanAnModel ThongTinSuaChuaBanAnTheoId(int suaChuaBanAnId);
        bool SuaChuaBanAn(SuaChuaBanAnDataRequest request);
        #endregion

        #region KhangNghi
        IEnumerable<NgayKhangNghiModel> DanhSachNgayKhangNghi(int hoSoVuAnId, int giaiDoan);
        ThongTinKhangNghiModel ThongTinKhangNghiTheoId(int khangNghiId);
        bool KhangNghi(KhangNghiDataRequest request);
        #endregion

        #region KhangCaoQuaHan
        IEnumerable<NgayKhangCaoQuaHanModel> DanhSachNgayKhangCaoQuanHan(int hoSoVuAnId, int giaiDoan);
        ThongTinKhangCaoQuaHanModel ThongTinKhangCaoQuaHanTheoId(int khangCaoQuaHanId);
        bool KhangCaoQuaHan(KhangCaoQuaHanDataRequest request);
        #endregion

        #region LuuKho
        IEnumerable<NgayLuuKhoModel> DanhSachNgayLuuKho(int hoSoVuAnId, int giaiDoan);
        ThongTinLuuKhoModel ThongTinLuuKhoTheoId(int luuKhoId);
        bool LuuKho(LuuKhoDataRequest request);
        #endregion

        #region KhangCao
        IEnumerable<KhangCaoModel> DanhSachKhangCao(int hoSoVuAnId, int giaiDoan);
        ThongTinKhangCaoModel ThongTinKhangCaoTheoId(int khangCaoId);
        bool KhangCao(KhangCaoDataRequest request);
        bool SuaKhangCao(SuaKhangCaoDataRequest request);
        bool XoaKhangCao(int khangCaoId);
        #endregion

        #region TamUngAnPhi

        IEnumerable<TamUngAnPhiModel> DanhSachNgaySuaDoiTamUngAnPhi(int hoSoVuAnId, int giaiDoan, int congDoan);
        TamUngAnPhiModel ChiTietTamUngAnPhiTheoId(int hoSoVuAnId, int anPhiId, int giaiDoan, int congDoan);
        ResponseResult SuaYeuCauDuNopTamUngAnPhi(YeuCauDuNopAnPhiModel dbModel);
        ResponseResult SuaKetQuaDuNopTamUngAnPhi(TamUngAnPhiModel dbModel);
        ResponseResult SuaMienDuNopTamUngAnPhi(MienDuNopModel dbModel);

        #endregion

        #region KiemTraDinhKemFile ChuyenHoSo

        int KiemTraDinhKemFileQuyetDinh(int hoSoVuAnId, int giaiDoan);
        int KiemTraDinhKemFileBanAn(int hoSoVuAnId, int giaiDoan);

        #endregion
    }
}
