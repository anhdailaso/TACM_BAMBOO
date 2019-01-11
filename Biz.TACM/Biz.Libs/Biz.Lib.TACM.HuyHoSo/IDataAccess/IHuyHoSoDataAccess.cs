using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.TACM.HuyHoSo.Model;

namespace Biz.Lib.TACM.HuyHoSo.IDataAccess
{
    public interface IHuyHoSoDataAccess
    {
        IEnumerable<HoSoVuAnModel> DanhSachHoSo();
        HoSoVuAnModel TimHoSo(string maHoSo);
        HoSoVuAnModel ChiTietHuy(int hoSoVuAnID);
        ResponseResult HuyHoSo(int hoSoVuAnID, string lyDoHuy, string nguoiHuy);
        ResponseResult SuaHuyHoSo(int hoSoVuAnID, string lyDoHuy);
    }
}
