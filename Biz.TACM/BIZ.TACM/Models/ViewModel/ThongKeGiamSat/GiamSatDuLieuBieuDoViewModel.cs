using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.ThongKeGiamSat
{
    public class GiamSatDuLieuBieuDoViewModel
    {
        public string GroupName { get; set; }
        public int Amount { get; set; }
    }
    public class GiamSatViewModel
    {
        public List<GiamSatDuLieuBieuDoViewModel> ListData { get; set; }

        public string ListHoSoVuAnID { get; set; }
    }
}