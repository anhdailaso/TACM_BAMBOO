using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class SuaDoiBoSungDonModel
    {
        public int SuaDoiBoSungDonID { get; set; }

        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYYEUCAU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayYeuCau { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THOIHANSUADOI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime ThoiHanSuaDoi { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOIDUNGYEUCAU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiDungYeuCau { get; set; }

        public string NhomNghiepVu { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }
    }
}
