using System;
using Biz.Lib.TACM.NhomAnToaAn.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.NhomAnToaAn.IDataAccess
{
    public interface INhomAnToaAnDataAccess
    {
        IEnumerable<NhomAnModel> NhomAnTheoToaAn(int ToaAnID);
        ResponseResult ChonNhomAn(int ToaAnID, string MaNhomAn, string TenNhomAn, string NguoiTao);
        ResponseResult HuyChonNhomAn(int NhomAnID);
    }
}
