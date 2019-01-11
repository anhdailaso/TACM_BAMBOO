using System;
using Biz.Lib.TACM.NhomAnToaAn.Model;
using Biz.TACM.Infrastructure.Attributes;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Helpers;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.IO;
using Biz.Lib.TACM.Resources.Resources;
using Biz.Lib.Authentication;
using Biz.Lib.SettingData.Model;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.Models.ViewModel.NhomAnToaAn;
namespace Biz.TACM.Controllers
{
    public class NhomAnToaAnController : BizController
    {
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        private readonly INhomAnToaAnService _nhomAnToaAnService;
        public NhomAnToaAnController(INhomAnToaAnService NhomAnToaAnService)
        {
            _nhomAnToaAnService = NhomAnToaAnService;
        }
        public ActionResult Index()
        {
            int id = GetAnSessionInfo().ToaAnId;
            ViewBag.listToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnIDTheoNV(id, AccountUtils.CurrentUsername());
            var list = SettingDataService.ListSettingDataItem("DINHNGHIACHUNG_NHOMANTOANAN").OrderBy(x => x.ItemValue).ToList();
            List<NhomAnViewModel> rtmodel = new List<NhomAnViewModel>();
            for (int i = 0; i < list.Count(); i++)
            {
                NhomAnViewModel a = new NhomAnViewModel();
                a.Stt = i + 1;
                a.ma = list[i].ItemRemark;
                a.text = list[i].ItemText;
                a.ischeck = true;
                rtmodel.Add(a);
            }
            var nhomAn = _nhomAnToaAnService.NhomAnTheoToaAn(id);
            foreach (var an in rtmodel)
            {
                var temp = nhomAn.Where(x => x.MaNhomAn == an.ma && x.TrangThai == 1).FirstOrDefault();
                if (temp != null)
                    an.ischeck = true;
                else an.ischeck = false;
            }
            return View(rtmodel.OrderBy(x=>x.text).ToList());
        }
        public ActionResult NhomAnTheoToaAn(int id)
        {
            var list = SettingDataService.ListSettingDataItem("DINHNGHIACHUNG_NHOMANTOANAN").OrderBy(x => x.ItemValue).ToList();
            List<NhomAnViewModel> rtmodel = new List<NhomAnViewModel>();
            for (int i = 0; i < list.Count(); i++)
            {
                NhomAnViewModel a = new NhomAnViewModel();
                a.Stt = i + 1;
                a.ma = list[i].ItemRemark;
                a.text = list[i].ItemText;
                a.ischeck = true;
                rtmodel.Add(a);
            }
            var nhomAn = _nhomAnToaAnService.NhomAnTheoToaAn(id);
            foreach (var an in rtmodel)
            {
                var temp = nhomAn.Where(x => x.MaNhomAn == an.ma && x.TrangThai == 1).FirstOrDefault();
                if (temp != null)
                    an.ischeck = true;
                else an.ischeck = false;
            }
            return PartialView("~/Views/NhomAnToaAn/_CheckList.cshtml", rtmodel.OrderBy(x => x.text).ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Luu(List<NhomAnViewModel> model, int ToaAnID)
        {
            var nhomAn = _nhomAnToaAnService.NhomAnTheoToaAn(ToaAnID);
            try
            {
                for (int i = 0; i < model.Count(); i++)
                {
                    ResponseResult result = new ResponseResult();
                    if (model[i].ischeck)
                    {
                        result = _nhomAnToaAnService.ChonNhomAn(ToaAnID, model[i].ma, model[i].text);
                    }
                    else if (model[i].ischeck != true)
                    {
                        var temp = nhomAn.Where(x => x.MaNhomAn == model[i].ma && x.TrangThai == 1).FirstOrDefault();
                        if (temp != null && temp.TrangThai == 1)
                            result = _nhomAnToaAnService.HuyChonNhomAn(temp.NhomAnID);
                        else
                        {
                            result.ResponseCode = 1;
                            result.ResponseMessage = "OK";
                        }
                    }
                    if (result == null || result.ResponseCode != 1)
                        return Json((JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGLUU_NHOMAN)));
                }
                return Json(JsonResponseViewModel.CreateSuccess());
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));

            }
        }
    }

}