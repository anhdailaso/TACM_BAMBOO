using Biz.TACM.Models.ViewModel.TaiKhoanNhanVien;
using System.Collections.Generic;
using System.ComponentModel;
using Biz.Lib.TACM.Resources.Resources;
using System.ComponentModel.DataAnnotations;
namespace Biz.TACM.Models.ViewModel.QuanLyNhanVien
{
    public class NhanVienViewModel
    {
        public int NhanvienID { get; set; }
        public int ToaAnID { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_MANV")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string MaNV { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_MANV")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string MaNVMoi { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_HO")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoNV { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TEN")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TenNV { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TENLOT")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        public string TenLotNV { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_GIOITINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string GioiTinh { get; set; }
        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYSINH")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgaySinh { get; set; }
        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_SODIENTHOAI")]
        //[RegularExpression("([0-9]+)", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRASO")]
        public string SoDienThoai { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_EMAIL")]
        [RegularExpression("([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+)", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRAEMAIL")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string Email { get; set; }
        public string DuongDanHinhDaiDien { get; set; }
        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_TENCHUCVU")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ChucVu { get; set; }
        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_CHUCDANH")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ChucDanh { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TRANGTHAI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int TrangThai { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_CHUCDANH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int SoDoToChucID { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TENCHUCVU")]
        public int? SoDoToChucChucVuID { get; set; }
        public bool TaoTaiKhoan { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string GhiChu { get; set; }
        public string HoVaTen
        {
            get { return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV; }
            set {; }
        }
        public string HoTenVaMaNV
        {
            get
            {

                return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV + (string.IsNullOrWhiteSpace(MaNV) ? " (" + MaNV + ")" : " (" + MaNVMoi + ")");
            }
        }
        public ChucNangViewModel ChucNangNhanVien { get; set; }
        public List<NhanVienThuKy> ListNhanVienThuKy { get; set; }
    }
    public class NhanVienThuKy
    {
        public int NhaanVienID { get; set; }
        public string MaNV { get; set; }
        public string HoVaTen { get; set; }
        public bool isckeck { get; set; }
        public string HoTenVaMaNV { get; set; }
        public string ChucDanh { get; set; }
    }
}