using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Linq;
using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.TACM.Enums;
using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.Home;

namespace Biz.TACM.Controllers
{
    public class HomeController : BizController
    {
        private readonly ISettingDataService _settingDataService;

        public HomeController(ISettingDataService settingDataService)
        {
            _settingDataService = settingDataService;
        }

        [CustomAuthorize]
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            var DSToaAn = _settingDataService.DanhSachToaAnTheoNV(AccountUtils.CurrentUsername());
            model.DSToaAn = DSToaAn;

            ViewBag.toaAnActive = GetAnSessionInfo().ToaAnId;
            return View(model);
        }

        public ActionResult DanhSachNhomAn(int toaAnId)
        {
            UpdateAnSessionInfo(idToaAn: toaAnId, maToaAn: GetAnSessionInfo().MaToaAn, idGiaiDoan: GiaiDoan.SoTham.GetHashCode());
            var viewModel = _settingDataService.ListSettingDataItem("DINHNGHIACHUNG_NHOMANTOANAN");
            viewModel = viewModel.OrderBy(x => x.ItemText).ToList();
            if (viewModel.Any())
            {
                if (viewModel.First().ItemRemark == "AD" && viewModel.Count > 1)
                {
                    var temp = viewModel.First();
                    viewModel.RemoveAt(0);
                    viewModel.Add(temp);
                }
            }
            ViewBag.TenToaAn = _settingDataService.GetToaAnTheoToaAnID(toaAnId).TenToaAn.Replace("TAND", "TOÀ ÁN NHÂN DÂN");
            return PartialView("_CacLoaiAn", viewModel);
        }

        //using this action to return the view which convert resources to javascript object
        public ActionResult ConvertResourcesToJavascript()
        {
            Response.ContentType = "text/javascript";
            return View("../Shared/ConvertResourcesToJavascript");
        }
        public ActionResult ChangeTitleHeader()
        {   
            return PartialView("../Shared/_PageTitleHeader");
        }
        public ActionResult ChangeLoginPartial()
        {
            return PartialView("../Shared/_LoginPartial");
        }
    }
}