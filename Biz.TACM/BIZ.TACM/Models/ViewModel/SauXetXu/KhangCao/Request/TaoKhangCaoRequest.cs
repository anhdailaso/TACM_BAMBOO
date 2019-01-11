using System;
using System.Collections.Generic;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangCao.Request
{
    public class TaoKhangCaoRequest : KhangCaoRequestBase
    {
        public int HoSoVuAnId { get; set; }
        public List<String> DanhSachNguoiBiKhangCao { get; set; }
    }
}