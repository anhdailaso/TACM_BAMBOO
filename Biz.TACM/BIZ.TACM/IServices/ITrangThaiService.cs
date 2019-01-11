using Biz.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.TACM.IServices
{
    public interface ITrangThaiService
    {
        IEnumerable<DataItemModel> GetAvailableHosoStates();

        IEnumerable<DataItemModel> GetCongDoanHoso();

        IEnumerable<DataItemModel> GetCongDoanHosoPhucTham();
    }
}
