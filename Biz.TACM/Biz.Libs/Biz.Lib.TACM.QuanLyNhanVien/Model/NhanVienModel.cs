using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Biz.Lib.TACM.QuanLyNhanVien.Model
{
    public class NhanVienModel
    {
        public int NhanvienID { get; set; }
        public int ToaAnID { get; set; }
        public string MaNV { get; set; }
        public string MaNVMoi { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_HO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoNV { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TEN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TenNV { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TENLOT")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TenLotNV { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_GIOITINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string GioiTinh { get; set; }

        public DateTime? NgaySinh { get; set; }

        public string SoDienThoai { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_EMAIL")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string Email { get; set; }
        public string DuongDanHinhDaiDien { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_CHUCDANH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ChucDanh { get; set; }
        public string ChucVu { get; set; }
        public string TenToaAn { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TRANGTHAI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int TrangThai { get; set; }
        public string NhiemVuNV { get; set; }
        public string NhomChucNang { get; set; }
        public int SoDoToChucID { get; set; }
        public int? SoDoToChucChucVuID { get; set; }
        public bool TaoTaiKhoan { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
        public string MaNVThuKy { get; set; }
        public string HoVaTen
        {
            get { return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV; }
            set {; }
        }
        public string HoTenVaMaNV
        {
            get
            {
                return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV + (string.IsNullOrWhiteSpace(MaNVMoi) ? " (" + MaNV + ")" : " (" + MaNVMoi + ")");
            }
        }
    }
}