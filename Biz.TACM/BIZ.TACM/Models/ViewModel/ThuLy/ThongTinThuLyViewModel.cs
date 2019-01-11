using Biz.Lib.TACM.Resources.Resources;
using System.ComponentModel.DataAnnotations;

namespace Biz.TACM.Models.ViewModel.ThuLy
{
    public class ThongTinThuLyViewModel
    {
        public int ThongTinThuLyID { get; set; }

        public int HoSoVuAnID { get; set; }

        public string SoThuLy { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TRUONGHOP_THULY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TruongHopThuLy { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THULY_THUTUC")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThuLyTheoThuTuc { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYTHULY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgayThuLy { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LOAI_QUANHE")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LoaiQuanHe { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_QUANHE_PHAPLUAT")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string QuanHePhapLuat { get; set; }

        public string ThoiHanGiaiQuyet { get; set; }

        public string ThoiHanGiaiQuyetTuNgay { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THOIHAN_GIAIQUYET")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThoiHanGiaiQuyetDenNgay { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOIDUNGYEUCAU_TOAAN_GIAIQUYET")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiDungYeuCau { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOIDUNG_KHANGCAO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiDungKhangCao { get; set; }

        public string TaiLieuChungTuKemTheo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_QUYETDINH_BIKHIEUNAI_KN_KC")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string QuyetDinh { get; set; }

        public string NhomNghiepVu { get; set; }

        public string GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public string NgayTao { get; set; }

        public string GhiChu { get; set; }
    }
}