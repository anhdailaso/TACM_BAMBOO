using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.TACM.HuyHoSo.Model;
using Biz.TACM.Models.ViewModel.HuyHoSo;

namespace Biz.TACM.IServices
{
    public interface IHuyHoSoService
    {
        IEnumerable<HoSoVuAnModel> DanhSachHoSo();
        HoSoVuAnViewModel TimHoSo(string maHoSo);
        HoSoVuAnViewModel ChiTietHuy(int hoSoVuAnID);
        ResponseResult HuyHoSo(int hoSoVuAnID, string lyDoHuy, string nguoiHuy);
        ResponseResult SuaHuyHoSo(int hoSoVuAnID, string lyDoHuy);
    }
}
