using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.TACM.GiamDocThamTaiTham.Model;

namespace Biz.Lib.TACM.GiamDocThamTaiTham.IDataAccess
{
    public interface IGiamDocThamTaiThamDataAccess
    {
        IEnumerable<GiamDocThamTaiThamViewListModel> DanhSachGDTTT();
        GiamDocThamTaiThamModel ChiTietGDTTT(int gDTTTid);
        ResponseResult SuaGDTTT(GiamDocThamTaiThamModel model);
        ResponseResult XoaGDTTT(int gDTTTid, string NguoiTao);
        HoSoVuAnModel TimHoSo(string maHoSo);
        List<MaHoSoModel> DanhSachHoSo(string nhomAn);
    }
}
