using Biz.Lib.TACM.ChuanBiXetXu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.ChuanBiXetXu.IDataAccess
{
    public interface IChuanBiXetXuDataAccess
    {
        //Hoa Giai
        IEnumerable<HoaGiaiModel> DanhSachNgayHoaGiai(int hoSoVuAnID, int giaiDoan);
        //IEnumerable<DLLoaiQuyetDinhModel> DLLoaiQuyetDinh(int giaiDoan);
        //IEnumerable<DLQuyetDinhModel> DLDanhSachQuyetDinh(int loaiQuyetDinhID);
        IEnumerable<NhanVienModel> DanhSachNhanVien(string nhomChucNang);
        IEnumerable<NhanVienModel> DanhSachThuKyTheoThamPhan(string MaNV);
        HoaGiaiModel ThongTinHoaGiaiTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan);
        HoaGiaiModel ThongTinHoaGiaiTheoHoaGiaiID(int id);
        ResponseResult ThemHoaGia(HoaGiaiModel model);

        //QuyetDinh
        IEnumerable<QuyetDinhModel> DanhSachQuyetDinh(int hoSoVuAnID, int giaiDoan);
        QuyetDinhModel ThongTinQuyetDinhTheoQuyetDinhID(int id);
        ResponseResult SuaQuyetDinh(QuyetDinhModel model);
        ResponseResult ThemQuyetDinh(QuyetDinhModel model);
        ResponseResult XoaQuyetDinh(int quyetDinhID);

        //QuyetDinh HinhSu
        IEnumerable<QuyetDinhHinhSuModel> DanhSachNgaySuaQuyetDinhHinhSu(int hoSoVuAnID, int giaiDoan, int quyetDinhGroup);
        QuyetDinhHinhSuModel ThongTinQuyetDinhHinhSuTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int quyetDinhGroup);
        QuyetDinhHinhSuModel ThongTinQuyetDinhHinhSuTheoQuyetDinhID(int id);
        ResponseResult ThemQuyetDinhHinhSu(QuyetDinhHinhSuModel model, int quyetdinhGroup);

    }
}
