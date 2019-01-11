using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Biz.Lib.TACM.ThuLy.Model
{
    public class AnPhiModel
    {
        public int AnPhiID { get; set; }
        public int HoSoVuAnID { get; set; }
        public int SoAnPhi { get; set; }
        public string NopAnPhi { get; set; }
        public decimal? GiaTriTranhChap { get; set; }
        public decimal? TongAnPhi { get; set; }
        public string PhanTramDuNop { get; set; }
        public decimal? GiaTriDuNop { get; set; }
        public DateTime? HanNopAnPhi { get; set; }
        public string CoQuanThiHanhAnThu { get; set; }
        public string DiaChiCoQuanThiHanhAnThu { get; set; }
        public DateTime? NgayRaThongBao { get; set; }
        public DateTime? NgayGiaoThongBao { get; set; }
        public DateTime? NgayNopAnPhi { get; set; }
        public string SoBienLai { get; set; }
        public DateTime? NgayNopBienLaiChoToaAn { get; set; }
        public string NguoiNhanBienLai { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
        public int NguoiDuNop { get; set; }
        public string HoTenNguoiDuNop { get; set; }
    }

    public class YeuCauDuNopAnPhiModel
    {
        public int AnPhiID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string NopAnPhi { get; set; }
        public decimal? GiaTriTranhChap { get; set; }
        public decimal? TongAnPhi { get; set; }

        [Range(0, 100, ErrorMessage = "% Dự nộp phải lớn hơn 0% và nhỏ hơn 100%.")]
        public float? PhanTramDuNop { get; set; }

        public decimal? GiaTriDuNop { get; set; }
        public DateTime? HanNopAnPhi { get; set; }
        public string CoQuanThiHanhAnThu { get; set; }
        public string DiaChiCoQuanThiHanhAnThu { get; set; }
        public DateTime? NgayRaThongBao { get; set; }
        public DateTime? NgayGiaoThongBao { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
        public int NguoiDuNop { get; set; }
    }

    public class MienDuNopModel
    {
        public int HoSoVuAnID { get; set; }
        public string NopAnPhi { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
    }

    public class KetQuaNopAnPhiModel
    {
        public int KetQuaNopAnPhiID { get; set; }
        public int HoSoVuAnID { get; set; }
        public DateTime NgayNopAnPhi { get; set; }
        public string SoBienLai { get; set; }
        public DateTime NgayNopBienLaiChoToaAn { get; set; }
        public string NguoiNhanBienLai { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
    }
}
