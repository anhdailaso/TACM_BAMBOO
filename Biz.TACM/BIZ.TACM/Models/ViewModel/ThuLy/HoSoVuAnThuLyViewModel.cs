using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Biz.Lib.SettingData.Model;

namespace Biz.TACM.Models.ViewModel.ThuLy
{
    public class HoSoVuAnThuLyViewModel
    {
        public int HoSoVuAnID { get; set; }
        public int HoSoVuAnLogID { get; set; }
        public string MaHoSo { get; set; }
        public int SoHoSo { get; set; }
        public string NhomAn { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public string TrangThaiCongDoan { get; set; }

        [Required(ErrorMessage = "Ngày làm đơn không được để trống.")]
        public string NgayLamDon { get; set; }

        [Required(ErrorMessage = "Ngày nộp đơn tại tòa án không được để trống.")]
        public string NgayNopDonTaiToaAn { get; set; }

        [Required(ErrorMessage = "Hình thức gửi đơn không được để trống.")]
        public string HinhThucGoiDon { get; set; }

        [Required(ErrorMessage = "Loại đơn không được để trống.")]
        public string LoaiDon { get; set; }

        [Required(ErrorMessage = "Loại quan hệ không được để trống.")]
        public string LoaiQuanHe { get; set; }

        [Required(ErrorMessage = "Quan hệ pháp luật không được để trống.")]
        public string QuanHePhapLuat { get; set; }

        [Required(ErrorMessage = "Yếu tố nước ngoài không được để trống.")]
        public string YeuToNuocNgoai { get; set; }

        [Required(ErrorMessage = "Người ký xác nhận đã nhận đơn không được để trống.")]
        public string NguoiKyXacNhanDaNhanDon { get; set; }

        public string CanBoNhanDon { get; set; }

        public string ThuLyTheoThuTuc { get; set; }

        public string ThamPhan { get; set; }
        public string ThamPhanKhac { get; set; }
        public string HoiThamNhanDan { get; set; }
        public string ThuKy { get; set; }
        public string KiemSatVien { get; set; }
        public int TrangThai { get; set; }
        public string GiaTriDuNop { get; set; }
        public DateTime? HanNopAnPhi { get; set; }
        public DateTime? NgayChuyenDon { get; set; }
        public DateTime? NgayTraDon { get; set; }
        public DateTime? NgayKhieuNai { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string GhiChu { get; set; }

        public NhanVienModel NhanVienThamPhan { get; set; }
        public NhanVienModel NhanVienCanBoNhanDon { get; set; }
        public NhanVienModel NhanVienNguoiKyXacNhanDaNhanDon { get; set; }
        public NhanVienModel NhanVienNguoiLapDon { get; set; }
    }
}