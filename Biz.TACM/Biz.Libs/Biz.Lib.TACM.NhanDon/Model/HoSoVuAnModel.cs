using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.SettingData.Model;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class HoSoVuAnModel
    {
        public int HoSoVuAnID { get; set; }
        
        public string MaHoSo { get; set; }
        
        public int SoHoSo { get; set; }

        public int ToaAnID { get; set; }

        public string NhomAn { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }
        
        public string TrangThaiCongDoan { get; set; }

        public string TenVuAn { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYLAMDON")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime? NgayLamDon { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYNOPDON_TOAAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime? NgayNopDonTaiToaAn { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_HINHTHUC_GUIDON")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HinhThucGoiDon { get; set; }

        public string HinhThucGoiDonKhac { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LOAIDON")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LoaiDon { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LOAI_QUANHE")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LoaiQuanHe { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_QUANHE_PHAPLUAT")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string QuanHePhapLuat { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_YEUTO_NUOCNGOAI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string YeuToNuocNgoai { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGUOIKY_XACNHAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NguoiKyXacNhanDaNhanDon { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_CANBO_NHANDON")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string CanBoNhanDon { get; set; }

        public string ThuLyTheoThuTuc { get; set; }

        public int TrangThai { get; set; }

        public string NguoiCapNhat { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public int LoaiKetQuaXetXu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOIDUNGDON")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiDungDon { get; set; }

        //quan he table khac
        public IEnumerable<DuongSuModel> DuongSu { get; set; }

        public string ThamPhan { get; set; }

        public string ThamPhan1 { get; set; }

        public string ThamPhan2 { get; set; }

        public string ThamPhanKhac { get; set; }

        public string HoiThamNhanDan { get; set; }

        public string HoiThamNhanDan2 { get; set; }

        public string HoiThamNhanDan3 { get; set; }

        public string ThuKy { get; set; }

        public string KiemSatVien { get; set; }

        public int HoiDong { get; set; }

        public DateTime? NgayChuyenDon { get; set; }

        public DateTime? NgayTraDon { get; set; }

        public DateTime? NgayKhieuNai { get; set; }
        
        public string HoNV { get; set; }
        
        public string TenNV { get; set; }
        
        public string TenLotNV { get; set; }

        public NhanVienModel NhanVienNguoiKyXacNhanDaNhanDon { get; set; }

        public NhanVienModel NhanVienThamPhan { get; set; }

        public NhanVienModel NhanVienCanBoNhanDon { get; set; }

        public NhanVienModel NhanVienNguoiLapDon { get; set; }
    }
}
