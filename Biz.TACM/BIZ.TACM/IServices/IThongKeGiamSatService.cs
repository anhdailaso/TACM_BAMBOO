using Biz.TACM.Models.ViewModel.ThongKeGiamSat;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Biz.Lib.TACM.ThongKeGiamSat.Models;

namespace Biz.TACM.IServices
{
    public interface IThongKeGiamSatService
    {
        IEnumerable<GiamSatHoSoVuAnViewModel> GetDanhSachHoSoVuAnGiamSat(string listHoSoVuAnID);
        GiamSatViewModel LocDuLieuGiamSat(GiamSatLocDuLieuViewModel viewModel);

        //ThongKeTongHop
        IEnumerable<ThongKeTongHopModel> ThongKeTongHop(int loaiThongKe, string tuNgay, string denNgay, int? toaAnID = null, string nhomAn = null, int? giaiDoan = null, string loaiQuanHe = null, string loaiDeNghi = null, string quanHePhapLuat = null, string toiDanh = null);
        IEnumerable<HoSoBaoCaoThongKeModel> DanhSachHoSoBaoCaoThongKe(string danhSachId);
        DataSet GetDataSetHoSoBaoCaoThongKe(string danhSachId);
        Stream ExportDanhSachHoSoThongKe(string danhSachId);
        Stream ExportChiTienXetXu(string tuNgay, string denNgay, int? toaAnId);

        //ThongKePhanCongThamPhan
        IEnumerable<HoSoPhanCongThamPhanModel> DanhSachHoSoChuaPhanCongThamPhan(string tuNgay, string denNgay, int toaAnId);
        DuLieuThongKeTrucTuyenModel ThongKeTrucTuyenPhanCongThamPhan(string tuNgay, string denNgay, int toaAnId);
        
        //ThongKeLuuKho
        ThongKeLuuKhoModel DuLieuBieuDoThongKeLuuKho(ref string listHoSoVuAnId, string tuNgay, string denNgay, int group, int? toaAnId = null, string nhomAn = null, int? giaiDoan = null);
        IEnumerable<HoSoThongKeLuuKhoModel> DanhSachHoSoThongKeLuuKho(string listHoSoVuAnId);
        Stream ExportThongKeLuuKho(string listHoSoVuAnId);

        //ThongKeAnHuySua
        ThongKeAnHuySuaModel DuLieuBieuDoThongKeAnHuySua(ref string listHoSoHuyId, ref string listHoSoSuaId, string tuNgay, string denNgay, int? toaAnId, string thamPhan);
        IEnumerable<HoSoThongKeHuySuaModel> DanhSachHoSoThongKeAnHuySua(string listHoSoHuyId, string listHoSoSuaId);
    }
}
