using Biz.TACM.Models.ViewModel.CongThongTinDienTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.TACM.IServices
{
    public interface ITraCuuThongTinService
    {
        TraCuuHoSoVuAnViewModel GetGetHoSoVuAnDuocTraCuu(string keyword);
    }
}
