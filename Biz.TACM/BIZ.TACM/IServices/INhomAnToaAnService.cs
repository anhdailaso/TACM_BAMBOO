using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.TACM.NhomAnToaAn.Model;

namespace Biz.TACM.IServices
{
    public interface INhomAnToaAnService
    {
        IEnumerable<NhomAnModel> NhomAnTheoToaAn(int ToaAnID);

        ResponseResult ChonNhomAn(int ToaAnID, string MaNhomAn, string TenNhomAn);
        ResponseResult HuyChonNhomAn(int NhomAnID);
    }
}
