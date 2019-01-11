using System;
using System.Collections.Generic;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangCao.Request
{
    public class SuaKhangCaoRequest : KhangCaoRequestBase
    {
        public int KhangCaoId { get; set; }
        public List<String> DanhSachNguoiBiKhangCao { get; set; }
    }
}