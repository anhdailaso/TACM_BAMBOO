using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.TACM.Models.ViewModel.HuyHoSo
{
    public class HoSoVuAnViewModel
    {
        public int HoSoVuAnID { get; set; }
        public string MaHoSo { get; set; }
        public int SoHoSo { get; set; }
        public string TenVuAn { get; set; }
        public string NhomAn { get; set; }
        public string ToaAn { get; set; }
        public string GiaiDoan { get; set; }
        public string CongDoan { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LYDOHUY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LyDoHuy { get; set; }
        public string NguoiHuy { get; set; }
        public string NgayHuy { get; set; }
    }
}