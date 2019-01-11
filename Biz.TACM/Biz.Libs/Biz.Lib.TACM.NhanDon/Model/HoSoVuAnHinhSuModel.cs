using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class HoSoVuAnHinhSuModel
    {
        public int HoSoVuAnID { get; set; }

        public string MaHoSo { get; set; }

        public int SoHoSo { get; set; }

        public int ToaAnID { get; set; }

        public string NhomAn { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public string TrangThaiCongDoan { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TENVUAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TenVuAn { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_SOBICAN")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SONGUYENDUONG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int? SoBiCan { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TONGSOBUTLUC")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SONGUYENDUONG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int? TongSoButLuc { get; set; }

        public string KhongCoCacButLuc { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TRUYTOBANG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TruyToBang { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_SO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string SoTruyTo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYTHANGNAM")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime? NgayTruyTo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_VKS_TRUYTO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string VienKiemSatTruyTo { get; set; }

        public string VienKiemSatTruyToKhac { get; set; }

        public string CanBoNhanDon { get; set; }

        public DateTime NgayNopDonTaiToaAn { get; set; }

        public int TrangThai { get; set; }

        public string NguoiCapNhat { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

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

        public NhanVienModel NhanVienCanBoNhanDon { get; set; }
    }
}
