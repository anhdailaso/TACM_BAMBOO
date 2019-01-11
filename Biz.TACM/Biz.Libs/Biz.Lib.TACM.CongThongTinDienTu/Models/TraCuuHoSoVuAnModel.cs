using Biz.Lib.TACM.ChuanBiXetXu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.CongThongTinDienTu.Models
{
    public class TraCuuHoSoVuAnModel
    {
        public int HoSoVuAnID { get; set; }
        public string MaHoSo { get; set; }
        public string TenVuAn { get; set; }
        public string SoThuLySoTham { get; set; }
        public string QuanHePhapLuat { get; set; }
        public string TenThamPhanSoTham { get; set; }
        public string KetQuaGiaiQuyetSoTham { get; set; }
        public string KhangCaoKhangNghi { get; set; }
        public string SoThuLyPhucTham { get; set; }
        public string TenThamPhanPhucTham { get; set; }
        public string KetQuaGiaiQuyetPhucTham { get; set; }
        public DateTime? HoaGiaiNgayGioMoPhienHop { get; set; }      //Chỉ có ở Sơ Thẩm
        public List<QuyetDinhModel> DanhSachQuyetDinhThongBaoSoTham { get; set; }
        public List<QuyetDinhModel> DanhSachQuyetDinhThongBaoPhucTham { get; set; }
        public DateTime? MoPhienHopNgayRaQuyetDinhSoTham { get; set; }
        public DateTime? MoPhienHopNgayRaQuyetDinhPhucTham { get; set; }
        public DateTime? MoPhienHopNgayGioMoPhienHopSoTham { get; set; }
        public DateTime? MoPhienHopNgayGioMoPhienHopPhucTham { get; set; }
        public DateTime? DuaVuAnRaXetXuNgayRaQuyetDinhSoTham { get; set; }
        public DateTime? DuaVuAnRaXetXuNgayRaQuyetDinhPhucTham { get; set; }
        public DateTime? DuaVuAnRaXetXuNgayGioMoPhienToaSoTham { get; set; }
        public DateTime? DuaVuAnRaXetXuNgayGioMoPhienToaPhucTham { get; set; }
        public int TrieuTapLanThuSoTham { get; set; }
        public int TrieuTapLanThuPhucTham { get; set; }
        public DateTime? TrieuTapThoiGianMoPhienToaSoTham { get; set; }
        public DateTime? TrieuTapThoiGianMoPhienToaPhucTham { get; set; }
        public string TrieuTapDiaDiemMoPhienToaSoTham { get; set; }
        public string TrieuTapDiaDiemMoPhienToaPhucTham { get; set; }
        public string LoaiQuanHe { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public string TrangThaiCongDoan { get; set; }
        public string NhomAn { get; set; }
    }
}
