using Biz.TACM.Models.ViewModel.HoSo;
using System.Collections.Generic;

namespace Biz.TACM.IServices
{
    public interface IHoSoService
    {
        IEnumerable<HoSoVuAnSummaryViewModel> Search(TruyVanHoSoViewModel query);
        bool UpdateState(int id, int newState);
    }
}