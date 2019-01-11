using Biz.Lib.TACM.XetXu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.SettingData.Model;

namespace Biz.Lib.TACM.XetXu.IDataAccess
{
    public interface IXetXuDataAccess
    {
        //XetXu
        IEnumerable<XetXuModel> DanhSachNgayXetXu(int hoSoVuAnID, int giaiDoan, int xetXuGroup);
        List<NhanVienModel> DanhSachThamPhanDuKhuyet(DateTime? ngayTao);
        List<NhanVienModel> DanhSachHoiThamNhanDanDuKhuyet(DateTime? ngayTao);
        List<NhanVienModel> DanhSachThuKyDuKhuyet(DateTime? ngayTao);
        List<NhanVienModel> DanhSachKiemSatVienDuKhuyet(DateTime? ngayTao);
        IEnumerable<NhanVienModel> DanhSachNhanVien(string nhomChucNang, int toaAnId);
        NhanVienModel ThongTinNhanVien(string maNV);
        XetXuModel ThongTinXetXuTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int xetXuGroup);
        XetXuModel ThongTinXetXuTheoXetXuID(int id);
        ResponseResult ThemXetXu(XetXuModel model);
        //TrieuTap
        IEnumerable<TrieuTapModel> DanhSachNgayTrieuTap(int hoSoVuAnID, int giaiDoan);
        List<DuongSuModel> DanhSachDuongSu(int hoSoVuAnID);
        List<DuongSuModel> DanhSachTrieuTapDuongSu(int trieuTapID);
        TrieuTapModel ThongTinTrieuTapTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        TrieuTapModel ThongTinTrieuTapTheoTrieuTapID(int id);
        ResponseResult ThemTrieuTap(TrieuTapModel model);
    }
}
