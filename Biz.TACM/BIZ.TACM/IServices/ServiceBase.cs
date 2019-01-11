using Biz.Lib.Helpers;
using Biz.Lib.TACM.NhanDon.Exceptions;
using Biz.TACM.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.IServices
{
    public abstract class ServiceBase
    {
        protected virtual void SafeExecute(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch(DataAccessException ex)
            {
                WriteLog.Exeption(ex);

                throw new TacmServiceException($"Exception when invoke the method {action.Method.Name}", ex);
            }
        }
    }
}