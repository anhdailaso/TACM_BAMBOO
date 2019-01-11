using Biz.Lib.Helpers;
using Biz.TACM.Infrastructure.Caching;
using Biz.TACM.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Services
{
    public class TrangThaiService : ServiceBase, ITrangThaiService
    {
        protected readonly ICacheProvider CacheProvider;

        public TrangThaiService(ICacheProvider cacheProvider)
        {
            this.CacheProvider = cacheProvider;
        }

        public IEnumerable<DataItemModel> GetAvailableHosoStates()
        {
            var allStates = this.CacheProvider.Get<IEnumerable<DataItemModel>>(CacheKeys.AllAvailableStates);

            if(allStates == null)
            {
                allStates = XMLUtils.BindData("trangthai");

                this.CacheProvider.Set(CacheKeys.AllAvailableStates, allStates);
            }

            return allStates;
        }

        public IEnumerable<DataItemModel> GetCongDoanHoso()
        {
            var allStates = this.CacheProvider.Get<IEnumerable<DataItemModel>>(CacheKeys.CongDoanHoSoStates);

            if (allStates == null)
            {
                allStates = XMLUtils.BindData("CongDoanHoSo");

                this.CacheProvider.Set(CacheKeys.CongDoanHoSoStates, allStates);
            }

            return allStates;
        }

        public IEnumerable<DataItemModel> GetCongDoanHosoPhucTham()
        {
            var allStates = this.CacheProvider.Get<IEnumerable<DataItemModel>>(CacheKeys.CongDoanHoSoPhucThamStates);

            if (allStates == null)
            {
                allStates = XMLUtils.BindData("CongDoanHoSoPhucTham");

                this.CacheProvider.Set(CacheKeys.CongDoanHoSoPhucThamStates, allStates);
            }

            return allStates;
        }
    }
}