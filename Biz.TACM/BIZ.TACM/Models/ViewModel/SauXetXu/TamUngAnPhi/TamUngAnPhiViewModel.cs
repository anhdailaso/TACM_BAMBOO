using System;
using System.ComponentModel.DataAnnotations;
using Biz.Lib.SettingData.Model;

namespace Biz.TACM.Models.ViewModel.SauXetXu.TamUngAnPhi
{
    public class TamUngAnPhiViewModel
    {
        public int AnPhiID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string NopAnPhi { get; set; }
        public MienDuNopViewModel MienDuNopViewModel { get; set; }
        public YeuCauDuNopAnPhiViewModel YeuCauDuNopViewModel { get; set; }
        public KetQuaNopAnPhiViewModel KetQuaNopViewModel { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string GhiChu { get; set; }
    }

    public class YeuCauDuNopAnPhiViewModel
    {
        public int AnPhiID { get; set; }
        public int HoSoVuAnID { get; set; }

        [Required(ErrorMessage = "Nộp án phí không được để trống.")]
        public string NopAnPhi { get; set; }

        [MaxLength(23, ErrorMessage = "Giá trị tranh chấp không được vượt quá 18 kí tự")]
        [Required(ErrorMessage = "Giá trị tranh chấp không được để trống.")]
        [RegularExpression("^[^a-z][0-9,]+$", ErrorMessage = "Giá trị nhập không hợp lệ.")]
        public string GiaTriTranhChap { get; set; }

        [Required]
        public string TongAnPhi { get; set; }

        [MaxLength(5, ErrorMessage = "% Dự nộp không được vượt quá 4 kí tự")]
        [RegularExpression("^(?:100(?:,0{1,2})?|(?:0|[1-9][0-9]?)(?:,[0-9]{1,2})?)", ErrorMessage = "% Dự nộp phải là một số lớn hơn 0 và nhỏ hơn 100.")]
        [Required(ErrorMessage = "% Dự nộp không được để trống.")]
        public string PhanTramDuNop { get; set; }

        [Required]
        public string GiaTriDuNop { get; set; }

        //[Required(ErrorMessage = "Hạn nộp án phí không được để trống.")]
        public string HanNopAnPhi { get; set; }

        [Required(ErrorMessage = "Cơ quan thi hành án thu không được để trống.")]
        public string CoQuanThiHanhAnThu { get; set; }

        [Required]
        public string DiaChiCoQuanThiHanhAnThu { get; set; }

        [Required(ErrorMessage = "Ngày ra thông báo không được để trống.")]
        public string NgayRaThongBao { get; set; }

        //[Required(ErrorMessage = "Ngày giao thông báo không được để trống.")]
        public string NgayGiaoThongBao { get; set; }

        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string GhiChu { get; set; }
    }

    public class MienDuNopViewModel
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

    public class KetQuaNopAnPhiViewModel
    {
        public int KetQuaNopAnPhiID { get; set; }
        public int HoSoVuAnID { get; set; }

        [Required(ErrorMessage = "Ngày nộp án phí không được để trống.")]
        public string NgayNopAnPhi { get; set; }

        [StringLength(100, ErrorMessage = "Số biên lai không được quá 100 kí tự.")]
        [Required(ErrorMessage = "Số biên lai không được để trống.")]
        public string SoBienLai { get; set; }

        [Required(ErrorMessage = "Ngày nộp biên lai cho tòa án không được để trống.")]
        public string NgayNopBienLaiChoToaAn { get; set; }

        [Required(ErrorMessage = "Người nhận biên lai không được để trống.")]
        public string NguoiNhanBienLai { get; set; }

        public NhanVienModel NhanVienNguoiNhanBienLai { get; set; }

        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string GhiChu { get; set; }
    }
}