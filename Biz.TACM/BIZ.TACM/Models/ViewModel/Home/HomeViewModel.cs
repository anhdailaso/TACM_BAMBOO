using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.Lib.SettingData.Model;

namespace Biz.TACM.Models.ViewModel.Home
{
    public class HomeViewModel
    {
        public List<ToaAnModel> DSToaAn { get; set; }
        public List<SettingItemModel> NghiepVu { get; set; }
    }
}