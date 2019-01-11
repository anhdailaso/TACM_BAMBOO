using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Biz.TACM.Exceptions
{
    public class TacmServiceException : Exception
    {
        public TacmServiceException()
        {
        }

        public TacmServiceException(string message) : base(message)
        {
        }

        public TacmServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TacmServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}