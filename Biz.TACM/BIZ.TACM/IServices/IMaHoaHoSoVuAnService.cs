using Biz.TACM.Models.ViewModel.NhanDon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Biz.TACM.IServices
{
    public interface IMaHoaHoSoVuAnService
    {
        string[] TestEditingFileWord(string fileName, string folderPath, List<DuongSuViewModel> DanhSachDuongSu);
        SelectList DanhSachTuCachThamGiaToTung();
    }
}
