using Biz.Lib.TACM.ChuanBiXetXu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.CongThongTinDienTu
{
    public class TraCuuHoSoVuAnViewModel
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
        public string HoaGiaiNgayGioMoPhienHop { get; set; }      //Chỉ có ở Sơ Thẩm
        public List<QuyetDinhModel> DanhSachQuyetDinhThongBaoSoTham { get; set; }
        public List<QuyetDinhModel> DanhSachQuyetDinhThongBaoPhucTham { get; set; }
        public string MoPhienHopNgayRaQuyetDinhSoTham { get; set; }
        public string MoPhienHopNgayRaQuyetDinhPhucTham { get; set; }
        public string MoPhienHopNgayGioMoPhienHopSoTham { get; set; }
        public string MoPhienHopNgayGioMoPhienHopPhucTham { get; set; }
        public string DuaVuAnRaXetXuNgayRaQuyetDinhSoTham { get; set; }
        public string DuaVuAnRaXetXuNgayRaQuyetDinhPhucTham { get; set; }
        public string DuaVuAnRaXetXuNgayGioMoPhienToaSoTham { get; set; }
        public string DuaVuAnRaXetXuNgayGioMoPhienToaPhucTham { get; set; }
        public int TrieuTapLanThuSoTham { get; set; }
        public int TrieuTapLanThuPhucTham { get; set; }
        public string TrieuTapThoiGianMoPhienToaSoTham { get; set; }
        public string TrieuTapThoiGianMoPhienToaPhucTham { get; set; }
        public string TrieuTapDiaDiemMoPhienToaSoTham { get; set; }
        public string TrieuTapDiaDiemMoPhienToaPhucTham { get; set; }
        public string LoaiQuanHe { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public string TrangThaiCongDoan { get; set; }
        public string NhomAn { get; set; }
    }
}