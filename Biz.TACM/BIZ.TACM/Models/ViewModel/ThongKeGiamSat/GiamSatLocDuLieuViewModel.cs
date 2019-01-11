using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.ThongKeGiamSat
{
    public class GiamSatLocDuLieuViewModel
    {
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TUNGAY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TuNgay { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_DENNGAY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string DenNgay { get; set; }

        public int? ToaAnID { get; set; }
        public string NhomAn { get; set; }
        public int? GiaiDoan { get; set; }
        public int Group { get; set; }
    }
}