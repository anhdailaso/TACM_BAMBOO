using Biz.Lib.TACM.ThongKeGiamSat.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Biz.Lib.TACM.ThongKeGiamSat.IDataAccess
{
    public interface IThongKeGiamSatDataAccess
    {
        //ThongKe
        IEnumerable<ThongKeTongHopModel> ThongKeTongHop(int loaiThongKe, string tuNgay, string denNgay, int? toaAnID = null, string nhomAn = null, int? giaiDoan = null, string loaiQuanHe = null, string loaiDeNghi = null, string quanHePhapLuat = null, string toiDanh = null);
        IEnumerable<HoSoBaoCaoThongKeModel> DanhSachHoSoBaoCaoThongKe(string danhSachId);
        DataSet GetDataSetHoSoBaoCaoThongKe(string danhSachId);
        DataSet GetDataSetChiTienXetXu(string tuNgay, string denNgay, int? toaAnId);
        DataSet GetDataSetThongKeLuuKho(string danhSachId);

        IEnumerable<HoSoPhanCongThamPhanModel> DanhSachHoSoChuaPhanCongThamPhan(string tuNgay, string denNgay, int toaAnId);
        List<ThongKeTrucTuyenModel> ThongKeTrucTuyenPhanCongThamPhan(string tuNgay, string denNgay, int toaAnId);
        IEnumerable<DuLieuBieuDoModel> DuLieuBieuDoThongKeLuuKho(ref string listHoSoVuAnId, string tuNgay, string denNgay, int group, int? toaAnId = null, string nhomAn = null, int? giaiDoan = null);
        IEnumerable<HoSoThongKeLuuKhoModel> GetDanhSachHoSoThongKeLuuKho(string listHoSoVuAnId);
        IEnumerable<DuLieuBieuDoHuySuaModel> DuLieuBieuDoThongKeAnHuySua(ref string listHoSoHuyId, ref string listHoSoSuaId, string tuNgay, string denNgay, int? toaAnId, string thamPhan);
        IEnumerable<HoSoThongKeHuySuaModel> GetDanhSachHoSoThongKeAnHuySua(string listHoSoHuyId, string listHoSoSuaId);

        //GiamSat
        IEnumerable<GiamSatHoSoVuAnModel> GetDanhSachHoSoVuAnGiamSat(string listHoSoVuAnID);
        IEnumerable<GiamSatDuLieuBieuDoModel> LocDuLieuGiamSat(ref string listHoSoVuAnID, string tuNgay, string denNgay, int group, int? toaAnID = null, string nhomAn = null, int? giaiDoan = null);
    }
}
