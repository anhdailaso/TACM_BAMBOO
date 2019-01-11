using Biz.Lib.TACM.CongThongTinDienTu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.CongThongTinDienTu.IDataAccess
{
    public interface ITraCuuThongTinDataAccess
    {
        TraCuuHoSoVuAnModel GetHoSoVuAnDuocTraCuu(string keyword, ref int hoSoVuAnID);
    }
}
