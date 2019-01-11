using Biz.Lib.TACM.SoDoToChuc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.SoDoToChuc.IDataAccess
{
    public interface ISoDoToChucDataAccess
    {
        IEnumerable<ChucDanhModel> DanhSachChucDanhTheoToaAn(int? toaAnId);
        ChucDanhModel ChiTietChucDanhTheoId(int? soDoToChucId); //chucDanhId
        ResponseResult ThemChucDanh(ChucDanhModel model);
        ResponseResult SuaChucDanh(ChucDanhModel model);
        ResponseResult XoaChucDanh(int soDoToChucId);
    }
}
