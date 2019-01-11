using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Biz.TACM.Models.ViewModel.NhanDon
{
    public class DuongSuViewModel
    {
        public int STT { get; set; }
        public int DuongSuID { get; set; }
        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_HOVATEN")]
        [StringLength(200, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoVaTen { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_COQUAN_TOCHUC")]
        [StringLength(200, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TenCoQuanToChuc { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_CMND")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        //[Required(ErrorMessage = "Số CMND không được để trống.")]
        public string SoCMND { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_DANTOC")]
        //[StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string DanToc { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYSINH")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgayThangNamSinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOISINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiSinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_GIOITINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string GioiTinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_SODIENTHOAI")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        //[Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression("([+0-9]+)", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRASO")]
        public string SoDienThoai { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_QUOCTICH")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string QuocTich { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOIDKKTT")]
        [StringLength(250, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiDKHKTT { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOTAMTRU")]
        [StringLength(250, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        public string NoiTamTru { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TUCACHTHAMGIA_TOTUNG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TuCachThamGiaToTung { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_DUONGSULA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string DuongSuLa { get; set; }

        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string GhiChu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TONGIAO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TonGiao { get; set; }
        
        public string NgayCapCMND { get; set; }
        public string NoiCapCMND { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_NGHENGHIEP")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgheNghiep { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TRINHDOVANHOA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TrinhDoVanHoa { get; set; }

        public string TienAn { get; set; }
        public string TienSu { get; set; }
        public string HoTenNguoiGiamHo { get; set; }
        public string DiaChiNguoiGiamHo { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_CON_ONG_HOVATEN")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoTenCha { get; set; }

        public string DiaChiCha { get; set; }
        public string NamSinhCha { get; set; }
        public bool ChaDaChetHayChua { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_CON_BA_HOVATEN")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoTenMe { get; set; }

        public string DiaChiMe { get; set; }
        public string NamSinhMe { get; set; }
        public bool MeDaChetHayChua { get; set; }
        public string TenGoiKhac { get; set; }
        public List<string> ListDacDiemNhanThanBiCao { get; set; }
        public string LuuYVeChucVu { get; set; }
        public string NgayBatTamGiam { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TINHTRANGGIAMGIU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TinhTrangGiamGiu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TOIDANHTRUYTO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ToiDanhTruyTo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_DIEULUATTRUYTO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string DieuLuatTruyTo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_VANPHONGLUATSU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string VanPhongLuatSu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_DOANLUATSU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string DoanLuatSu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOICONGTAC")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiCongTac { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGUOIDAIDIENCUA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int NguoiDaiDienCua { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGUOIBAOVECUA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int NguoiBaoVeCua { get; set; }

        public string QuanHeVoiNguoiThamGiaToTung { get; set; }
        public string ChucVu { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_DACDIEMNHANTHANBIHAI")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        //public List<string> ListDacDiemNhanThanBiHai { get; set; }
        public string DacDiemNhanThanBiHai { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGUOIBAOCHUALA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NguoiBaoChuaLa { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOICUTRU")]
        [StringLength(250, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiCuTru { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_DIACHITRUSO")]
        [StringLength(250, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string DiaChiTruSo { get; set; }

        public string HoTenNguoiDuocDaiDien { get; set; }

        public class DacDiemNhanThan
        {
            public bool IsChecked { get; set; }
            public string DacDiem { get; set; }
        }

    }
}