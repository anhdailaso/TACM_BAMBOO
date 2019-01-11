using Biz.Lib.TACM.SoDoToChuc.Models;
using Biz.TACM.Models.ViewModel.SoDoToChuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Biz.TACM.IServices
{
    public interface ISoDoToChucService
    {
        IEnumerable<ChucDanhViewModel> DanhSachChucDanhTheoToaAn(int? toaAnId);
        IEnumerable<ChucDanhViewModel> DanhSachChucVuTheoToaAn(int? toaAnId);
        ChucDanhViewModel ChiTietChucDanhTheoId(int? soDoToChucId); //chucDanhId
        SelectList DanhSachChucDanhCha(string selectedValue, int? toaAnId);
        SelectList DanhSachChucVuCha(string selectedValue, int? toaAnId);
        ResponseResult ThemChucDanh(ChucDanhViewModel viewModel);
        ResponseResult SuaChucDanh(ChucDanhViewModel viewModel);
        ResponseResult XoaChucDanh(int soDoToChucId);
    }
}