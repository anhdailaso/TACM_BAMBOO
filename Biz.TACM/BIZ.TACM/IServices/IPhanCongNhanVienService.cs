using System;
using Biz.Lib.TACM.PhanCongNhanVien.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.TACM.IServices
{
    public interface IPhanCongNhanVienService
    {
        IEnumerable<NhomAnNhanVienModel> DSNhomAn(int ToaAnID);
        IEnumerable<NhanVienNhomAnModel> DSNhanVienNhomAn(int NhomAnID);
        IEnumerable<NhanVienModel> DSNhanVienTheoChucDanh(int ToaAnID, int? SoDoToChucID);
        ResponseResult ChonNhomAnNV(int NhanVienID, int NhomAnID);
        ResponseResult HuyChonNhomAnNV_log(int NhanVienNhomAnID, int NhanVienID, int NhomAnID);

    }
}
