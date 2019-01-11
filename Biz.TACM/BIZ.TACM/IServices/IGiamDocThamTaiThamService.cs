using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.TACM.Models.ViewModel.GiamDocThamTaiTham;
using Biz.Lib.TACM.GiamDocThamTaiTham.Model;

namespace Biz.TACM.IServices
{
    public interface IGiamDocThamTaiThamService
    {
        IEnumerable<GiamDocThamTaiThamViewListModel> DanhSachGDTTT();
        GiamDocThamTaiThamViewModel ChiTietGDTTT(int gDTTTid);
        ResponseResult SuaGDTTT(GiamDocThamTaiThamViewModel model);
        ResponseResult XoaGDTTT(int gDTTTid, string NguoiTao);
        HoSoVuAnViewModel TimHoSo(string maHoSo);
        List<MaHoSoModel> DanhSachHoSo(string nhomAn);
    }
}
