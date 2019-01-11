using Biz.Lib.TACM.KetQuaXetXu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.KetQuaXetXu.IDataAccess
{
    public interface IKetQuaXetXuDataAccess
    {
        //QuyetDinh
        IEnumerable<LoaiQuyetDinhModel> DanhSachLoaiQuyetDinh();
        IEnumerable<QuyetDinhModel> DanhSachNgayQuyetDinh(int hoSoVuAnID, int giaiDoan);
        QuyetDinhModel ThongTinQuyetDinhTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        QuyetDinhModel ThongTinQuyetDinhTheoQuyetDinhID(int id);
        QuyetDinhModel ThongTinQuyetDinhTheoSoQuyetDinh(string soQuyetDinh, int hoSoVuAnID, DateTime ngayRaQuyetDinh);
        ResponseResult ThemQuyetDinh(QuyetDinhModel model);
        int SoQuyetDinhMax(int hoSoVuAnId, DateTime? ngayRaQuyetDinh);
        //BanAn
        IEnumerable<BanAnModel> DanhSachNgayBanAn(int hoSoVuAnID, int giaiDoan);
        BanAnModel ThongTinBanAnTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        BanAnModel ThongTinBanAnTheoBanAnID(int id);
        BanAnModel ThongTinBanAnTheoSoBanAn(string soBanAn, int hoSoVuAnID, DateTime ngayTuyenAn);
        ResponseResult ThemBanAn(BanAnModel model);
        int SoBanAnMax(int hoSoVuAnId, DateTime? ngayTuyenAn);
        ResponseResult CapNhatHuyBanAnSoTham(int hoSoVuAnID);

        //QuyetDinh ADBPXLHC
        QuyetDinhADBPXLHCModel ThongTinQuyetDinhADBPXLHCTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        QuyetDinhADBPXLHCModel ThongTinQuyetDinhADBPXLHCTheoQuyetDinhID(int quyetDinhID);
        ResponseResult ThemQuyetDinhADBPXLHC(QuyetDinhADBPXLHCModel model);

        //Toi Danh
        ToiDanhModel ChiTietToiDanhTheoID(int toiDanhID);
        IEnumerable<ToiDanhModel> DanhSachKetQuaXetXu(int hoSoVuAnId);
        ResponseResult TaoToiDanh(ToiDanhModel model);
        ResponseResult SuaToiDanh(ToiDanhModel model);
        ResponseResult XoaToiDanh(int toiDanhId, string nguoiTao);
    }
}
